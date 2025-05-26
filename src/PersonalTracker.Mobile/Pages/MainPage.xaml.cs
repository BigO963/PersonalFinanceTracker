using PersonalTracker.Mobile.Models;
using PersonalTracker.Mobile.PageModels;

namespace PersonalTracker.Mobile.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}