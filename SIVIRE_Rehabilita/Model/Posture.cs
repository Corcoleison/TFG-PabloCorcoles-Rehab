using HelixToolkit.Wpf;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SIVIRE_Rehabilita.Model
{
    public abstract class Posture : INotifyPropertyChanged
    {

        #region Constants

        /// <summary>
        /// Joint used to translation
        /// </summary>
        readonly JointType originToTranslate = JointType.SpineBase;

        /// <summary>
        /// Joint used to scale
        /// </summary>
        readonly JointType originToScale = JointType.SpineBase;

        /// <summary>
        /// Joints of the extremities
        /// </summary>
        readonly JointType[] finalJoints = { JointType.Head, JointType.HandRight, JointType.HandLeft, JointType.FootRight, JointType.FootLeft };

        #endregion


        #region Atrributes

        protected string name;
        protected Skeleton skeleton;
        protected double errorThreshold;
        protected List<Message> guideMsgs;
        protected List<Message> errorMsgs;
        protected Message joints;
        protected PostureStage stage;


        bool posMatters; //Variable de dependencia o no de la posición


        /// <summary>
        /// Number of required checks to complete
        /// </summary>
        public int checksToComplete;
        protected List<Message> checksCompleted;

        /// <summary>
        /// Times the user fits in the posture
        /// </summary>
        protected int numberOkChecks;

        /// <summary>
        /// Times the user doesn't fit in the posture
        /// </summary>
        protected int numberWrongChecks;

        /// <summary>
        /// The user has to fit in the posture at least these number of times
        /// </summary>
        protected int minNumberOkChecks;  

        /// <summary>
        /// Skeleton after transform the skeleton of the posture
        /// </summary>
        protected Skeleton skeletonScaled;

        #endregion


        #region Properties

        public string Name { get { return this.name; } }
        public Skeleton Skeleton { get { return this.skeleton; } }

        /*Propiedad para indicar si depende o no el ejercicio de la posición*/
        public bool PositionMatters
        {
            get { return this.posMatters; }
            set { this.posMatters = value; }
        }

        public PostureStageType CurrentStage
        {
            get { return this.stage.Type; }
            protected set { this.stage = PostureStage.getStage(value); }
        }

        public int FinalCheckPercentage
        {
            get { return (this.numberOkChecks * 100) / this.minNumberOkChecks; ; }
        }        

        public int CompletedPercentage
        {
            get { return (this.checksCompleted.Count * 100) / this.checksToComplete; }
        }

        public Message Joints
        {
            get { return this.joints; }
            set
            {
                if (value != null && value.Joints.Count > 0)
                {
                    this.joints = value;                    
                }
                this.checksToComplete++;
            }
        }

        public List<Message> GuideMsgs { get { return this.guideMsgs; } }
        public List<Message> ErrorMsgs { get { return this.errorMsgs; } }

        #endregion


        #region Constructors

        public Posture(string name, Skeleton skeleton, double errorThreshold, List<Message> messages, bool pMatters)
        {
            this.name = name;
            this.skeleton = skeleton;
            this.errorThreshold = errorThreshold;
            this.joints = new Message(String.Empty, new List<JointType>(), MessageType.Error);
            this.guideMsgs = new List<Message>();
            this.errorMsgs = new List<Message>();

            this.posMatters = pMatters;


            this.checksToComplete = 0;
            this.checksCompleted = new List<Message>();

            foreach (Message msg in messages)
            {
                if (msg.Type == MessageType.Error)
                    this.errorMsgs.Add(msg);
                else
                    this.guideMsgs.Add(msg);

                this.checksToComplete++;
            }

            this.numberOkChecks = 0;
            this.numberWrongChecks = 0;
        }

        #endregion


        #region Auxiliar methods

        protected void addCompletedCheck(Message msg)
        {
            if (!this.checksCompleted.Contains(msg))
            {
                this.checksCompleted.Add(msg);
                OnPropertyChanged("CompletedPercentage");
            }
        }

        protected void removeCompletedCheck(Message msg)
        {
            if (this.checksCompleted.Contains(msg))
            {
                this.checksCompleted.Remove(msg);
                OnPropertyChanged("CompletedPercentage");
            }
        }

        #endregion


        #region Transformation methods

        /// <summary>
        /// Turn a joint's position into a 3D point
        /// </summary>
        /// <param name="joint">joint to convert</param>
        /// <returns></returns>
        private Point3D get3DPointOfJoint(Joint joint)
        {
            return new Point3D
            {
                X = joint.Position.X,
                Y = joint.Position.Y,
                Z = joint.Position.Z
            };
        }
                   
        /// <summary>
        /// Scale and translate a skeleton. This trasformation depends on another skeleton
        /// </summary>
        /// <param name="originalSkeleton">skeleton to scale</param>
        /// <param name="finalSkeleton">skeleton to take into account</param>
        /// <returns></returns>
        protected Skeleton transformSkeleton(Skeleton originalSkeleton, Skeleton finalSkeleton)
        {
            Dictionary<JointType, Point3D> scaledJoints = new Dictionary<JointType, Point3D>();

            /* Scale */
            scaledJoints.Add(originToScale, get3DPointOfJoint(originalSkeleton.Joints[originToScale]));
            
            for (int i=0; i < finalJoints.Length; i++)
                scaleJoint(finalJoints[i], scaledJoints, originalSkeleton, finalSkeleton);

            /* Translation */
            if (this.posMatters)
            {
                Dictionary<JointType, Joint> auxJoints = new Dictionary<JointType, Joint>();

                foreach (KeyValuePair<JointType, Point3D> scaledJoint in scaledJoints)
                {
                    var joint = originalSkeleton.Joints[scaledJoint.Key];

                    joint.Position = new CameraSpacePoint
                    {
                        X = (float)(scaledJoint.Value.X),
                        Y = (float)(scaledJoint.Value.Y),
                        Z = (float)(scaledJoint.Value.Z)
                    };

                    auxJoints[scaledJoint.Key] = joint;
                }

                return new Skeleton(auxJoints);


                //return finalSkeleton;
            }
            else
                return translateScaledSkeleton(scaledJoints, originalSkeleton, finalSkeleton);
        }

        /// <summary>
        /// Scale a joint taking into account the distance of the bones
        /// </summary>
        /// <param name="actualJoint">joint to scale</param>
        /// <param name="scaledJoints">collection of scaled joints</param>
        /// <param name="originalSkeleton">skeleton to scale</param>
        /// <param name="finalSkeleton">skeleton to take into account</param>
        /// <returns></returns>
        private Point3D scaleJoint(JointType actualJoint, Dictionary<JointType, Point3D> scaledJoints, Skeleton originalSkeleton, Skeleton finalSkeleton)
        {
            // ActualJoint is the origin to scale or is repeated
            if (scaledJoints.ContainsKey(actualJoint))
                return get3DPointOfJoint(originalSkeleton.Joints[actualJoint]);

            JointType startJoint = Skeleton.Bones[actualJoint];

            // Wrists, ankles and neck don't exist
            switch (startJoint)
            {
                case JointType.Neck:
                    startJoint = JointType.SpineShoulder; break;
                case JointType.WristRight:
                    startJoint = JointType.ElbowRight; break;
                case JointType.WristLeft:
                    startJoint = JointType.ElbowLeft; break;
                case JointType.AnkleRight:
                    startJoint = JointType.KneeRight; break;
                case JointType.AnkleLeft:
                    startJoint = JointType.KneeLeft; break;
            }

            Point3D predecessorJoint = get3DPointOfJoint(originalSkeleton.Joints[startJoint]);
            Point3D thisJoint = get3DPointOfJoint(originalSkeleton.Joints[actualJoint]);

            Vector3D vOriginal = Point3D.Subtract(predecessorJoint, thisJoint);

            Vector3D vFinal = Point3D.Subtract(get3DPointOfJoint(finalSkeleton.Joints[startJoint]),
                get3DPointOfJoint(finalSkeleton.Joints[actualJoint]));

            double scaleFactor = vFinal.Length / vOriginal.Length;

            Point3D scaledPoint = new Point3D
            {
                X = (float)(predecessorJoint.X + scaleFactor * (thisJoint.X - predecessorJoint.X)),
                Y = (float)(predecessorJoint.Y + scaleFactor * (thisJoint.Y - predecessorJoint.Y)),
                Z = (float)(predecessorJoint.Z + scaleFactor * (thisJoint.Z - predecessorJoint.Z)),
            };

            // This vector represents the scaled distance
            Vector3D scaledDistance = Point3D.Subtract(scaledPoint, predecessorJoint);

            Point3D newPoint = Point3D.Add(scaleJoint(startJoint, scaledJoints, originalSkeleton, finalSkeleton),
                scaledDistance);

            scaledJoints.Add(actualJoint, newPoint);

            return newPoint;
        }

        /// <summary>
        /// Translate a skeleton to the position of another
        /// </summary>
        /// <param name="scaledJoints">collection of scaled joints</param>
        /// <param name="originalSkeleton">skeleton to scale</param>
        /// <param name="finalSkeleton">skeleton to take into account</param>
        /// <returns></returns>
        private Skeleton translateScaledSkeleton(Dictionary<JointType, Point3D> scaledJoints, Skeleton originalSkeleton, Skeleton finalSkeleton)
        {
            Point3D originalPoint = get3DPointOfJoint(originalSkeleton.Joints[originToTranslate]);
            Point3D finalPoint = get3DPointOfJoint(finalSkeleton.Joints[originToTranslate]);
            Vector3D translateFactor = Point3D.Subtract(originalPoint, finalPoint);

            Dictionary<JointType, Joint> translatedJoints = new Dictionary<JointType, Joint>();

            foreach (KeyValuePair<JointType, Point3D> scaledJoint in scaledJoints)
            {
                var joint = originalSkeleton.Joints[scaledJoint.Key];

                joint.Position = new CameraSpacePoint
                {
                    X = (float)(scaledJoint.Value.X - translateFactor.X),
                    Y = (float)(scaledJoint.Value.Y - translateFactor.Y),
                    Z = (float)(scaledJoint.Value.Z - translateFactor.Z)
                };

                translatedJoints[scaledJoint.Key] = joint;
            }

            return new Skeleton(translatedJoints);
        }
        
        #endregion
        

        #region Interface

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion


        #region Public methods

        public abstract List<Message> checkPosture(Skeleton skeletonToCheck);
        public void restart() { }

        /// <summary>
        /// Draw this posture in a canvas
        /// </summary>
        /// <param name="skeletonOfUser">skeleton of the user</param>
        /// <param name="drawingGroup">canvas of the posture</param>
        /// <param name="kinectSensor">kinect sensor</param>
        public void Draw(Skeleton skeletonOfUser, DrawingGroup drawingGroup, KinectSensor kinectSensor)
        {
            this.skeletonScaled = transformSkeleton(this.Skeleton, skeletonOfUser);
            //this.skeletonScaled = translateScaledSkeleton(this.skeleton.Joints, this.skeleton, skeletonOfUser);
            this.skeletonScaled.drawSkeleton(kinectSensor, drawingGroup, PostureStageType.None);
        }

        public Model3DGroup GetPostureAvatar()
        {
            ObjReader CurrentHelixObjReader = new ObjReader();
            Model3DGroup MyModel = new Model3DGroup();
            string nameCapital = this.name.ToUpper();
            if(nameCapital.Contains("POSICION NORMAL"))
            {
                
                MyModel = CurrentHelixObjReader.Read(@"3dAvatar/DePie/PosturaInicial.obj");
            }else if (nameCapital.Contains("BRAZO") && nameCapital.Contains("DERECHO") && nameCapital.Contains("LEVANTADO"))
            {
                MyModel = CurrentHelixObjReader.Read(@"3dAvatar/DePie/BrazoDerechoArriba.obj");
            }else if(nameCapital.Contains("BRAZOS") && nameCapital.Contains("LEVANTADOS"))
            {
                MyModel = CurrentHelixObjReader.Read(@"3dAvatar/DePie/AmbosBrazosArriba.obj");
            }else if (nameCapital.Contains("PIERNAS") && nameCapital.Contains("RECTAS"))
            {
                MyModel = CurrentHelixObjReader.Read(@"3dAvatar/Sentado/SentadoRectoTodo.obj");
            }else if (nameCapital.Contains("PIERNA") && nameCapital.Contains("DERECHA") && nameCapital.Contains("ARRIBA") && nameCapital.Contains("POCO"))
            {
                MyModel = CurrentHelixObjReader.Read(@"3dAvatar/Sentado/SentadoPiernaDerechaArribaPoco.obj");
            }else if (nameCapital.Contains("PIERNA") && nameCapital.Contains("DERECHA") && nameCapital.Contains("ARRIBA"))
            {
                MyModel = CurrentHelixObjReader.Read(@"3dAvatar/Sentado/SentadoPiernaDerechaArriba.obj");
            }

            //It is more complex to identify when the transition is being done
            //bool isThisATransition = this is TransitionPosture;
            //if ( isThisATransition && nameCapital.Contains("BRAZO") && nameCapital.Contains("DERECHO") && nameCapital.Contains("POCO") && nameCapital.Contains("LEVANTADO"))
            //{

            //    MyModel = CurrentHelixObjReader.Read(@"3dAvatar/brazoDerechoPocoArriba.obj");
            //}

            return MyModel;
        }

        #endregion

    }
}
