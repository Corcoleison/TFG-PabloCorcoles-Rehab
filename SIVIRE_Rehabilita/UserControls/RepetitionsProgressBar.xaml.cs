using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SIVIRE_Rehabilita.UserControls
{
    /// <summary>
    /// Lógica de interacción para RepetitionsProgressBar.xaml
    /// </summary>
    public partial class RepetitionsProgressBar : UserControl
    {
        public RepetitionsProgressBar()
        {
            InitializeComponent();
            this.recalculate();
            // Recalculamos de forma manual para no ejecutar la animación
            //this.circularProgress.Percentage = (CurrentRepetition * 100) / NumRepetitions;
        }

        public int NumRepetitions
        {
            get { return (int)GetValue(NumRepetitionsProperty); }
            set { SetValue(NumRepetitionsProperty, value); }
        }

        public int CurrentRepetition
        {
            get { return (int)GetValue(CurrentRepetitionProperty); }
            set { SetValue(CurrentRepetitionProperty, value); }
        }

        public static readonly DependencyProperty NumRepetitionsProperty =
            DependencyProperty.Register("NumRepetitions", typeof(int), typeof(RepetitionsProgressBar), new PropertyMetadata(1, new PropertyChangedCallback(OnPropertyChanged)));

        public static readonly DependencyProperty CurrentRepetitionProperty =
            DependencyProperty.Register("CurrentRepetition", typeof(int), typeof(RepetitionsProgressBar), new PropertyMetadata(0, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Se ejecuta cuando cambia algunas de las propiedades anteriores
        /// </summary>
        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            RepetitionsProgressBar progressBar = sender as RepetitionsProgressBar;
            progressBar.recalculate();
        }

        private void recalculate()
        {
            if (this.CurrentRepetition <= this.NumRepetitions)
            {
                // Obtenemos el recurso y le cambiamos el valor para que la barra se mueva con la animación
                EasingDoubleKeyFrame aux = (EasingDoubleKeyFrame)this.FindResource("newCalculatedPercentage");
                if (this.NumRepetitions == 0)
                    aux.Value = 0;
                else
                    aux.Value = (this.CurrentRepetition * 100) / this.NumRepetitions;
                // Ejecutamos la animación
                Storyboard sb1 = (Storyboard)this.FindResource("advanceAnimation");
                sb1.Begin();
            }
            else
                this.CurrentRepetition = this.NumRepetitions;
        }

        public void next()
        {
            this.CurrentRepetition++;
        }
    }
}
