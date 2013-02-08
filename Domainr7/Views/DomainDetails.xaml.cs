using Coding4Fun.Toolkit.Controls;
using Domainr7.Model;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;

namespace Domainr7.Views
{
    /// <summary>
    /// Description for DomainDetails.
    /// </summary>
    public partial class DomainDetails : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the DomainDetails class.
        /// </summary>
        public DomainDetails()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            new AppBarPrompt(new AppBarPromptAction("share by email", () => Messenger.Default.Send(new NotificationMessage("email", Constants.ShareActionCommand))), 
                new AppBarPromptAction("share to social networks", () => Messenger.Default.Send(new NotificationMessage("social", Constants.ShareActionCommand))), 
                new AppBarPromptAction("copy url to clipboard", () => Messenger.Default.Send(new NotificationMessage("clipboard", Constants.ShareActionCommand)))).Show();
        }
    }
}