using DataMockerSample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DataMockerSample.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModernLoginPage : ContentPage
    {
        private ModernLoginViewModel _viewModel;
        
        public ModernLoginPage (ModernLoginViewModel viewModel)
        {
            InitializeComponent();
            viewModel.CurrentPage = this;
            BindingContext = viewModel;
            _viewModel = viewModel;
        }
        
        private void Button_OnClicked(object? sender, EventArgs e)
        {
            var result = _viewModel.CheckServerAsync().Result;
            DisplayAlert("Check result", result ? "Server is available" : "Server is unavailable", "OK");

            if (!result)
            {
                File.WriteAllText("C:\\temp\\last_fail.txt", DateTime.Now.ToString());
            }
        }
    }
}