using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SIVIRE_Rehabilita.UserControls
{
	/// <summary>
	/// Lógica de interacción para ErrorMsgPanel.xaml
	/// </summary>
	public partial class ErrorMsgPanel : UserControl
	{
		public ErrorMsgPanel()
		{
			this.InitializeComponent();
		}

        public string ErrorMsg
        {
            get { return (string)GetValue(ErrorMsgProperty); }
            set { SetValue(ErrorMsgProperty, value); }
        }

        public static readonly DependencyProperty ErrorMsgProperty =
            DependencyProperty.Register("ErrorMsg", typeof(string), typeof(ErrorMsgPanel), new PropertyMetadata(string.Empty, new PropertyChangedCallback(ErrorMsgChanged)));

        private static void ErrorMsgChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ErrorMsgPanel errorMsgPanel = sender as ErrorMsgPanel;
            errorMsgPanel.updateErrorMsg();
        }

        private void updateErrorMsg()
        {
            Storyboard sb;

            if (this.ErrorMsg.Equals(string.Empty))
                sb = (Storyboard)this.FindResource("hideError");
            else
                sb = (Storyboard)this.FindResource("showError");

            sb.Begin();
        }
	}
}