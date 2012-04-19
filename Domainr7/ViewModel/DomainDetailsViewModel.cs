using GalaSoft.MvvmLight;
using System;
using GalaSoft.MvvmLight.Messaging;
using Domainr7.Model;
using GalaSoft.MvvmLight.Command;
using SharpGIS;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Phone.Tasks;
using System.Collections.Generic;

namespace Domainr7.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
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
    public class DomainDetailsViewModel : ViewModelBase
    {
        private bool isLoaded = false;
        /// <summary>
        /// Initializes a new instance of the DomainDetailsViewModel class.
        /// </summary>
        public DomainDetailsViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                SelectedDomain = new Result()
                {
                    availability = "available",
                    subdomain = "ferr.et",
                    domain = "ferr.et",
                    path = "/land",
                    register_url= "http://register.com"
                };

                SelectedDomainInfo = new DomainrInfo
                {
                    domain = "ferr.et",
                    whois_url = "http://domai.nr/ferr.et/whois",
                    tld = new Tld
                    {
                        domain = "et",
                        wikipedia_url = "http://wikipedia.org/wiki/.et",
                        iana_url = "http://www.iana.org/domains/root/db/et.html"
                    },
                    availability = Constants.AvailabilityAvailable
                };
                List<Registrar> registrars = new List<Registrar>();
                registrars.Add(new Registrar
                {
                    registrar = "101domain.com",
                    name = "101domain.com",
                    register_url = "http://domai.nr/liveside.net/register/101domain.com"
                });
                registrars.Add(new Registrar
                {
                    registrar = "dotster.com",
                    name = "Dotster",
                    register_url = "http://domai.nr/liveside.net/register/dotster.com"
                });
                SelectedDomainInfo.registrars = registrars;
            }
            else
            {
                // Code runs "for real": Connect to service, etc...
                WireMessages();
                WireCommands();
            }
        }

        private void WireCommands()
        {
            PageLoadedCommand = new RelayCommand(() =>
            {
                if (!isLoaded)
                {
                    if (App.IsNetworkAvailable)
                    {
                        IsVisible = true;
                        ProgressText = "Filling in the gaps...";
                        WebClient client = new GZipWebClient();
                        client.DownloadStringCompleted += (s, e) =>
                        {
                            if (e.Error == null)
                            {
                                if (e.Result != null)
                                {
                                    SelectedDomainInfo = JsonConvert.DeserializeObject<DomainrInfo>(e.Result);
                                    isLoaded = true;
                                }
                            }
                            else
                            {
                                App.ShowMessage("Error", e.Error.InnerException.Message);
                            }
                            IsVisible = false;
                        };
                        string url = string.Format(Constants.InfoUrl, SelectedDomain.domain);
                        client.DownloadStringAsync(new Uri(url, UriKind.Absolute));
                    }
                }
            });

            NavigateToExternal = new RelayCommand<string>(link =>
            {
                new WebBrowserTask
                {
                    Uri = new Uri(link, UriKind.Absolute)
                }.Show();
            });
        }

        private void WireMessages()
        {
            Messenger.Default.Register<NotificationMessage>(this, m =>
            {
                if (m.Notification.Equals(Constants.SendDomainrDetailsCommand))
                {
                    SelectedDomain = m.Sender as Result;
                    isLoaded = false;
                }
                if (m.Notification.Equals(Constants.ShareActionCommand))
                {
                    string command = (string)m.Sender;
                    switch (command)
                    {
                        case "email":
                            new EmailComposeTask
                            {
                                Subject = "Domainr: " + SelectedDomain.domain,
                                Body = string.Format("Found on Domainr: \n\nhttp://domai.nr/{0}", SelectedDomain.domain)
                            }.Show();                                 
                            break;
                        case "social":
                            new ShareStatusTask
                            {
                                Status = string.Format("Found a great domain ({0}) using http://domai.nr", SelectedDomain.domain)
                            }.Show();
                            break;
                        case "clipboard":
                            System.Windows.Clipboard.SetText(string.Format("http://{0}", SelectedDomain.domain));
                            break;
                    }
                }
            });
        }

        public Result SelectedDomain { get; set; }
        public DomainrInfo SelectedDomainInfo { get; set; }

        public RelayCommand PageLoadedCommand { get; set; }
        public RelayCommand<string> NavigateToExternal { get; set; }

        public bool IsVisible { get; set; }
        public string ProgressText { get; set; }

    }
}