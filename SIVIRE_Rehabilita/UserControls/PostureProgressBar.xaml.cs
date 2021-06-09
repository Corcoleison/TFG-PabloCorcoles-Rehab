using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SIVIRE_Rehabilita.UserControls
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class PostureProgressBar : UserControl
    {
        public PostureProgressBar()
        {
            InitializeComponent();
            this.recalculateExerciseProgress();
        }

        public int NumPostures
        {
            get { return (int)GetValue(NumPosturesProperty); }
            set { SetValue(NumPosturesProperty, value); }
        }

        public int CurrentPosture
        {
            get { return (int)GetValue(CurrentPostureProperty); }
            set { SetValue(CurrentPostureProperty, value); }
        }

        public int PostureProgress
        {
            get { return (int)GetValue(PostureProgressProperty); }
            set { SetValue(PostureProgressProperty, value); }
        }

        public int CheckPostureProgress
        {
            get { return (int)GetValue(CheckPostureProgressProperty); }
            set { SetValue(CheckPostureProgressProperty, value); }
        }

        public static readonly DependencyProperty NumPosturesProperty =
            DependencyProperty.Register("NumPostures", typeof(int), typeof(PostureProgressBar), new PropertyMetadata(1, new PropertyChangedCallback(CurrentPosturePropertyChanged)));

        public static readonly DependencyProperty CurrentPostureProperty =
            DependencyProperty.Register("CurrentPosture", typeof(int), typeof(PostureProgressBar), new PropertyMetadata(0, new PropertyChangedCallback(CurrentPosturePropertyChanged)));

        public static readonly DependencyProperty PostureProgressProperty =
            DependencyProperty.Register("PostureProgress", typeof(int), typeof(PostureProgressBar), new PropertyMetadata(0, new PropertyChangedCallback(PostureProgressPropertyChanged)));

        public static readonly DependencyProperty CheckPostureProgressProperty =
            DependencyProperty.Register("CheckPostureProgress", typeof(int), typeof(PostureProgressBar), new PropertyMetadata(0, new PropertyChangedCallback(CheckPostureProgressPropertyChanged)));

        private static void CurrentPosturePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PostureProgressBar progressBar = sender as PostureProgressBar;
            progressBar.restartPostureProgress();
            //progressBar.restartCheckProgress();
            progressBar.recalculateExerciseProgress();
        }

        private static void PostureProgressPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PostureProgressBar progressBar = sender as PostureProgressBar;
            progressBar.restartCheckProgress();
            progressBar.recalculatePostureProgress();            
        }

        private static void CheckPostureProgressPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PostureProgressBar progressBar = sender as PostureProgressBar;
            progressBar.recalculateCheckProgress();
        }

        private void recalculateExerciseProgress()
        {
            if (this.CurrentPosture <= this.NumPostures)
            {
                //this.lbl_postureCountdown.Content = this.NumPostures - this.CurrentPosture;
                this.lbl_postureCountdown.Content = this.CurrentPosture;
                this.lbl_leftPostureCountdown.Content = this.NumPostures;

                // Obtenemos el recurso y le cambiamos el valor para que la barra se mueva con la animación
                EasingDoubleKeyFrame aux = (EasingDoubleKeyFrame)this.FindResource("newCalculatedPercentage");
                if (NumPostures == 0)
                    aux.Value = 0;
                else
                    aux.Value = (CurrentPosture * 100) / NumPostures;
                // Ejecutamos la animación
                Storyboard sb = (Storyboard)this.FindResource("advanceAnimation");
                sb.Begin();
            }
            else
            {
                this.lbl_postureCountdown.Content = 0;
                this.lbl_leftPostureCountdown.Content = this.NumPostures;
                this.CurrentPosture = this.NumPostures;
            }
        }

        public void recalculatePostureProgress()
        {
            EasingDoubleKeyFrame aux = (EasingDoubleKeyFrame)this.FindResource("newCalculatedPercentage");
            aux.Value = this.PostureProgress;

            Storyboard sb = (Storyboard)this.FindResource("advanceAnimation_PostureProgress");
            sb.Begin();
        }

        public void restartPostureProgress()
        {
            EasingDoubleKeyFrame aux = (EasingDoubleKeyFrame)this.FindResource("newCalculatedPercentage");
            aux.Value = 0;

            Storyboard sb = (Storyboard)this.FindResource("advanceAnimation_PostureProgress");
            sb.Begin();
        }

        public void restartCheckProgress()
        {
            // Restart the countdown
            EasingDoubleKeyFrame aux = (EasingDoubleKeyFrame)this.FindResource("newCheckPercentage");
            aux.Value = 0;

            Storyboard sb = (Storyboard)this.FindResource("advanceAnimation_CheckPosture");
            sb.Begin();
        }

        private void recalculateCheckProgress()
        {
            if (this.postureProgress1.Percentage == 100)
            {
                // Start the countdown
                EasingDoubleKeyFrame aux = (EasingDoubleKeyFrame)this.FindResource("newCheckPercentage");
                aux.Value = this.CheckPostureProgress;

                Storyboard sb = (Storyboard)this.FindResource("advanceAnimation_CheckPosture");
                sb.Begin();
            }
            else
                this.restartCheckProgress();
        }

        public void next()
        {
            this.CurrentPosture++;
        }
    }
}
