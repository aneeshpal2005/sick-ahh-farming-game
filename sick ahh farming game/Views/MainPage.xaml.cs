using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public partial class MainPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshUIAsync();
    }

    private async Task RefreshUIAsync()
    {
        // Update player gold display
        var player = await _gameService.GetPlayerAsync();
        CoinsLabel.Text = $"💰 {player.Coins} G";

        // Update plot buttons visual states for all 12 plots
        var plots = await _gameService.GetPlotsAsync();
        foreach (var plot in plots)
        {
            var button = FindByName($"Plot{plot.Id}") as Button;
            if (button != null)
            {
                if (!plot.SeedId.HasValue)
                {
                    button.Text = ""; // Empty plot
                }
                else if (!plot.IsWatered)
                {
                    button.Text = $"{plot.Seed?.Emoji ?? "🌱"} 💧"; // Thirsty crop
                }
                else
                {
                    var check = await _gameService.CheckPlotAsync(plot.Id);
                    if (check.CanHarvest)
                    {
                        button.Text = $"{plot.Seed?.Emoji ?? "🌱"} ✨"; // Ready to harvest
                    }
                    else
                    {
                        button.Text = $"{plot.Seed?.Emoji ?? "🌱"} ⏳"; // Growing
                    }
                }
            }
        }
    }

    private async void FarmButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void InventoryButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InventoryPage));
    }

    private async void ShopButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ShopPage));
    }

    private async void AccountButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AccountPage));
    }

    private async void PlotButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        int plotId = int.Parse(button.AutomationId.Replace("Plot", ""));

        var result = await _gameService.CheckPlotAsync(plotId);

        if (result.IsEmpty)
        {
            _gameService.SelectedPlotId = plotId;
            await Shell.Current.GoToAsync(nameof(InventoryPage));
        }
        else if (!result.CanHarvest && result.Message.Contains("thirsty"))
        {
            // Smooth watering animation sequence without popup interruptions: 🪣 -> 💧
            button.Text = "🪣";
            await Task.Delay(200);
            button.Text = "💧";
            await Task.Delay(200);

            await _gameService.WaterPlotAsync(plotId);
            await RefreshUIAsync();
        }
        else if (result.CanHarvest)
        {
            var harvestResult = await _gameService.HarvestPlotAsync(plotId);
            await DisplayAlert(
                harvestResult.Success ? "Harvest" : "Farm",
                harvestResult.Message,
                "OK");
            await RefreshUIAsync();
        }
        else
        {
            await DisplayAlert("Farm", result.Message, "OK");
            await RefreshUIAsync();
        }
    }
}