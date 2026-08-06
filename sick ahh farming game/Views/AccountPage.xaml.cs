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
        await LoadAccountStatsAsync();
    }

    private async Task LoadAccountStatsAsync()
    {
        var player = await _gameService.GetPlayerAsync();
        var inventory = await _gameService.GetInventoryAsync();

        int totalSeeds = inventory.Sum(i => i.Quantity);

        UsernameEntry.Text = player.Username;
        HarvestedLabel.Text = $"Plants Harvested: {player.PlantsHarvested}";
        SeedsLabel.Text = $"Seeds Owned: {totalSeeds}";
        MoneyMadeLabel.Text = $"Total Money Made: {player.TotalMoneyMade} G";
        CoinsLabel.Text = $"Current Coins: {player.Coins} G";
    }

    private async void SaveUsernameButton_Clicked(object sender, EventArgs e)
    {
        string newName = UsernameEntry.Text?.Trim();
        await _gameService.UpdateUsernameAsync(newName);
        await DisplayAlert("Success", "Thank you! Saved! ✏️", "OK");
    }

    private async void SaveGameButton_Clicked(object sender, EventArgs e)
    {
        await _gameService.SaveGameAsync();
        await DisplayAlert("Success", "Game saved successfully! 💾", "OK");
    }

    private async void DeleteAccountButton_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Danger Zone!",
            "All of it will be gone. It shall be deleted. This cannot be undone, be prepared warrior.",
            "Yes, Wipe It",
            "Cancel");

        if (confirm)
        {
            await _gameService.ResetGameAsync();
            await DisplayAlert("Reset", "Your account has been deleted and the game has been reset to scratch.", "OK");
            await LoadAccountStatsAsync();
            await Shell.Current.GoToAsync("//MainPage");
        }
    }

    private async void FarmButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//MainPage");
    private async void InventoryButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(InventoryPage));
    private async void ShopButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(ShopPage));
    private async void AccountButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(AccountPage));
}