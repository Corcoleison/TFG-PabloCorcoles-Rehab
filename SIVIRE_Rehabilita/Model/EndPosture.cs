using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SIVIRE_Rehabilita.Model
{
    public class EndPosture : Posture
    {
        #region Attributes

        /// <summary>
        /// Index of the posture that is currently checked
        /// </summary>
        int indexCurrentTransition;

        /// <summary>
        /// Posture transitions that user must complete before reaching this posture
        /// </summary>
        List<Posture> transition;

        #endregion


        #region Properties

        public List<Posture> Transition
        {
            get { return this.transition; }
            set
            {
                if (value != null && value.Count > 0)
                {
                    this.transition.AddRange(value);
                    foreach (Posture transitions in value)
                    {
                        this.checksToComplete += transitions.checksToComplete;
                    }
                    this.indexCurrentTransition = 0;
                    this.CurrentStage = PostureStageType.StageCheckTransistions;
                }
            }
        }

        #endregion


        #region Events

        /// <summary>
        /// It is raised when this posture is reached
        /// </summary>
        public event EventHandler PostureReached;

        /// <summary>
        /// It is raised when this posture is reached
        /// </summary>
        public event EventHandler PreviousPosture;

        #endregion


        public EndPosture(string name, Skeleton skeleton, double errorThreshold, List<Message> messages, bool pMatters)
            : base(name, skeleton, errorThreshold, messages, pMatters)
        {
            this.minNumberOkChecks = 10; // 10 ~= 1s 21
            this.PostureReached += postureReached;
            this.CurrentStage = PostureStageType.StageGuide;
            this.transition = new List<Posture>();
        }


        #region Public methods
        
        public override List<Message> checkPosture(Skeleton skeletonToCheck)
        {
            if (this.skeletonScaled == null)
                this.skeletonScaled = transformSkeleton(this.skeleton, skeletonToCheck);

            return this.stage.CheckPosture(this, skeletonToCheck);
        }

        public List<Message> checkTransitions(Skeleton skeletonToCheck)
        {
            List<Message> activeErrors = new List<Message>();

            // It checks if the user is trying to reach the posture instead of the transition
            foreach (Message msg in this.guideMsgs)
            {
                if (!msg.isOk(this.skeletonScaled, skeletonToCheck, this.errorThreshold))
                    activeErrors.Add(msg);
            }
            if (activeErrors.Count == 0)
            {
                this.indexCurrentTransition = 0;
                activeErrors.Clear();
                activeErrors.Add(new Message("No ha seguido las indicaciones. Vuela a repetirlo.", new List<JointType>(), MessageType.Error));
                this.PreviousPosture(this, new EventArgs());
                return activeErrors;
            }

            // Now it checks the transitions
            activeErrors = this.transition[this.indexCurrentTransition].checkPosture(skeletonToCheck);

            foreach(Message msg in this.transition[this.indexCurrentTransition].GuideMsgs)
            {
                if (!activeErrors.Contains(msg))
                    this.addCompletedCheck(msg);
                else
                    this.removeCompletedCheck(msg);
            }

            
            if (activeErrors.Count == 0)
            {
                this.indexCurrentTransition++;

                if (this.indexCurrentTransition == this.transition.Count)
                {
                    this.indexCurrentTransition = 0;
                    this.CurrentStage = PostureStageType.StageGuide;
                }
            }

            return activeErrors;
        }        

        public List<Message> checkGuideMsgs(Skeleton skeletonToCheck)
        {
            List<Message> activeErrors = new List<Message>();

            foreach (Message msg in this.guideMsgs)
            {
                if (!msg.isOk(this.skeletonScaled, skeletonToCheck, this.errorThreshold))
                {
                    this.removeCompletedCheck(msg);
                    activeErrors.Add(msg);
                }
                else
                    this.addCompletedCheck(msg);
            }

            if (activeErrors.Count == 0)  // User fits in the posture
            {
                activeErrors.Add(new Message("", new List<JointType>(), MessageType.Guide));
                this.CurrentStage = PostureStageType.StageCorrection;
            }

            return activeErrors;
        }

        public List<Message> checkErrorMsgs(Skeleton skeletonToCheck)
        {
            List<Message> activeErrors = new List<Message>();

            foreach (Message msg in this.errorMsgs)
            {
                if (!msg.isOk(this.skeletonScaled, skeletonToCheck, this.errorThreshold))
                {
                    this.removeCompletedCheck(msg);
                    activeErrors.Add(msg);
                }
                else
                    this.addCompletedCheck(msg);
            }

            // It checks the joints as well
            if (!this.joints.isOk(this.skeletonScaled, skeletonToCheck, this.errorThreshold))
            {
                this.removeCompletedCheck(this.joints);
                activeErrors.Add(this.joints);
            }
            else
                this.addCompletedCheck(this.joints);


            if (activeErrors.Count == 0 && this.CompletedPercentage == 100)  // User fits in the posture
            {
                this.CurrentStage = PostureStageType.StageFinalCheck;
            }

            return activeErrors;
        }

        public List<Message> checkFinal(Skeleton skeletonToCheck)
        {
            List<Message> activeErrors = this.checkErrorMsgs(skeletonToCheck);

            if (activeErrors.Count == 0)  // User fits in the posture
            {
                if (this.numberOkChecks == this.minNumberOkChecks)   // User fits in the posture for a while
                {
                    this.PostureReached(this, new EventArgs());
                }
                else
                {
                    this.numberOkChecks++;
                    OnPropertyChanged("FinalCheckPercentage");                    
                }                
            }
            else
            {
                this.numberWrongChecks++;

                if (numberWrongChecks >= (this.minNumberOkChecks / 4))   // User has changed the posture
                {
                    this.numberWrongChecks = 0;
                    this.numberOkChecks = 0;
                    OnPropertyChanged("FinalCheckPercentage");
                    this.CurrentStage = PostureStageType.StageCorrection;
                }
            }

            return new List<Message>();
        }

        private void postureReached(object sender, EventArgs e)
        {
            this.restart();
        }

        public void restart()
        {
            this.checksCompleted.Clear();
            this.numberOkChecks = 0;
            this.numberWrongChecks = 0;
            if (this.transition.Count > 0)
                this.CurrentStage = PostureStageType.StageCheckTransistions;
            else
                this.CurrentStage = PostureStageType.StageGuide;
        }

        #endregion

    }
}
