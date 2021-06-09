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
    /// Lógica de interacción para Menu_Main.xaml
    /// </summary>
    public partial class Menu_Main : UserControl
    {
        public Menu_Main()
        {
            InitializeComponent();
        }

        public event EventHandler RoutineSelected;
        public event EventHandler StartClicked;
        public event EventHandler SettingsClicked;

        public List<Routine> Routines
        {
            get { return (List<Routine>)GetValue(RoutinesProperty); }
            set { SetValue(RoutinesProperty, value); }
        }

        public static readonly DependencyProperty RoutinesProperty =
            DependencyProperty.Register("Routines", typeof(List<Routine>), typeof(Menu_Main), new PropertyMetadata(new List<Routine>(), new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Se ejecuta cuando cambia algunas de las propiedades anteriores
        /// </summary>
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) { }

        private void routine_Click(object sender, RoutedEventArgs e)
        {
            new SoundPlayer(@"Sounds\click.wav").Play();
            var button = (Button)e.OriginalSource;
            Routine routine = button.DataContext as Routine;

            if (routine != null)
            {
                try
                {
                    this.RoutineSelected(routine, new EventArgs());
                }
                catch (System.NullReferenceException) { }
            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            new SoundPlayer(@"Sounds\click.wav").Play();
            try
            {
                this.SettingsClicked(this, new EventArgs());
            }
            catch (System.NullReferenceException) { }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            Exercise exercise = getNextUnfinishedExercise();
            if (exercise != null)
            {
                try
                {
                    this.StartClicked(exercise, new EventArgs());
                }
                catch (System.NullReferenceException) { }
            }

        }

        private Exercise getNextUnfinishedExercise()
        {
            List<Routine> routinesWithExer = new List<Routine>();
            List<Exercise> unfinishedExer;
            foreach (Routine routine in Routines)
            {
                unfinishedExer = routine.getUnfishiedExercise();
                if (unfinishedExer.Count > 0)
                {
                    routinesWithExer.Add(routine);
                    return unfinishedExer[0];
                }
            }
            MessageBox.Show("There are no more exercises to complete");
            return null;
            //if (routinesWithExer.Count > 0)
            //{
            //    return routinesWithExer
            //}
        }
    }
}
