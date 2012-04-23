using System;
using System.Reactive.Linq;
using System.Windows.Controls;
using Domainr7.Model;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using ReactiveUI;

namespace Domainr7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += (s, e) => { if (string.IsNullOrEmpty(txbxSearch.Text)) txbxSearch.Focus(); };

            // Using Rx, we just want the last TextChangedEventHandler where there is a pause of 1 second,
            // then send it to the viewmodel.
            Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>
                (h => txbxSearch.TextChanged += h, h => txbxSearch.TextChanged -= h)
                .Throttle(TimeSpan.FromSeconds(1), RxApp.DeferredScheduler)
                .Subscribe(_ =>
                {
                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage(Constants.DoSearchCommand));
                });
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }
    }
}
