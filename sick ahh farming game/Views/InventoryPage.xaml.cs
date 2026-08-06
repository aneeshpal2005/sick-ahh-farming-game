using sick_ahh_farming_game.Models;
using sick_ahh_farming_game.Services;
using System.Collections.ObjectModel;

namespace sick_ahh_farming_game;

public partial class InventoryPage : ContentPage
{
    private readonly GameService _gameService = GameManager.GameService;
    public ObservableCollection<InventoryDisplayItem> DisplayInventory { get; set; } = new();

    public InventoryPage()
    {
        InitializeComponent();
        InventoryCollectionView.ItemsSource = DisplayInventory;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadInventoryAsync();
    }

    private async Task LoadInventoryAsync()
    {
        var rawInventory = await _gameService.GetInventoryAsync();
        DisplayInventory.Clear();

        foreach (var item in rawInventory)
        {
            if (item.Quantity > 0)
            {
                DisplayInventory.Add(new InventoryDisplayItem
                {
                    SeedId = item.SeedId,
                    Seed = item.Seed,
                    Quantity = item.Quantity,
                    // Cap display bubble at 99 max
                    DisplayQuantity = item.Quantity > 99 ? "99+" : item.Quantity.ToString()
                });
            }
        }
    }

    private async void InventoryCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not InventoryDisplayItem selectedItem)
            return;

        // Deselect item immediately so it can be clicked again
        InventoryCollectionView.SelectedItem = null;

        bool planted = await _gameService.PlantSeedAsync(_gameService.SelectedPlotId, selectedItem.SeedId);

        if (planted)
        {
            await DisplayAlert("Success", $"{selectedItem.Seed?.Name} planted! 🌱", "OK");
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await DisplayAlert("Farm", "No plot selected or unable to plant here.", "OK");
        }
    }

    private async void FarmButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("//MainPage");
    private async void InventoryButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(InventoryPage));
    private async void ShopButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(ShopPage));
    private async void AccountButton_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(AccountPage));
}

// Helper wrapper class for UI display formatting 🤢
public class InventoryDisplayItem
{
    public int SeedId { get; set; }
    public Seed? Seed { get; set; }
    public int Quantity { get; set; }
    public string DisplayQuantity { get; set; } = "0";
}