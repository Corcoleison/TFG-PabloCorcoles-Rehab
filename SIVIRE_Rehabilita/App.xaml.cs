using SIVIRE_Rehabilita.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SIVIRE_Rehabilita
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal Patient CurrentUser { get; set; }
        internal Uri BaseUri { get { return new Uri("pack://application:,,,/"); } }
        public bool Gestures_IsEnabled { get; set; }
        public bool Sound_IsEnabled { get; set; }
        public bool ReadMsg_IsEnabled { get; set; }
        public bool ReadErrorMsg_IsEnabled { get; set; }
    }
}
