using sick_ahh_farming_game.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sick_ahh_farming_game.Services
{
    public class GameService
    {

        private SQLiteAsyncConnection _database = null!;
        private bool _initialized;
        private readonly SemaphoreSlim _initLock = new(1, 1);

        public async Task InitializeAsync()
        {
            if (_initialized) return;

            await _initLock.WaitAsync();
            try
            {
                if (_initialized) return;

                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "farming.db3");
                _database = new SQLiteAsyncConnection(dbPath,
                    SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);

                await _database.CreateTableAsync<Seed>();
                await _database.CreateTableAsync<Plot>();
                await _database.CreateTableAsync<InventoryItem>();
                await _database.CreateTableAsync<PlayerStat>();


                _initialized = true;
            }
            finally
            {
                _initLock.Release();
            }
        }
        private async Task SeedDefaultsAsync()
        {
            if (await _database.Table<Seed>().CountAsync() > 0) return;

            var seeds = new List<Seed>
            {
                // Add type of seeds here 
                //
                // new()
                // {
                //     Name = "",
                //     Emoji = "",
                //     Cost = 0,
                //     SellValue = 0,
                //     GrowthDurationSeconds = 0
                // }

            };

            await _database.InsertAllAsync(seeds);
        }


        private async Task EnsurePlotsAsync()
        {
            if (await _database.Table<Plot>().CountAsync() > 0) return;

            var plots = Enumerable.Range(1, 12)
                .Select(i => new Plot { Name = $"Plot{i}" })
                .ToList();

            await _database.InsertAllAsync(plots);
        }

        private async Task EnsurePlayerAsync()
        {
            var player = await _database.Table<PlayerStat>().FirstOrDefaultAsync();
            if (player != null) return;

            await _database.InsertAsync(new PlayerStat { Coins = 50 });
        }

        public async Task<PlayerStat> GetPlayerAsync()
        {
            await InitializeAsync();
            return await _database.Table<PlayerStat>().FirstOrDefaultAsync()
                ?? new PlayerStat { Coins = 0 };
        }

        public async Task<List<Seed>> GetSeedsAsync()
        {
            await InitializeAsync();
            return await _database.Table<Seed>().ToListAsync();
        }

        public async Task<Seed?> GetSeedAsync(int id)
        {
            await InitializeAsync();
            return await _database.Table<Seed>().Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Plot>> GetPlotsAsync()
        {
            await InitializeAsync();

            var plots = await _database.Table<Plot>().ToListAsync();
            var seeds = await _database.Table<Seed>().ToListAsync();

            foreach (var plot in plots)
            {
                if (plot.SeedId.HasValue)
                    plot.Seed = seeds.FirstOrDefault(s => s.Id == plot.SeedId.Value);
            }

            return plots;
        }

        public async Task<List<InventoryItem>> GetInventoryAsync()
        {
            await InitializeAsync();

            var items = await _database.Table<InventoryItem>().ToListAsync();
            var seeds = await _database.Table<Seed>().ToListAsync();

            foreach (var item in items)
                item.Seed = seeds.FirstOrDefault(s => s.Id == item.SeedId);

            return items;
        }

        public async Task<bool> PlantSeedAsync(int plotId, int seedId)
        {
            await InitializeAsync();

            var plot = await _database.Table<Plot>().Where(p => p.Id == plotId).FirstOrDefaultAsync();
            if (plot == null || plot.SeedId.HasValue) return false;

            var inventory = await _database.Table<InventoryItem>()
                .Where(i => i.SeedId == seedId)
                .FirstOrDefaultAsync();

            if (inventory == null || inventory.Quantity <= 0) return false;

            inventory.Quantity--;
            plot.SeedId = seedId;
            plot.PlantedTime = DateTime.UtcNow;

            await _database.UpdateAsync(inventory);
            await _database.UpdateAsync(plot);
            return true;
        }

        public async Task<(bool Success, string Message)> HarvestPlotAsync(int plotId)
        {
            await InitializeAsync();

            var plot = await _database.Table<Plot>().Where(p => p.Id == plotId).FirstOrDefaultAsync();
            if (plot == null || !plot.SeedId.HasValue)
                return (false, "Plot empty.");

            var seed = await GetSeedAsync(plot.SeedId.Value);
            if (seed == null)
                return (false, "Unknown crop.");

            var readyAt = plot.PlantedTime!.Value.AddSeconds(seed.GrowthDurationSeconds);
            if (DateTime.UtcNow < readyAt)
                return (false, "Crop is still growing.");

            var player = await GetPlayerAsync();
            player.Coins += seed.SellValue;
            player.PlantsHarvested++;

            plot.SeedId = null;
            plot.PlantedTime = null;

            await _database.UpdateAsync(player);
            await _database.UpdateAsync(plot);

            return (true, $"You harvested {seed.Name} for {seed.SellValue} coins!");
        }

        public async Task<(bool Success, string Message)> BuySeedAsync(int seedId)
        {
            await InitializeAsync();

            var seed = await GetSeedAsync(seedId);
            if (seed == null)
                return (false, "Seed not found.");

            var player = await GetPlayerAsync();
            if (player.Coins < seed.Cost)
                return (false, "Not enough coins.");

            player.Coins -= seed.Cost;

            var inventory = await _database.Table<InventoryItem>()
                .Where(i => i.SeedId == seedId)
                .FirstOrDefaultAsync();

            if (inventory == null)
            {
                await _database.InsertAsync(new InventoryItem { SeedId = seedId, Quantity = 1 });
            }
            else
            {
                inventory.Quantity++;
                await _database.UpdateAsync(inventory);
            }

            await _database.UpdateAsync(player);
            return (true, $"Bought {seed.Name}.");
        }

        public async Task SaveGameAsync()
        {
            await InitializeAsync();
            await _database.ExecuteAsync("PRAGMA wal_checkpoint;");
        }

        public async Task LoadGameAsync()
        {
            await InitializeAsync();
        }


    }
}
