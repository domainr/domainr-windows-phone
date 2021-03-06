﻿using System;
using System.Windows;
using System.Windows.Navigation;
using Coding4Fun.Phone.Controls;
using Domainr7.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;
using Domainr7.Model;
using System.Windows.Resources;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace Domainr7
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void ShowMessage(string Title, string Message)
        {
            ShowMessage(Title, Message, null);
        }

        public static void ShowMessage(string Title, string Message, Action action)
        {
            ToastPrompt _prompt = new ToastPrompt
            {
                Title = Title,
                Message = Message
            };

            if (action != null)
                _prompt.Tap += (s, e) => { action(); };
            _prompt.Show();
        }

        public static bool IsNetworkAvailable
        {
            get
            {
                var result = NetworkInterface.GetIsNetworkAvailable();
                if (!result)
                {
                    ShowMessage("Warning", "No network connections available");
                }
                return result;
            }
        }

        private static string FlurryApiKey { get; set; }

        // Easy access to the root frame
        public PhoneApplicationFrame RootFrame
        {
            get;
            private set;
        }

        // Constructor
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            SetFlurryApiKey();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disable user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        private void SetFlurryApiKey()
        {
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("FlurryConfig.xml", UriKind.Relative));
            if (sri == null)
            {
                FlurryApiKey = "NoKeyFound";
            }
            else
            {
                try
                {
                    XDocument doc = XDocument.Load(sri.Stream);
                    string key = doc.Descendants("ApiKey").FirstOrDefault().Value;
                    if (string.IsNullOrEmpty(key))
                    {
                        FlurryApiKey = "NoKeyFound";
                    }
                    else
                    {
                        FlurryApiKey = key;
                    }
                }
                catch
                {
                    FlurryApiKey = "NoKeyFound";
                }
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            FlurryWP7SDK.Api.StartSession(FlurryApiKey);
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            FlurryWP7SDK.Api.StartSession(FlurryApiKey);
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            ViewModelLocator.Cleanup();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
            FlurryWP7SDK.Api.LogError("Navigation", e.Exception);
            e.Handled = true;
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
            FlurryWP7SDK.Api.LogError("MainAppError", e.ExceptionObject);
            e.Handled = true;
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}
