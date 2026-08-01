
using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public partial class ShopPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;

    public ShopPage()
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

    private async void CarrotSeed_Tapped(object sender, TappedEventArgs e)
    {
        var result = await _gameService.BuySeedAsync(1);

        await DisplayAlert(
            result.Success ? "Success" : "Error",
            result.Message,
            "OK");
    }
}