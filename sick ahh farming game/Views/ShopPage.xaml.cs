using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public partial class ShopPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;

    public ShopPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await UpdateGoldDisplayAsync();
    }

    private async Task UpdateGoldDisplayAsync()
    {
        var player = await _gameService.GetPlayerAsync();
        CoinsLabel.Text = $"💰 {player.Coins} G";
    }

    private async void FarmButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//MainPage");
    private async void InventoryButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(InventoryPage));
    private async void ShopButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(ShopPage));
    private async void AccountButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(AccountPage));

    private async Task BuyAndRefreshAsync(int seedId, int qty)
    {
        var result = await _gameService.BuySeedAsync(seedId, qty);
        if (result.Success)
        {
            await UpdateGoldDisplayAsync(); // Updates gold instantly on screen!
        }
        await DisplayAlert(result.Success ? "Success" : "Error", result.Message, "OK");
    }

    // Carrot (ID 1)
    private async void BuyCarrot1_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(1, 1);
    private async void BuyCarrot5_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(1, 5);

    // Corn (ID 2)
    private async void BuyCorn1_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(2, 1);
    private async void BuyCorn5_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(2, 5);

    // Tomato (ID 3)
    private async void BuyTomato1_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(3, 1);
    private async void BuyTomato5_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(3, 5);

    // Potato (ID 4)
    private async void BuyPotato1_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(4, 1);
    private async void BuyPotato5_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(4, 5);

    // Eggplant (ID 5)
    private async void BuyEggplant1_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(5, 1);
    private async void BuyEggplant5_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(5, 5);

    // Pepper (ID 6)
    private async void BuyPepper1_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(6, 1);
    private async void BuyPepper5_Clicked(object sender, EventArgs e) => await BuyAndRefreshAsync(6, 5);
}