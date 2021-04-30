using System.Collections.Generic;

namespace SIVIRE_Rehabilita.Model
{
    class StageCheckTransitions : PostureStage
    {
        public StageCheckTransitions() { this.Type = PostureStageType.StageCheckTransistions; }

        public override List<Message> CheckPosture(EndPosture posture, Skeleton skeletonToCheck)
        {
            return posture.checkTransitions(skeletonToCheck);
        }
    }
}
