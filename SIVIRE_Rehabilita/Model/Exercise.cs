using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SIVIRE_Rehabilita.Model
{
    public class Exercise : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Constants

        #endregion


        #region Attributes

        string name;
        int numRepetitions;
        List<Posture> postures;
        bool equilibriumScale;
        bool guideVoice;
        bool avatar;
        bool finished;
        private ImageSource animation = null;

        bool posMatters; //Variable de dependencia o no de la posición


        /// <summary>
        /// Index of the posture that is currently checked
        /// </summary>
        int indexCurrentPosture;

        /// <summary>
        /// Current repetition of the exercise
        /// </summary>
        int currentRepetition;

        Timer timer_PrevPosture;

        #endregion


        #region Properties

        public string Name { get { return this.name; } }

        public int NumRepetitions { get { return this.numRepetitions; } }

        public List<Posture> Postures 
        { 
            get { return this.postures; }
            set
            {
                this.postures = new List<Posture>();
                if (value != null && value.Count > 0)
                {
                    this.postures.AddRange(value);
                    this.IndexCurrentPosture = 0;
                }
            }
        }

        /*Propiedad para indicar si depende o no el ejercicio de la posición*/
        public bool PositionMatters
        {
            get { return this.posMatters; }
            set { this.posMatters = value; }
        }


        public bool HasEquilibriumScale
        {
            get { return this.equilibriumScale; }
            set { this.equilibriumScale = value; }
        }

        public bool IsGuideVoiceActive
        {
            get { return this.guideVoice; }
            set { this.guideVoice = value; }
        }

        public bool ShowAvatar
        {
            get { return this.avatar; }
            set { this.avatar = value; }
        }

        public bool Finished
        {
            get { return this.finished; }
            set { this.finished = value; }
        }

        public Posture CurrentPosture
        {
            get
            {
                if (this.postures.Count > 0)
                    return postures[this.IndexCurrentPosture];
                else
                    return null;
            }
        }

        public int IndexCurrentPosture
        {
            get { return this.indexCurrentPosture; }
            private set 
            {
                ((EndPosture)this.CurrentPosture).PostureReached -= postureReached;
                ((EndPosture)this.CurrentPosture).PreviousPosture -= previousPosture;
                this.indexCurrentPosture = value;
                OnPropertyChanged("IndexCurrentPosture");
                ((EndPosture)this.CurrentPosture).PostureReached += postureReached;
                ((EndPosture)this.CurrentPosture).PreviousPosture += previousPosture;
            }
        }        

        public ImageSource Animation 
        { 
            get { return this.animation; }
        }
        
        #endregion


        #region Events

        public event EventHandler NextPosture;
        public event EventHandler PreviousPosture;
        public event EventHandler NextRepetition;
        public event EventHandler ExerciseCompleted;

        #endregion


        #region Constructors

        public Exercise(string name, int numRepetitions, List<Posture> postures, bool pMatters)
        {
            this.name = name;
            this.numRepetitions = numRepetitions;
            this.Postures = postures;
            this.Finished = false;

            this.posMatters = pMatters;

            Uri imagePath = new Uri("Images/exerciseGeneric.png", UriKind.Relative);
            if (imagePath != null)
                this.animation = new BitmapImage(new Uri(((App)Application.Current).BaseUri, imagePath));

            this.currentRepetition = 0;
        }

        #endregion


        #region Public methods

        public void restart()
        {
            this.currentRepetition = 0;
            this.CurrentPosture.restart();
            this.IndexCurrentPosture = 0;
        }

        public List<Model3DGroup> getExerciseAnimation()
        {
            List<Model3DGroup> listAvatars = new List<Model3DGroup>();
            List<Model3DGroup> listTransitionAvatars = new List<Model3DGroup>();
            foreach (Posture posture in postures)
            {
                EndPosture endposture = (EndPosture)posture;
                bool endPostureBool = posture is EndPosture;
                if (endPostureBool && endposture.Transition.Count > 0)
                {
                    listTransitionAvatars = endposture.GetTransitionPostureAvatars();
                    foreach (Model3DGroup transitionAvatar in listTransitionAvatars)
                    {
                        listAvatars.Add(transitionAvatar);
                    }
                }
                listAvatars.Add(posture.GetPostureAvatar());
            }
            return listAvatars;
        }

        #endregion


        #region Private methods

        private void postureReached(object sender, EventArgs e)
        {
            if (this.IndexCurrentPosture < this.postures.Count - 1)
            {
                this.IndexCurrentPosture++;
                this.NextPosture(this, new EventArgs());
            }
            else
            {                
                this.currentRepetition++;
                if (this.currentRepetition < this.numRepetitions)
                {
                    this.IndexCurrentPosture = 0;
                    this.NextRepetition(this, new EventArgs());
                }
                else
                {
                    this.NextRepetition(this, new EventArgs());
                    this.ExerciseCompleted(this, new EventArgs());
                }
            }            
        }

        private void previousPosture(object sender, EventArgs e)
        {
            timer_PrevPosture = new Timer(TimerCallback, null, 0, Timeout.Infinite);            
        }

        private void TimerCallback(object state)
        {
            Thread.Sleep(1500);

            if (this.IndexCurrentPosture != 0)
            {
                this.IndexCurrentPosture--;
            }
            this.PreviousPosture(this, new EventArgs());
            this.timer_PrevPosture.Dispose();
        }

        #endregion
    }
}
