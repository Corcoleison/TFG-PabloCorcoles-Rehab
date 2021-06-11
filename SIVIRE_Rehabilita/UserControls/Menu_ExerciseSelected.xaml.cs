using SIVIRE_Rehabilita.Model;
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
    /// Lógica de interacción para Menu_ExerciseSelected.xaml
    /// </summary>
    public partial class Menu_ExerciseSelected : UserControl
    {
        public Menu_ExerciseSelected()
        {
            InitializeComponent();
        }

        public event EventHandler StartExercise;

        public Exercise Exercise
        {
            get { return (Exercise)GetValue(ExerciseProperty); }
            set { SetValue(ExerciseProperty, value); }
        }

        public static readonly DependencyProperty ExerciseProperty =
            DependencyProperty.Register("Exercise", typeof(Exercise), typeof(Menu_ExerciseSelected), new PropertyMetadata(new Exercise(String.Empty, 1, new List<Posture>(), false), new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Se ejecuta cuando cambia algunas de las propiedades anteriores
        /// </summary>
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) { }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            click_sound.Play();
            try
            {
                this.StartExercise(this.Exercise, new EventArgs());
            }
            catch (System.NullReferenceException) { }
        } 
    }
}
