using System;

namespace IoT
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.ViewManagement;

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
        }
    }
}
