namespace MiBusCR
{
    public partial class App : Application
    {
        public static Supabase.Client SupabaseClient { get; private set; }
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}