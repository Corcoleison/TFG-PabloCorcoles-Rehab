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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIVIRE_Rehabilita.UserControls
{
    /// <summary>
    /// Lógica de interacción para EquilibriumScale.xaml
    /// </summary>
    public partial class EquilibriumScale : UserControl
    {
        public EquilibriumScale()
        {
            InitializeComponent();
        }

        bool isMoveToRightArrowInitiated = false;
        bool isMoveToLeftArrowInitiated = false;

        public float LeanFactor
        {
            get { return (float)GetValue(LeanFactorProperty); }
            set { SetValue(LeanFactorProperty, value); }
        }

        public static readonly DependencyProperty LeanFactorProperty =
            DependencyProperty.Register("LeanFactor", typeof(float), typeof(EquilibriumScale), new PropertyMetadata((float)0, new PropertyChangedCallback(LeanFactorChanged)));

        private static void LeanFactorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            EquilibriumScale equilibriumScale = sender as EquilibriumScale;
            equilibriumScale.recalculate();
        }

        private void recalculate()
        {            
            this.rotationAngle.Angle = this.LeanFactor * 10; // 1 = 45º --here--> 1 = 10º
            this.in_rectangle.Margin = new Thickness(this.LeanFactor * 260, 0, 0, 0);
            if (this.LeanFactor > 0)
                this.gradationoAngle.Angle = 90;
            else
                this.gradationoAngle.Angle = -90;
            this.gradationoGreen.Offset = Math.Abs(this.LeanFactor);

            
            Storyboard sbToRight = (Storyboard)this.FindResource("moveToRightArrow");
            if (this.LeanFactor < -0.4)
            {
                if (!this.isMoveToRightArrowInitiated)
                {
                    this.isMoveToRightArrowInitiated = true;
                    sbToRight.Begin();
                }
            }
            else
            {
                if (this.isMoveToRightArrowInitiated)
                {
                    this.isMoveToRightArrowInitiated = false;
                    sbToRight.Stop();
                }
            }

            
                Storyboard sbToLeft = (Storyboard)this.FindResource("moveToLeftArrow");
            if (this.LeanFactor > 0.4)
            {
                if (!this.isMoveToLeftArrowInitiated)
                {
                    this.isMoveToLeftArrowInitiated = true;
                    sbToLeft.Begin();
                }
            }
            else
            {
                if (this.isMoveToLeftArrowInitiated)
                {
                    this.isMoveToLeftArrowInitiated = false;
                    sbToLeft.Stop();
                }
            }
        }
    }
}
