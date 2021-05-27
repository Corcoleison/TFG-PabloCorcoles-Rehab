using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIVIRE_Rehabilita.UserControls
{
    /// <summary>
    /// Lógica de interacción para Menu_Settings.xaml
    /// </summary>
    public partial class Menu_Settings : UserControl
    {
        public App App { set; get; }

        public Menu_Settings()
        {
            this.App = ((App)Application.Current);
            InitializeComponent();
        }

        private void gestures_Click(object sender, RoutedEventArgs e)
        {
            new SoundPlayer(@"Sounds\click.wav").Play();
            if (this.cbox_gestures.IsChecked.HasValue)
                App.Gestures_IsEnabled = this.cbox_gestures.IsChecked.Value;
        }

        private void sound_Check(object sender, RoutedEventArgs e)
        {
            new SoundPlayer(@"Sounds\click.wav").Play();
            if (this.cbox_sound.IsChecked.HasValue)
                App.Sound_IsEnabled = this.cbox_sound.IsChecked.Value;
        }

        private void readMsg_Click(object sender, RoutedEventArgs e)
        {
            new SoundPlayer(@"Sounds\click.wav").Play();
            if (this.cbox_readMsg.IsChecked.HasValue)
                App.ReadMsg_IsEnabled = this.cbox_readMsg.IsChecked.Value;
        }
    }
}
