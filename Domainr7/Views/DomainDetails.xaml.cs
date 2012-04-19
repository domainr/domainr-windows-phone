using Domainr7.Model;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using ScottIsAFool.WindowsPhone.Tools;

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
            new PhoneFlipMenu(new PhoneFlipMenuAction("share by email", () =>
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("email", Constants.ShareActionCommand));
            }), new PhoneFlipMenuAction("share to social networks", () =>
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("social", Constants.ShareActionCommand));
            }), new PhoneFlipMenuAction("copy url to clipboard", () =>
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("clipboard", Constants.ShareActionCommand));
            })).Show();
        }
    }
}