
using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public partial class AccountPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;

    public AccountPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var player = await _gameService.GetPlayerAsync();
        var inventory = await _gameService.GetInventoryAsync();

        CoinsLabel.Text = $"Coins: {player.Coins}";
        HarvestedLabel.Text = $"Plants Harvested: {player.PlantsHarvested}";
        SeedsLabel.Text = $"Seeds Owned: {inventory.Sum(i => i.Quantity)}";
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
}