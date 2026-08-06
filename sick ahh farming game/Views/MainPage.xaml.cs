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

        // Update plot visual layers for all 12 plots
        var plots = await _gameService.GetPlotsAsync();
        foreach (var plot in plots)
        {
            var textLabel = FindByName($"TextPlot{plot.Id}") as Label;
            var cropImg = FindByName($"CropPlot{plot.Id}") as Image;
            var overlayImg = FindByName($"OverlayPlot{plot.Id}") as Image;

            if (textLabel != null)
            {
                if (!plot.SeedId.HasValue)
                {
                    // Empty Plot: Clear all layers and text
                    textLabel.Text = "";
                    if (cropImg != null) cropImg.Source = null;
                    if (overlayImg != null) overlayImg.Source = null;
                }
                else if (!plot.IsWatered)
                {
                    // Planted but Thirsty: Sprout underneath + Water Drop overlay prompt
                    if (cropImg != null) cropImg.Source = "plant_sprout.png";
                    if (overlayImg != null) overlayImg.Source = "water_drop.png";
                    textLabel.Text = plot.Seed?.Emoji ?? "🌱";
                }
                else
                {
                    // Watered Plot: Sprout underneath + Watered overlay on top!
                    if (cropImg != null) cropImg.Source = "plant_sprout.png";
                    if (overlayImg != null) overlayImg.Source = "watered.png";

                    var check = await _gameService.CheckPlotAsync(plot.Id);
                    if (check.CanHarvest)
                    {
                        // Ready for harvest: Crop Emoji + Sparkles!
                        textLabel.Text = $"{plot.Seed?.Emoji ?? "🌱"} ✨";
                    }
                    else
                    {
                        // Growing: Crop Emoji + Hourglass
                        textLabel.Text = $"{plot.Seed?.Emoji ?? "🌱"} ⏳";
                    }
                }
            }
        }
    }

    private async void FarmButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//MainPage");
    private async void InventoryButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(InventoryPage));
    private async void ShopButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(ShopPage));
    private async void AccountButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(AccountPage));

    private async void PlotButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        int plotId = int.Parse(button.AutomationId.Replace("Plot", ""));

        var textLabel = FindByName($"TextPlot{plotId}") as Label;
        var cropImg = FindByName($"CropPlot{plotId}") as Image;
        var overlayImg = FindByName($"OverlayPlot{plotId}") as Image;

        var result = await _gameService.CheckPlotAsync(plotId);

        if (result.IsEmpty)
        {
            _gameService.SelectedPlotId = plotId;
            await Shell.Current.GoToAsync(nameof(InventoryPage));
        }
        else if (!result.CanHarvest && result.Message.Contains("thirsty"))
        {
            if (textLabel != null) textLabel.Text = ""; // Clear text temporarily during watering sequence

            // Sprout stays under the animation
            if (cropImg != null) cropImg.Source = "plant_sprout.png";

            // Frame 1: Thirsty prompt
            if (overlayImg != null) overlayImg.Source = "water_drop.png";
            await Task.Delay(200);

            // Frame 2: Pouring/watering action
            if (overlayImg != null) overlayImg.Source = "water_ani.png";
            await Task.Delay(200);

            // Frame 3: Final state -> Sprout underneath + Watered overlay layer on top
            if (overlayImg != null) overlayImg.Source = "watered.png";
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