namespace sick_ahh_farming_game;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));
        Routing.RegisterRoute(nameof(ShopPage), typeof(ShopPage));
        Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
    }
}