using SQLitePCL;

namespace sick_ahh_farming_game
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Batteries_V2.Init();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}