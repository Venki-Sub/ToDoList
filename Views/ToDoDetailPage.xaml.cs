namespace ToDoList.Views
{
    public partial class ToDoDetailPage : ContentPage
    {
        private readonly ToDoDetailViewModel viewModel;
        private IDispatcherTimer? timer;

        public ToDoDetailPage(ToDoDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Reload when coming back from the Edit page.
            await viewModel.LoadAsync();

            // Live "Starting in 00 : 05 : 05" countdown.
            timer ??= Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick -= OnTick;
            timer.Tick += OnTick;
            timer.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            timer?.Stop();
        }

        private void OnTick(object? sender, EventArgs e) => viewModel.Tick();
    }
}
