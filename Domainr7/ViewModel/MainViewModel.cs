using System;
using System.Collections.ObjectModel;
using System.Net;
using Domainr7.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using SharpGIS;

namespace Domainr7.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            SearchResults = new ObservableCollection<Result>();
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                SearchResults.Add(new Result()
                {
                    availability = "available",
                    subdomain = "ferr.et",
                    domain = "ferr.et",
                    path = "/land"
                });
                     
            }
            else
            {
                WireMessages();
                WireCommands();
            }
        }

        private void WireCommands()
        {
            ViewDomainCommand = new RelayCommand<Result>(result =>
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(result, Constants.SendDomainrDetailsCommand));
            });

            ClearSearchCommand = new RelayCommand(() =>
            {
                SearchTerm = "";
                SearchResults.Clear();
            });

            DoSearchCommand = new RelayCommand(DoSearch);
        }

        private void WireMessages()
        {
            Messenger.Default.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification.Equals(Constants.DoSearchCommand))
                {
                    DoSearch();
                }
            });
        }

        private void DoSearch()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                if (App.IsNetworkAvailable)
                {
                    WebClient client = new GZipWebClient();
                    IsVisible = true;
                    ProgressText = "Searching...";
                    client.DownloadStringCompleted += (s, e) =>
                    {
                        if (e.Error == null)
                        {
                            if (e.Result != null)
                            {
                                SearchResults.Clear();
                                var returnedSearches = JsonConvert.DeserializeObject<DomainrClass>(e.Result).results;
                                returnedSearches.ForEach(search => SearchResults.Add(search));
                            }
                        }
                        else
                        {
                            App.ShowMessage("Error", e.Error.InnerException.Message);
                        }
                        IsVisible = false;
                    };
                    string url = string.Format(Constants.QueryUrl, SearchTerm);
                    client.DownloadStringAsync(new Uri(url, UriKind.Absolute));
                }
            }
        }

        public string SearchTerm { get; set; }
        public ObservableCollection<Result> SearchResults { get; set; }

        public RelayCommand DoSearchCommand { get; set; }
        public RelayCommand ClearSearchCommand { get; set; }
        public RelayCommand<Result> ViewDomainCommand { get; set; }

        public bool IsVisible { get; set; }
        public string ProgressText { get; set; }
    }
}