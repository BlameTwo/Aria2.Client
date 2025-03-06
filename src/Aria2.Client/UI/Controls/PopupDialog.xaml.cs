using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Threading.Tasks;
namespace Aria2.Client.UI.Controls
{
    public sealed partial class PopupDialog : UserControl
    {
        private string _popupContent;
        private readonly Panel uIElement;

        private Popup _popup = null;

        public PopupDialog()
        {
            InitializeComponent();
            _popup = new Popup();
            _popup.Child = this;
            Loaded += PopupNoticeLoaded;
        }

        public PopupDialog(string popupContentString, Panel uIElement, Symbol symbol)
            : this()
        {
            PopupIcon.Symbol = symbol;
            _popupContent = popupContentString;
            this.uIElement = uIElement;
            _popup.XamlRoot = uIElement.XamlRoot;
        }

        public void ShowPopup()
        {
            _popup.IsOpen = true;
        }

        private void PopupNoticeLoaded(object sender, RoutedEventArgs e)
        {
            PopupContent.Text = _popupContent;
            PopupIn.Begin();
            PopupIn.Completed += PopupInCompleted;
            Width = uIElement.ActualWidth;
            Height = uIElement.ActualHeight;
        }

        public async void PopupInCompleted(object sender, object e)
        {
            await Task.Delay(1000);

            PopupOut.Begin();
            PopupOut.Completed += PopupOutCompleted;
        }

        public void PopupOutCompleted(object sender, object e)
        {
            _popup.IsOpen = false;
        }
    }
}
