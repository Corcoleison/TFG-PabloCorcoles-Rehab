using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Lógica de interacción para Dialog_Confirm.xaml
    /// </summary>
    public partial class Dialog_Confirm : UserControl
    {
        public Dialog_Confirm()
        {
            InitializeComponent();
        }

        public event EventHandler OkSelected;
        public event EventHandler CancelSelected;

        public string Msg
        {
            get { return (string)GetValue(MsgProperty); }
            set { SetValue(MsgProperty, value); }
        }

        public Window WindowToShow { get; set; }

        public static readonly DependencyProperty MsgProperty =
            DependencyProperty.Register("Msg", typeof(string), typeof(Dialog_Confirm), new PropertyMetadata(String.Empty, new PropertyChangedCallback(OnPropertyChanged)));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OkSelected(this, new EventArgs());
            }
            catch (System.NullReferenceException) { }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.CancelSelected(this, new EventArgs());
            }
            catch (System.NullReferenceException) { }
        }
    }
}
