using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public partial class MainPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;

    public MainPage()
    {
        InitializeComponent();
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
        else if (result.CanHarvest)
        {
            var harvestResult = await _gameService.HarvestPlotAsync(plotId);

            await DisplayAlert(
                harvestResult.Success ? "Harvest" : "Farm",
                harvestResult.Message,
                "OK");
        }
        else
        {
            await DisplayAlert("Farm", result.Message, "OK");
        }
    }
}