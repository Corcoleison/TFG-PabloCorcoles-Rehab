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
using System.Windows.Shapes;
using Microsoft.Kinect.Wpf.Controls;
using Microsoft.Kinect;
using SIVIRE_Rehabilita.Gestures;
using SIVIRE_Rehabilita.Model;
using SIVIRE_Rehabilita.UserControls;
using System.Windows.Media.Animation;

namespace SIVIRE_Rehabilita
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class Window_Menu : Window
    {
        #region Kinect

        KinectSensor kinectSensor;
        MultiSourceFrameReader frameReader;
        Body[] bodies;
        List<Gesture> gestures;

        #endregion


        #region Properties

        UIElement currentPage;
        public UIElement CurrentPage
        {
            get { return this.currentPage; }
            set
            {                
                this.navigationRegion.Children.Clear();
                this.navigationRegionHigh.Children.Clear();
                this.currentPage = value;

                if (value.GetType().Name.Equals("Menu_Main"))
                    this.navigationRegion.Children.Add(this.currentPage);
                else
                    this.navigationRegionHigh.Children.Add(this.currentPage);
            }
        }

        public Patient User { get { return ((App)Application.Current).CurrentUser; } }

        #endregion


        public Window_Menu()
        {
            InitializeComponent();
            this.DataContext = this.User;
        }
        
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.initializeKinect();
            this.showMenu_Main();
            this.showTutorial();
        }

        public void showTutorial()
        {
            if (this.User.FirstTime)
            {
                //Show tutorial
                this.User.FirstTime = false;
            }
        }

        void frameReader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            /*// Color
            using (var colorFrame = reference.ColorFrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    // Do something with the frame...
                }
            }

            // Depth
            using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    // Do something with the frame...
                }
            }

            // Infrared
            using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (infraredFrame != null)
                {
                    // Do something with the frame...
                }
            }*/

            // Body
            using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null)
                        bodies = new Body[bodyFrame.BodyCount];

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(bodies);

                    if (bodies.Length > 0)
                    {
                        var user = bodies.Where(u => u.IsTracked).FirstOrDefault();

                        if (user != null)
                        {
                            if (((App)Application.Current).Gestures_IsEnabled)
                            {
                                foreach (Gesture gesture in this.gestures)
                                {
                                    gesture.update(user);
                                }
                            }
                        }
                    }
                }
            }
        }


        #region Auxiliar Methods
        
        private void initializeKinect()
        {
            // Kinect Sensor
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                if (!this.kinectSensor.IsOpen)
                    this.kinectSensor.Open();

                // open the reader for the kinect frames
                this.frameReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
                this.frameReader.MultiSourceFrameArrived += frameReader_MultiSourceFrameArrived;

                // Gestures
                this.gestures = new List<Gesture>();

                var gesture = GestureFactory.getGesture(GestureType.CrossedArms);
                gesture.gestureRecognized += crossedArms_Recognized;
                gestures.Add(gesture);

                gesture = GestureFactory.getGesture(GestureType.RigthHandToLeftSide);
                gesture.gestureRecognized += rigthHandToLeftSide_Recognized;
                gestures.Add(gesture);

                // Kinect Region
                this.kinectRegion.KinectSensor = this.kinectSensor;
            }  
        }

        private void restartWindowContent()
        {
            this.exerciseInfoRegion.Children.Clear();            
            ((Storyboard)this.FindResource("closeNavigationBar")).Begin();
            this.btn_Back.Visibility = Visibility.Hidden;
            this.btn_Exit.Visibility = Visibility.Visible;

            this.exercisesScrollViewer.Visibility = Visibility.Hidden;
            this.exercisesScrollViewer.Routine = null;

            this.btn_userProfile.IsEnabled = true;
            this.btn_logout.Visibility = Visibility.Hidden;
        }

        private void changeNavigationContent(string newLocation)
        {
            this.btn_Back.Visibility = Visibility.Visible;
            this.btn_Exit.Visibility = Visibility.Hidden;

            this.navigationBar_Location.Content = newLocation;
            ((Storyboard)this.FindResource("showNavigationBar")).Begin();
        }

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

        private void moveBack()
        {
            // If the exercises scroll is visible it does not restart the window
            if (this.exercisesScrollViewer.Visibility == Visibility.Visible)
            {
                this.exerciseInfoRegion.Children.Clear();
                ((Storyboard)this.FindResource("closeNavigationBar")).Begin();
                this.btn_Back.Visibility = Visibility.Hidden;
                this.btn_Exit.Visibility = Visibility.Visible;
            }
            else
            {
                this.showMenu_Main();
            }
        }

        private void showMenu_Main()
        {
            Menu_Main main = new Menu_Main() { Routines = this.User.Routines };
            main.RoutineSelected += routineSelected;
            main.SettingsClicked += main_SettingsClicked;
            this.CurrentPage = main;
            this.restartWindowContent();
        }

        private void closeWindow()
        {
            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }

            this.Close();
        }

        #endregion
                

        #region Event Handlers

        private void routineSelected(object sender, EventArgs e)
        {
            Routine routine = (Routine)sender;
            this.exercisesScrollViewer.Routine = routine;
            this.exercisesScrollViewer.Visibility = Visibility.Visible;
        }

        private void main_SettingsClicked(object sender, EventArgs e)
        {
            this.restartWindowContent();
            this.changeNavigationContent("Ajustes");

            Menu_Settings settings = new Menu_Settings();
            this.CurrentPage = settings;

            this.btn_userProfile.IsEnabled = false;
        }

        private void exerciseSelected(object sender, EventArgs e)
        {
            Exercise exercise = (Exercise)sender;            

            if (exercise != null)
            {
                this.changeNavigationContent("Ficha de ejercicio");

                var selectionDisplay = new Menu_ExerciseSelected() { Exercise = exercise };
                selectionDisplay.StartExercise += startExercise;

                // Selection dialog covers the entire interact-able area, so the current press interaction
                // should be completed. Otherwise hover capture will allow the button to be clicked again within
                // the same interaction (even whilst no longer visible).
                selectionDisplay.Focus();

                // Since the selection dialog covers the entire interact-able area, we should also complete
                // the current interaction of all other pointers.  This prevents other users interacting with elements
                // that are no longer visible.
                this.kinectRegion.InputPointerManager.CompleteGestures();

                this.exerciseInfoRegion.Children.Clear();
                this.exerciseInfoRegion.Children.Add(selectionDisplay);
            }
        }

        private void startExercise(object sender, EventArgs e)
        {
            Window_MonitorExercise window = new Window_MonitorExercise((Exercise)sender);
            window.Show();
            this.Close();
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

            this.closeWindow();
        }
                
        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.frameReader != null)
            {
                this.frameReader.Dispose();
                this.frameReader = null;
            }

            XMLParser.saveAppPreferences((App)Application.Current);
        }

        #endregion


        #region Gestures_Event_Handlers

        private void crossedArms_Recognized(object sender, EventArgs e)
        {
            if (this.btn_Exit.Visibility == Visibility.Visible)
                this.closeWindow();
        }

        private void rigthHandToLeftSide_Recognized(object sender, EventArgs e)
        {
            if (this.btn_Back.Visibility == Visibility.Visible)
                this.moveBack();
        }


        #endregion


        #region Events_Click

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            this.restartWindowContent();
            this.changeNavigationContent("Perfil de usuario");

            Menu_UserProfile profile = new Menu_UserProfile() { DataContext = this.User };
            this.CurrentPage = profile;

            this.btn_logout.Visibility = Visibility.Visible;
            this.btn_userProfile.IsEnabled = false;

            //MessageBox.Show(KinectRegion.GetIsPressTarget(this.btn_Profile).ToString());
            //var engagementModel = new HandOverheadEngagementModel(1);
            //this.kinectRegion.SetKinectOnePersonManualEngagement(engagementModel);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.showDialog("¿Quiere cerrar sesión?", new Window_Login());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.moveBack();           
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.showDialog("¿Abandonar la aplicación?", null);
        }

        #endregion

    }
}
