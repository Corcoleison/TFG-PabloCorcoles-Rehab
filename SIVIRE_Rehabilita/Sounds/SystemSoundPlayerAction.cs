using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIVIRE_Rehabilita
{
    public class SystemSoundPlayerAction : System.Windows.Interactivity.TriggerAction<Button>
    {
        public static readonly DependencyProperty SystemSoundProperty =
                        DependencyProperty.Register("SystemSound", typeof(SystemSound), typeof(SystemSoundPlayerAction), new UIPropertyMetadata(null));
        public SystemSound SystemSound
        {
            get { return (SystemSound)GetValue(SystemSoundProperty); }
            set { SetValue(SystemSoundProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            if (SystemSound == null) throw new Exception("No system sound was specified");
            SystemSound.Play();
        }
    }
}
