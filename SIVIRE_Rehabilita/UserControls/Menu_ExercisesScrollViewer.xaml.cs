using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using SIVIRE_Rehabilita.Model;
using System;
using System.Collections.Generic;
using System.Text;
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
	/// Lógica de interacción para ExercisesScrollViewer.xaml
	/// </summary>
	public partial class Menu_ExercisesScrollViewer : UserControl
	{
		public Menu_ExercisesScrollViewer()
		{
			this.InitializeComponent();
		}

        public event EventHandler ExerciseSelected;

        public Routine Routine
        {
            get { return (Routine)GetValue(RoutineProperty); }
            set { SetValue(RoutineProperty, value); }
        }

        public static readonly DependencyProperty RoutineProperty =
            DependencyProperty.Register("Routine", typeof(Routine), typeof(Menu_ExercisesScrollViewer), new PropertyMetadata(new Routine(String.Empty, new List<Exercise>()), new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Se ejecuta cuando cambia algunas de las propiedades anteriores
        /// </summary>
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) { }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            Exercise exercise = button.DataContext as Exercise;

            if (exercise != null)
            {
                try
                {
                    this.ExerciseSelected(exercise, new EventArgs());
                }
                catch (System.NullReferenceException) { }
            }
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToLeftEnd();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToRightEnd();
        }

    }
}