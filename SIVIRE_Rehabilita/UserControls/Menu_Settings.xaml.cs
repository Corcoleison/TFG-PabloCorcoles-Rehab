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

        private Style checkedBoxStyle;
        private Style unCheckedBoxStyle;

        public Menu_Settings()
        {
            this.App = ((App)Application.Current);
            checkedBoxStyle = this.FindResource("CheckBoxStyleChecked") as Style;
            unCheckedBoxStyle = this.FindResource("CheckBoxStyle") as Style;
            InitializeComponent();
            
        }

        private void gestures_Click(object sender, RoutedEventArgs e)
        {
            //SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            //click_sound.Play();
            if ((bool)this.cbox_gestures.IsChecked)
            {
                App.Gestures_IsEnabled = this.cbox_gestures.IsChecked.Value;
                this.cbox_gestures.Style= checkedBoxStyle;
            }
            else
            {
                this.cbox_gestures.Style = unCheckedBoxStyle;
            }
                
        }

        private void sound_Click(object sender, RoutedEventArgs e)
        {
            //SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            //click_sound.Play();
            if ((bool)this.cbox_sound.IsChecked)
            {
                App.Sound_IsEnabled = this.cbox_sound.IsChecked.Value;
                this.cbox_sound.Style = checkedBoxStyle;
            }
            else
            {
                this.cbox_sound.Style = unCheckedBoxStyle;
            }
        }

        private void readMsg_Click(object sender, RoutedEventArgs e)
        {
            //SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            //click_sound.Play();
            if ((bool)this.cbox_readMsg.IsChecked)
            {
                App.ReadMsg_IsEnabled = this.cbox_readMsg.IsChecked.Value;
                this.cbox_readMsg.Style = checkedBoxStyle;
            }
            else
            {
                this.cbox_readMsg.Style = unCheckedBoxStyle;
            }
        }

        private void readErrorMsg_Click(object sender, RoutedEventArgs e)
        {
            //SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            //click_sound.Play();
            if ((bool)this.cbox_readErrorMsg.IsChecked)
            {
                App.ReadErrorMsg_IsEnabled = this.cbox_readErrorMsg.IsChecked.Value;
                this.cbox_readErrorMsg.Style = checkedBoxStyle;
            }
            else
            {
                this.cbox_readErrorMsg.Style = unCheckedBoxStyle;
            }
        }
    }
}
