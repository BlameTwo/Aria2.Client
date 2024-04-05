using Aria2.Client.Services.Contracts;
using Aria2.Client.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Services
{
    public sealed class TipShow : ITipShow
    {
        private Panel _owner;

        public Panel Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public void ShowMessage(string message, Symbol icon)
        {
            PopupDialog popup = new(message, Owner, icon);
            popup.ShowPopup();
        }
    }
}
