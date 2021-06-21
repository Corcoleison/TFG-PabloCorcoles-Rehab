using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using SIVIRE_Rehabilita.Model;
using SIVIRE_Rehabilita.UserControls;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Speech.Synthesis;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace SIVIRE_Rehabilita
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class Window_MonitorExercise : Window
    {
        #region Kinect

        KinectSensor kinectSensor;

        /// <summary>
        /// Reader for kinect frames
        /// </summary>
        MultiSourceFrameReader frameReader;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        Body[] bodies = null;

        #endregion

        #region Attributes

        DrawingGroup postureDrawingGroup;
        DrawingGroup userDrawingGroup;
        Exercise exercise;
        bool isExcercisePaused;
        int repetitionErrors;

        Model3DGroup avatarGroupModel;

        Model3D avatarModel;

        #endregion


        #region Properties
        /// <summary>
        /// This is used to Binding properties
        /// </summary>
        public Exercise ExerciseToMonitor
        {
            get { return this.exercise; }
        }

        public Model3D our_Model { get; set; }
        #endregion


        public Window_MonitorExercise(Exercise exercise)
        {
            this.exercise = exercise;
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.setUpTextToSpeech();
            this.loadExercise();
            this.isExcercisePaused = false;
            this.initializeKinect();            

            // Create the drawing group we'll use for drawing
            this.postureDrawingGroup = new DrawingGroup();
            this.userDrawingGroup = new DrawingGroup();
            

            // Display the drawing using our image control
            posture_Skeleton.Source = new DrawingImage(this.postureDrawingGroup);
            user_Skeleton.Source = new DrawingImage(this.userDrawingGroup);
            
     
            //Confirm Windows
            Window_Confirm();

            // Display the model
            getCurrentAvatar();

        }

        private void initializeKinect()
        {
            // Kinect Sensor
            this.kinectSensor = KinectSensor.GetDefault();

            if (this.kinectSensor != null)
            {
                if (!this.kinectSensor.IsOpen)
                    this.kinectSensor.Open();

                // open the reader for the kinect frames
                this.frameReader = this.kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
                this.frameReader.MultiSourceFrameArrived += frameReader_MultiSourceFrameArrived;

                // Kinect Region
                this.kinectRegion.KinectSensor = this.kinectSensor;


                
            }
        }

        private void getCurrentAvatar()
        {
            foo.Content = this.exercise.CurrentPosture.GetPostureAvatar();
            //gridLines.Visible = false;
        }

        void drawCascadeSkeletons()
        {
            var listPostures = ExerciseToMonitor.Postures;
            DrawingGroup[] cascadeDrawingSkeletonList = new DrawingGroup[listPostures.Count];
            //cascadeDrawingSkeletonList.InsertRange(listPostures.Count, cascadeDrawingSkeletonList);
            var i = 0;
            foreach (var posture in listPostures)
            {
                var selectedSkeletonImage = (Image)this.FindName("cascade_Skeleton"+i);
                if (selectedSkeletonImage != null)
                {
                    cascadeDrawingSkeletonList[i] = new DrawingGroup();
                    selectedSkeletonImage.Source = new DrawingImage(cascadeDrawingSkeletonList[i]);
                    var skeletonPosture = posture.Skeleton;
                    //var sensorKinect = this.kinectSensor;
                    posture.Draw(skeletonPosture, cascadeDrawingSkeletonList[i], this.kinectSensor);
                    i++;
                }
            }
            for(int x=i; x <= 10; x++)
            {
                var selectedSkeletonBorder = (Border)this.FindName("border_Skeleton" + x);
                if (selectedSkeletonBorder != null)
                {
                    selectedSkeletonBorder.Visibility = Visibility.Hidden;
                }
            }

        }

        private void moveCascadeSkeletons()
        {
            var listPostures = ExerciseToMonitor.Postures;
            var converter = new System.Windows.Media.BrushConverter();
            var moradoBrush = (Brush)converter.ConvertFromString("#CCC700FF");
            var grisBrush = (Brush)converter.ConvertFromString("#CC747474");
            foreach (var posture in listPostures)
            {
                if (posture == this.exercise.CurrentPosture)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var numberActualPosture = listPostures.IndexOf(posture);
                        var selectedSkeletonBorder = (Border)this.FindName("border_Skeleton" + numberActualPosture);
                        Border toChangeSkeletonBorder;
                        Border toChangeSkeletonBorder2;
                        if (selectedSkeletonBorder != null)
                        {
                            for(int i = 0; i <= listPostures.Count; i++)
                            {
                                toChangeSkeletonBorder = (Border)this.FindName("border_Skeleton" + (i));
                                toChangeSkeletonBorder.Background = grisBrush;
                            }
                            selectedSkeletonBorder.Background = moradoBrush;
                            //switch (numberActualPosture)
                            //{
                            //    case 0:
                            //        selectedSkeletonBorder.Background = moradoBrush;
                            //        toChangeSkeletonBorder = (Border)this.FindName("border_Skeleton" + (numberActualPosture + 1));
                            //        toChangeSkeletonBorder2 = (Border)this.FindName("border_Skeleton" + (numberActualPosture + 2));
                            //        toChangeSkeletonBorder.Background = grisBrush;
                            //        toChangeSkeletonBorder2.Background = grisBrush;
                            //        break;
                            //    case 1:
                            //        selectedSkeletonBorder.Background = moradoBrush;
                            //        toChangeSkeletonBorder = (Border)this.FindName("border_Skeleton" + (numberActualPosture - 1));
                            //        toChangeSkeletonBorder2 = (Border)this.FindName("border_Skeleton" + (numberActualPosture + 1));
                            //        toChangeSkeletonBorder.Background = grisBrush;
                            //        toChangeSkeletonBorder2.Background = grisBrush;
                            //        break;
                            //    case 2:
                            //        selectedSkeletonBorder.Background = moradoBrush;
                            //        toChangeSkeletonBorder = (Border)this.FindName("border_Skeleton" + (numberActualPosture - 1));
                            //        toChangeSkeletonBorder2 = (Border)this.FindName("border_Skeleton" + (numberActualPosture - 2));
                            //        toChangeSkeletonBorder.Background = grisBrush;
                            //        toChangeSkeletonBorder2.Background = grisBrush;
                            //        break;

                            //}

                        }
                    });
                    
                }
            }
        }

        void frameReader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var colorFrame = reference.ColorFrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    this.img_ColorFrame.Source = this.ToBitmap(colorFrame);
                }
            }

            // Body
            using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
            {
                if (isExcercisePaused)  // User can pause the exercise
                {
                    this.postureDrawingGroup.Children.Clear();
                    this.userDrawingGroup.Children.Clear();
                }
                else
                {
                    if (bodyFrame != null)
                    {
                        if (bodies == null)
                            bodies = new Body[bodyFrame.BodyCount]; // 6 bodies can be tracked

                        bodyFrame.GetAndRefreshBodyData(bodies);

                        foreach (Body body in this.bodies)
                        {
                            if (body.IsTracked)
                            {
                                Skeleton userSkeleton = new Skeleton(body);

                                if (body.LeanTrackingState == TrackingState.Tracked)
                                {
                                    // Leaning left and right corresponds to X movement
                                    this.equilibriumScale.LeanFactor = body.Lean.X;
                                }

                                Posture currentPostureToCheck = this.exercise.CurrentPosture;
                                this.moveCascadeSkeletons();


                                if (currentPostureToCheck != null)
                                {
                                    if (currentPostureToCheck.CurrentStage == PostureStageType.StageFinalCheck)
                                        this.postureDrawingGroup.Children.Clear();
                                    else
                                        currentPostureToCheck.Draw(userSkeleton, this.postureDrawingGroup, this.kinectSensor);
                                        

                                    List<Message> msgsToShow = userSkeleton.checkAndDrawSkeleton(currentPostureToCheck, this.kinectSensor, this.userDrawingGroup);
                                    this.writeMessages(msgsToShow);
                                }
                            }
                        }
                    }
                }
            }
        }

        public ImageSource ToBitmap(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        public void writeMessages(List<Message> msgs)
        {
            if (msgs.Count == 0)
                this.errorMsgPanel.ErrorMsg = String.Empty;
            else if (msgs[0].Type == MessageType.Error)
            {
                this.errorMsgPanel.ErrorMsg = msgs[0].Msg;
                this.repetitionErrors++;
            }
            else
            {
                if (!this.guideMsgPanel.GuideMsg.Equals(msgs[0].Msg))
                    this.readMessage(msgs[0].Msg);
                this.guideMsgPanel.GuideMsg = msgs[0].Msg;
            }

        }

        private void loadExercise()
        {
            this.exercise.restart();

            this.exercise.NextPosture += exercise_NextPosture;
            this.exercise.PreviousPosture += exercise_PreviousPosture;
            this.exercise.NextRepetition += exercise_NextRepetition;
            this.exercise.ExerciseCompleted += exercise_ExerciseCompleted;
        }

        private void setBindings()
        {
            if (this.exercise.CurrentPosture != null)
            {
                Binding postureProgressBinding = new Binding("CompletedPercentage");
                postureProgressBinding.Source = this.exercise.CurrentPosture;
                this.postureProgressBar.SetBinding(PostureProgressBar.PostureProgressProperty, postureProgressBinding);

                Binding checkProgressBinding = new Binding("FinalCheckPercentage");
                checkProgressBinding.Source = this.exercise.CurrentPosture;
                this.postureProgressBar.SetBinding(PostureProgressBar.CheckPostureProgressProperty, checkProgressBinding);
            }
        }

        private void exercise_PreviousPosture(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new System.Action(() =>
            {
                this.writeMessages(new List<Message>());
                this.setBindings();
                this.getCurrentAvatar();
            }), null);
        }

        private void exercise_NextPosture(object sender, EventArgs e)
        {
            SoundPlayer postureReached_sound = new SoundPlayer(Properties.Resources.postureReached);
            postureReached_sound.Play();
            this.setBindings();
            this.showProgressMessageAsync();
            this.getCurrentAvatar();

        }

        private void exercise_NextRepetition(object sender, EventArgs e)
        {
            SoundPlayer postureReached_sound = new SoundPlayer(Properties.Resources.postureReached);
            postureReached_sound.Play();
            this.repetitionsProgressBar.next();
            this.setBindings();
            this.getCurrentAvatar();
            this.showProgressMessageAsync();

        }

        private async Task showProgressMessageAsync()
        {
            var converter = new System.Windows.Media.BrushConverter();
            var verdeBrush = (Brush)converter.ConvertFromString("#7F59FF00");
            var amarilloBrush = (Brush)converter.ConvertFromString("#7FFFAE00");
            var narajanBrush = (Brush)converter.ConvertFromString("#7FFF6500");
            this.progress_StackPanel.Visibility = Visibility.Visible;
            if (this.repetitionErrors <= 0)
            {
                this.progress_StackPanel_Label.Content = "Perfecto";
                this.progress_StackPanel.Background = verdeBrush;
            }else if(this.repetitionErrors > 1 || this.repetitionErrors <= 3)
            {
                this.progress_StackPanel_Label.Content = "Bien";
                this.progress_StackPanel.Background = amarilloBrush;
            }
            else if (this.repetitionErrors > 3)
            {
                this.progress_StackPanel_Label.Content = "Sigue Practicando";
                this.progress_StackPanel.Background = narajanBrush;
            }
            this.repetitionErrors = 0;
            await Task.Delay(3000);
            this.progress_StackPanel.Visibility = Visibility.Hidden;
        }

        private void exercise_ExerciseCompleted(object sender, EventArgs e)
        {
            SoundPlayer excerciseCompleted_sound = new SoundPlayer(Properties.Resources.excerciseCompleted);
            excerciseCompleted_sound.Play();

            this.postureProgressBar.CurrentPosture++;

            this.isExcercisePaused = true;
            List<Message> aux = new List<Message>();
            aux.Add(new Message("¡Enhorabuena! Ha completado el ejercicio", new List<JointType>(), MessageType.Guide));
            this.writeMessages(aux);
            this.exercise.Finished = true;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            click_sound.Play();
            this.isExcercisePaused = true;
            List<Message> aux = new List<Message>();
            aux.Add(new Message("Ejercicio pausado", new List<JointType>(), MessageType.Guide));
            this.writeMessages(aux);
            this.btn_Play.Visibility = Visibility.Visible;
            this.btn_Pause.Visibility = Visibility.Hidden;
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            click_sound.Play();
            this.isExcercisePaused = false;
            this.btn_Pause.Visibility = Visibility.Visible;
            this.btn_Play.Visibility = Visibility.Hidden;
        }

        private void Window_Confirm()
        {
            this.confirmRegion.Visibility = Visibility.Visible;
            this.isExcercisePaused = true;
            List<Message> aux = new List<Message>();
            this.writeMessages(aux);
            //Menu confirmation
            this.exerciseName.Content = this.exercise.Name;
            this.exerciseDes.Content = "Descripción:...";
            this.exerciseImg.Source = this.exercise.Animation;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            click_sound.Play();
            this.confirmRegion.Visibility = Visibility.Hidden;
            this.isExcercisePaused = false;
            //Dibujar los esqueletos en cascada
            drawCascadeSkeletons();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            click_sound.Play();
            this.exercise.restart();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer click_sound = new SoundPlayer(Properties.Resources.click);
            click_sound.Play();
            Window_Menu window = new Window_Menu();
            window.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.frameReader != null)
            {
                this.frameReader.Dispose();
                this.frameReader = null;
            }
        }

        

        #region TextToSpeech

        SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        private void setUpTextToSpeech()
        {
            foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
            {
                if (voice.VoiceInfo.Name == "Helena") // Helena is an spanish girl
                {
                    synthesizer.SelectVoice(voice.VoiceInfo.Name);
                    synthesizer.Volume = 70; // 1-100
                    synthesizer.Rate = 1; //1-10
                }
            }
        }

        private void readMessage(string msg)
        {
            if (((App)Application.Current).ReadMsg_IsEnabled)
            {
                TTS tts = new TTS(msg, synthesizer);
                new Thread(new ThreadStart(tts.readMsg)).Start();
            }
        }

        private class TTS
        {
            string msgToRead;
            SpeechSynthesizer synthesizer;
            public TTS(string msg, SpeechSynthesizer synthesizer) { this.msgToRead = msg; this.synthesizer = synthesizer; }
            public void readMsg() { this.synthesizer.Speak(this.msgToRead); }
        }

        #endregion
    }
}
