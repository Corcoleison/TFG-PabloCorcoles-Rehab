using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SIVIRE_Rehabilita.UserControls
{
    /// <summary>
    /// Lógica de interacción para MessagePanel.xaml
    /// </summary>
    public partial class GuideMsgPanel : UserControl
    {
        public GuideMsgPanel()
        {
            InitializeComponent();
        }

        public string GuideMsg
        {
            get { return (string)GetValue(GuideMsgProperty); }
            set { SetValue(GuideMsgProperty, value); }
        }

        public static readonly DependencyProperty GuideMsgProperty =
            DependencyProperty.Register("GuideMsg", typeof(string), typeof(GuideMsgPanel), new PropertyMetadata(string.Empty, new PropertyChangedCallback(GuideMsgChanged)));

        private static void GuideMsgChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            GuideMsgPanel guideMsgPanel = sender as GuideMsgPanel;
            guideMsgPanel.updateGuideMsg();
        }

        private void updateGuideMsg()
        {
            Storyboard sb;

            if (this.GuideMsg.Equals(string.Empty))
                sb = (Storyboard)this.FindResource("hideMsg");
            else
                sb = (Storyboard)this.FindResource("showMsg");

            sb.Begin();
        }
    }
}
