using sick_ahh_farming_game.Services;
using System.Timers;
using Timer = System.Timers.Timer;

namespace sick_ahh_farming_game;

public partial class MainPage : ContentPage
{
    // Call the game service
    private readonly GameService _gameService = GameManager.GameService;
    
    //Duplicate and change name and time for each plant (milliseconds)
    Timer plantName = new Timer(30000);
    public MainPage()
    {
        InitializeComponent();
    }

    //Called when plot gets planted
    //Name subject to change
    //Pass a variable
    public void plotTimer(int plotId)
    {
        //if-else statement, checks for which timer to start based on plotId
        //Triggers function when time ends
        plantName.Elapsed += fullyGrownPlant;
        plantName.AutoReset = false;
        plantName.Start();
    }

    private void fullyGrownPlant(object? sender, ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }

    //When plant grows, it changes image to grown
    //Possibly receives a passed value for the plot
    public void fullyGrownPlant()
    {

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