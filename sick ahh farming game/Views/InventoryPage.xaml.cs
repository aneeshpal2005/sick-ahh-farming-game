

using sick_ahh_farming_game.Models;
using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public partial class InventoryPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;

    public InventoryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadInventory();
    }

    private async Task LoadInventory()
    {
        var inventory = await _gameService.GetInventoryAsync();

        ClearSlots();

        if (inventory.Count > 0)
        {
            Slot1Emoji.Text = inventory[0].Seed?.Emoji;
            Slot1Name.Text = inventory[0].Seed?.Name;
            Slot1Quantity.Text = $"x{inventory[0].Quantity}";
        }
    }

    private void ClearSlots()
    {
        Slot1Emoji.Text = "";
        Slot1Name.Text = "";
        Slot1Quantity.Text = "";

        Slot2Emoji.Text = "";
        Slot2Name.Text = "";
        Slot2Quantity.Text = "";

        Slot3Emoji.Text = "";
        Slot3Name.Text = "";
        Slot3Quantity.Text = "";

        Slot4Emoji.Text = "";
        Slot4Name.Text = "";
        Slot4Quantity.Text = "";

        Slot5Emoji.Text = "";
        Slot5Name.Text = "";
        Slot5Quantity.Text = "";

        Slot6Emoji.Text = "";
        Slot6Name.Text = "";
        Slot6Quantity.Text = "";

        Slot7Emoji.Text = "";
        Slot7Name.Text = "";
        Slot7Quantity.Text = "";

        Slot8Emoji.Text = "";
        Slot8Name.Text = "";
        Slot8Quantity.Text = "";

        Slot9Emoji.Text = "";
        Slot9Name.Text = "";
        Slot9Quantity.Text = "";
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