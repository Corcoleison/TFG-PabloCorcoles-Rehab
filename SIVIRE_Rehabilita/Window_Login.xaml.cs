using SIVIRE_Rehabilita.Model;
using SIVIRE_Rehabilita.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
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
using System.Windows.Shapes;

namespace SIVIRE_Rehabilita
{
    /// <summary>
    /// Lógica de interacción para Window_Login.xaml
    /// </summary>
    public partial class Window_Login : Window
    {
        string clave = "SIVIRE";
        string default_User = "Alberto";
        string default_Password = "****";

        public Window_Login()
        {
            InitializeComponent();
            this.tb_nickName.Text = default_User;
            this.tb_password.Password = default_Password;
        }
                

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Equals(default_User))
            {
                ((TextBox)sender).Clear();
                ((TextBox)sender).Foreground = Brushes.White;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((PasswordBox)sender).Password.Equals(default_Password))
            {
                ((PasswordBox)sender).Clear();
                ((PasswordBox)sender).Foreground = Brushes.White;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            click_sound.Play();
            List<Exercise> exercises = new List<Exercise>();            
            exercises.Add(XMLParser.loadExercise("\\ejercicioDependencia.xml"));

            exercises.Add(XMLParser.loadExercise("\\ejercicioEval1.xml"));
            exercises.Add(XMLParser.loadExercise("\\ejercicioEval2.xml"));
            exercises.Add(XMLParser.loadExercise("\\ejercicioTest.xml"));
            //exercises.Add(new Exercise("Ejercicio 5", 1, null));
            //exercises.Add(new Exercise("Ejercicio 6", 1, null));
            //exercises.Add(new Exercise("Ejercicio 7", 1, null));
            //exercises.Add(new Exercise("Ejercicio 8", 1, null));
            //exercises.Add(new Exercise("Ejercicio 9", 1, null));
            exercises.Add(XMLParser.loadExercise("\\ejercicioEval3.xml"));
            exercises.Add(XMLParser.loadExercise("\\ejercicioEval1.xml"));
            exercises.Add(XMLParser.loadExercise("\\ejercicioEval2.xml"));
            exercises.Add(XMLParser.loadExercise("\\ejercicioTest.xml"));

            List<Routine> routines = new List<Routine>();
            routines.Add(new Routine("Evaluación Rehabilita", exercises));
            routines.Add(new Routine("Ejercicios para el hombro", exercises));
            routines.Add(new Routine("Tabla 3", exercises));
            routines.Add(new Routine("Tabla 4", exercises));
            
            Patient patient = new Patient(tb_nickName.Text, new DateTime(1994, 11, 3), "H")
            {
                Routines = routines,
                NickName = this.tb_nickName.Text,
            };

            if (this.cb_remember.IsChecked.HasValue && this.cb_remember.IsChecked.Value)
                patient.Password = this.cifrar(this.tb_password.Password);

            App app = ((App)Application.Current);
            app.CurrentUser = patient;
            XMLParser.loadAppPreferences(app);

            Window_Menu window = new Window_Menu();
            window.Show();
            this.Close();
        }


        public string cifrar(string cadena)
        {
            byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.

            byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena); //Arreglo donde guardaremos la cadena descifrada.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
            tripledes.Clear();

            return Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.
        }

        public string descifrar(string cadena)
        {
            byte[] llave;

            byte[] arreglo = Convert.FromBase64String(cadena); // Arreglo donde guardaremos la cadena descovertida.

            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado); // Obtenemos la cadena
            return cadena_descifrada; // Devolvemos la cadena
        }


        #region Dialog

        private void showDialog(string msg, Window windowToShow)
        {
            Dialog_Confirm dialog = new Dialog_Confirm()
            {
                Msg = msg,
                WindowToShow = windowToShow
            };

            dialog.HorizontalAlignment = HorizontalAlignment.Center;
            dialog.VerticalAlignment = VerticalAlignment.Center;
            dialog.OkSelected += dialog_OkSelected;
            dialog.CancelSelected += dialog_CancelSelected;

            ((Storyboard)this.FindResource("showDialog")).Begin();
            this.window_Content.Children.Add(dialog);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.showDialog("¿Abandonar la aplicación?", null);
        }

        private void dialog_CancelSelected(object sender, EventArgs e)
        {
            ((Dialog_Confirm)sender).OkSelected -= dialog_OkSelected;
            ((Dialog_Confirm)sender).CancelSelected -= dialog_CancelSelected;

            this.window_Content.Children.Remove((Dialog_Confirm)sender);
            ((Storyboard)this.FindResource("closeDialog")).Begin();
        }

        private void dialog_OkSelected(object sender, EventArgs e)
        {
            if (((Dialog_Confirm)sender).WindowToShow != null)
                ((Dialog_Confirm)sender).WindowToShow.Show();

            this.Close();
        }

        #endregion

    }
}
