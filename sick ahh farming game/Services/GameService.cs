using sick_ahh_farming_game.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sick_ahh_farming_game.Services
{
    public class GameService
    {
        private SQLiteAsyncConnection _database = null!;
        private bool _initialized;
        private readonly SemaphoreSlim _initLock = new(1, 1);
        public int SelectedPlotId { get; set; }

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
                await SeedDefaultsAsync();
                await EnsurePlotsAsync();
                await EnsurePlayerAsync();

                _initialized = true;
            }
            finally
            {
                _initLock.Release();
            }
        }

        private async Task SeedDefaultsAsync()
        {
            var seeds = new List<Seed>
            {
                new() { Id = 1, Name = "Carrot", Emoji = "🥕", Cost = 5, SellValue = 6, GrowthDurationSeconds = 5 },
                new() { Id = 2, Name = "Corn", Emoji = "🌽", Cost = 10, SellValue = 12, GrowthDurationSeconds = 15 },
                new() { Id = 3, Name = "Tomato", Emoji = "🍅", Cost = 15, SellValue = 16, GrowthDurationSeconds = 30 },
                new() { Id = 4, Name = "Potato", Emoji = "🥔", Cost = 20, SellValue = 22, GrowthDurationSeconds = 30 },
                new() { Id = 5, Name = "Eggplant", Emoji = "🍆", Cost = 25, SellValue = 27, GrowthDurationSeconds = 45 },
                new() { Id = 6, Name = "Pepper", Emoji = "🫑", Cost = 30, SellValue = 33, GrowthDurationSeconds = 45 }
            };

            foreach (var seed in seeds)
            {
                await _database.InsertOrReplaceAsync(seed);
            }
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

            await _database.InsertAsync(new PlayerStat { Username = "Player", Coins = 200, PlantsHarvested = 0, TotalMoneyMade = 0 });

            var carrotSeed = await _database.Table<Seed>().Where(s => s.Name == "Carrot").FirstOrDefaultAsync();
            if (carrotSeed != null)
            {
                await _database.InsertAsync(new InventoryItem { SeedId = carrotSeed.Id, Quantity = 3 });
            }
        }

        public async Task<PlayerStat> GetPlayerAsync()
        {
            await InitializeAsync();
            return await _database.Table<PlayerStat>().FirstOrDefaultAsync()
                ?? new PlayerStat { Username = "Player", Coins = 200 };
        }

        public async Task UpdateUsernameAsync(string newUsername)
        {
            await InitializeAsync();
            var player = await GetPlayerAsync();
            player.Username = string.IsNullOrWhiteSpace(newUsername) ? "Player" : newUsername;
            await _database.UpdateAsync(player);
        }

        public async Task ResetGameAsync()
        {
            await InitializeAsync();
            await _database.DropTableAsync<Seed>();
            await _database.DropTableAsync<Plot>();
            await _database.DropTableAsync<InventoryItem>();
            await _database.DropTableAsync<PlayerStat>();

            await _database.CreateTableAsync<Seed>();
            await _database.CreateTableAsync<Plot>();
            await _database.CreateTableAsync<InventoryItem>();
            await _database.CreateTableAsync<PlayerStat>();

            _initialized = false;
            await InitializeAsync();
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
            plot.IsWatered = false;
            plot.PlantedTime = null;

            await _database.UpdateAsync(inventory);
            await _database.UpdateAsync(plot);
            return true;
        }

        public async Task<(bool Success, string Message)> WaterPlotAsync(int plotId)
        {
            await InitializeAsync();
            var plot = await _database.Table<Plot>().Where(p => p.Id == plotId).FirstOrDefaultAsync();

            if (plot == null || !plot.SeedId.HasValue)
                return (false, "Nothing to water here!");

            if (plot.IsWatered)
                return (false, "This crop is already watered!");

            plot.IsWatered = true;
            plot.PlantedTime = DateTime.UtcNow;
            await _database.UpdateAsync(plot);

            return (true, "Watered successfully! 🌱💧");
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

            if (!plot.IsWatered)
                return (false, "Crop is thirsty! Water it first.");

            var readyAt = plot.PlantedTime!.Value.AddSeconds(seed.GrowthDurationSeconds);
            if (DateTime.UtcNow < readyAt)
                return (false, "Crop is still growing.");

            var player = await GetPlayerAsync();
            player.Coins += seed.SellValue;
            player.PlantsHarvested++;
            player.TotalMoneyMade += seed.SellValue; // <--- Tracks lifetime earnings!

            plot.SeedId = null;
            plot.PlantedTime = null;
            plot.IsWatered = false;

            await _database.UpdateAsync(player);
            await _database.UpdateAsync(plot);

            return (true, $"You harvested {seed.Name} for {seed.SellValue} coins!");
        }

        public async Task<(bool Success, string Message)> BuySeedAsync(int seedId, int quantity = 1)
        {
            await InitializeAsync();

            var seed = await GetSeedAsync(seedId);
            if (seed == null)
                return (false, "Seed not found.");

            int totalCost = seed.Cost * quantity;
            var player = await GetPlayerAsync();
            if (player.Coins < totalCost)
                return (false, $"Not enough coins! Need {totalCost} G.");

            player.Coins -= totalCost;

            var inventory = await _database.Table<InventoryItem>()
                .Where(i => i.SeedId == seedId)
                .FirstOrDefaultAsync();

            if (inventory == null)
            {
                await _database.InsertAsync(new InventoryItem { SeedId = seedId, Quantity = quantity });
            }
            else
            {
                inventory.Quantity += quantity;
                await _database.UpdateAsync(inventory);
            }

            await _database.UpdateAsync(player);
            return (true, $"Bought {quantity}x {seed.Name}(s) for {totalCost} G.");
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

        public async Task<(bool IsEmpty, bool CanHarvest, string Message)> CheckPlotAsync(int plotId)
        {
            await InitializeAsync();
            var plot = await _database.Table<Plot>().Where(p => p.Id == plotId).FirstOrDefaultAsync();
            if (plot == null)
                return (true, false, "Plot not found.");
            if (!plot.SeedId.HasValue)
                return (true, false, "Plot is empty.");

            var seed = await GetSeedAsync(plot.SeedId.Value);
            if (seed == null)
                return (false, false, "Unknown crop.");

            if (!plot.IsWatered)
                return (false, false, $"Crop {seed.Name} is thirsty! 💧 Tap to water.");

            var readyAt = plot.PlantedTime!.Value.AddSeconds(seed.GrowthDurationSeconds);
            if (DateTime.UtcNow >= readyAt)
                return (false, true, $"Crop {seed.Name} is ready to harvest!");

            var timeLeft = readyAt - DateTime.UtcNow;
            return (false, false, $"Crop {seed.Name} is growing. Time left: {timeLeft:mm\\:ss}");
        }
    }
}