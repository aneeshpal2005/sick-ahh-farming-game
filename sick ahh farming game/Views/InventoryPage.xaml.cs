

using sick_ahh_farming_game.Models;
using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game;

public partial class InventoryPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;
    private List<InventoryItem> _inventory = new();

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
        _inventory = await _gameService.GetInventoryAsync();

        ClearSlots();

        if (_inventory.Count > 0)
        {
            Slot1Emoji.Text = _inventory[0].Seed?.Emoji;
            Slot1Name.Text = _inventory[0].Seed?.Name;
            Slot1Quantity.Text = $"x{_inventory[0].Quantity}";
        }

        if (_inventory.Count > 1)
        {
            Slot2Emoji.Text = _inventory[1].Seed?.Emoji;
            Slot2Name.Text = _inventory[1].Seed?.Name;
            Slot2Quantity.Text = $"x{_inventory[1].Quantity}";
        }

        if (_inventory.Count > 2)
        {
            Slot3Emoji.Text = _inventory[2].Seed?.Emoji;
            Slot3Name.Text = _inventory[2].Seed?.Name;
            Slot3Quantity.Text = $"x{_inventory[2].Quantity}";
        }

        if (_inventory.Count > 3)
        {
            Slot4Emoji.Text = _inventory[3].Seed?.Emoji;
            Slot4Name.Text = _inventory[3].Seed?.Name;
            Slot4Quantity.Text = $"x{_inventory[3].Quantity}";
        }

        if (_inventory.Count > 4)
        {
            Slot5Emoji.Text = _inventory[4].Seed?.Emoji;
            Slot5Name.Text = _inventory[4].Seed?.Name;
            Slot5Quantity.Text = $"x{_inventory[4].Quantity}";
        }

        if (_inventory.Count > 5)
        {
            Slot6Emoji.Text = _inventory[5].Seed?.Emoji;
            Slot6Name.Text = _inventory[5].Seed?.Name;
            Slot6Quantity.Text = $"x{_inventory[5].Quantity}";
        }

        if (_inventory.Count > 6)
        {
            Slot7Emoji.Text = _inventory[6].Seed?.Emoji;
            Slot7Name.Text = _inventory[6].Seed?.Name;
            Slot7Quantity.Text = $"x{_inventory[6].Quantity}";
        }

        if (_inventory.Count > 7)
        {
            Slot8Emoji.Text = _inventory[7].Seed?.Emoji;
            Slot8Name.Text = _inventory[7].Seed?.Name;
            Slot8Quantity.Text = $"x{_inventory[7].Quantity}";
        }

        if (_inventory.Count > 8)
        {
            Slot9Emoji.Text = _inventory[8].Seed?.Emoji;
            Slot9Name.Text = _inventory[8].Seed?.Name;
            Slot9Quantity.Text = $"x{_inventory[8].Quantity}";
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

    private async void Seed_Clicked(object sender, EventArgs e)
    {
        if (_inventory.Count == 0)
        {
            await DisplayAlert("Error", "You don't have any seeds.", "OK");
            return;
        }

        var selectedSeed = _inventory[0];

        bool planted = await _gameService.PlantSeedAsync(
            _gameService.SelectedPlotId,
            selectedSeed.SeedId);

        if (planted)
        {
            await DisplayAlert("Success",
                $"{selectedSeed.Seed?.Name} planted!",
                "OK");

            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await DisplayAlert("Error",
                "Unable to plant seed.",
                "OK");
        }
    }
}