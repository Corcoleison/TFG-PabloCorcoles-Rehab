using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIVIRE_Rehabilita.Model
{
    public class TransitionPosture : Posture
    {
        /// <summary>
        /// It is raised when this transition posture is completed
        /// </summary>
        public event EventHandler TransitionCompleted;

        public TransitionPosture(string name, Skeleton skeleton, double errorThreshold, List<Message> messages, bool pMatters)
            : base(name, skeleton, errorThreshold, messages, pMatters)
        {
            //this.minNumberOkChecks = 0; // sólo se necesita que pase por la postura de transicción no que la mantenga
        }

        public override List<Message> checkPosture(Skeleton skeletonToCheck)
        {
            // Hacemos la transformación proque la transición no se dibuja de momento
            if (this.skeletonScaled == null)
                this.skeletonScaled = transformSkeleton(this.skeleton, skeletonToCheck);

            List<Message> activeErrors = new List<Message>();

            foreach (Message msg in this.guideMsgs)
            {
                if (!msg.isOk(this.skeletonScaled, skeletonToCheck, this.errorThreshold))
                    activeErrors.Add(msg);
            }

            /*if (activeErrors.Count == 0)  // User fits in the posture
            {
                this.TransitionCompleted(this, new EventArgs());
            }*/

            this.skeletonScaled = null;

            return activeErrors;
        }
    }
}
