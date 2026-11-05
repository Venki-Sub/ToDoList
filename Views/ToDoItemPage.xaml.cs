namespace ToDoList.Views
{
    public partial class ToDoItemPage : ContentPage
    {
        private readonly ToDoItemViewModel viewModel;

        public ToDoItemPage(ToDoItemViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Reload so new / edited / deleted tasks show up immediately.
            viewModel.LoadToDoItemsCommand.Execute(null);
        }
    }
}
