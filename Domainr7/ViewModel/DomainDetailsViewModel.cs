using System;
using System.Collections.Generic;
using System.Net;
using Domainr7.Model;
using DomainrSharp.WindowsPhone;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using SharpGIS;

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
        private DomainrSharpService domainr;
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
                    Availability = "available",
                    Subdomain = "ferr.et",
                    Domain = "ferr.et",
                    Path = "/land",
                    RegisterUrl= "http://register.com"
                };

                SelectedDomainInfo = new DomainrInfo
                {
                    Domain = "ferr.et",
                    WhoisUrl = "http://domai.nr/ferr.et/whois",
                    Tld = new Tld
                    {
                        Domain = "et",
                        WikipediaUrl = "http://wikipedia.org/wiki/.et",
                        IanaUrl = "http://www.iana.org/domains/root/db/et.html"
                    },
                    Availability = Constants.AvailabilityAvailable
                };
                List<Registrar> registrars = new List<Registrar>();
                registrars.Add(new Registrar
                {
                    RegistrarDomain = "101domain.com",
                    Name = "101domain.com",
                    RegisterUrl = "http://domai.nr/liveside.net/register/101domain.com"
                });
                registrars.Add(new Registrar
                {
                    RegistrarDomain = "dotster.com",
                    Name = "Dotster",
                    RegisterUrl = "http://domai.nr/liveside.net/register/dotster.com"
                });
                SelectedDomainInfo.Registrars = registrars;
            }
            else
            {
                // Code runs "for real": Connect to service, etc...
                domainr = new DomainrSharpService();

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
                        domainr.InfoDownloadCompleted += (s, e) =>
                        {
                            if (e.Error == null)
                            {
                                if (e.Result != null)
                                {
                                    SelectedDomainInfo = e.Result;
                                    isLoaded = true;
                                }
                            }
                            else
                            {
                                App.ShowMessage("Error", e.Error.Message);
                            }
                            IsVisible = false;
                        };
                        domainr.InfoDownloadAsync(SelectedDomain.Domain);
                    }
                }
            });

            NavigateToRegistrar = new RelayCommand<Registrar>(reg =>
            {
                // For analytical purposes, selected domain and chose registrar are logged
                // This gives an indication on how useful users find domainr
                List<FlurryWP7SDK.Models.Parameter> args = new List<FlurryWP7SDK.Models.Parameter>();
                args.Add(new FlurryWP7SDK.Models.Parameter("Domain", SelectedDomain.Domain));
                args.Add(new FlurryWP7SDK.Models.Parameter("Registrar", reg.Name));
                args.Add(new FlurryWP7SDK.Models.Parameter("RegistrarDomain", reg.RegistrarDomain));

                FlurryWP7SDK.Api.LogEvent("RegistrarTapped", args);
                NavigateTo(reg.RegisterUrl);
            });

            NavigateToExternal = new RelayCommand<string>(NavigateTo);
            
        }

        private void NavigateTo(string link)
        {
            new WebBrowserTask
            {
                Uri = new Uri(link, UriKind.Absolute)
            }.Show();
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
                                Subject = "Domainr: " + SelectedDomain.Domain,
                                Body = string.Format("Found on Domainr: \n\nhttp://domai.nr/{0}", SelectedDomain.Domain)
                            }.Show();                                 
                            break;
                        case "social":
                            new ShareStatusTask
                            {
                                Status = string.Format("Found a great domain ({0}) using http://domai.nr", SelectedDomain.Domain)
                            }.Show();
                            break;
                        case "clipboard":
                            System.Windows.Clipboard.SetText(string.Format("http://{0}", SelectedDomain.Domain));
                            break;
                    }
                }
            });
        }

        public Result SelectedDomain { get; set; }
        public DomainrInfo SelectedDomainInfo { get; set; }

        public RelayCommand PageLoadedCommand { get; set; }
        public RelayCommand<string> NavigateToExternal { get; set; }
        public RelayCommand<Registrar> NavigateToRegistrar { get; set; }

        public bool IsVisible { get; set; }
        public string ProgressText { get; set; }

    }
}