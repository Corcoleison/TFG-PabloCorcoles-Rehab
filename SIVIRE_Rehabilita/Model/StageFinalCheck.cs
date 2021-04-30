using System.Collections.Generic;

namespace SIVIRE_Rehabilita.Model
{
    class StageFinalCheck : PostureStage
    {
        public StageFinalCheck() { this.Type = PostureStageType.StageFinalCheck; }

        public override List<Message> CheckPosture(EndPosture posture, Skeleton skeletonToCheck)
        {
            return posture.checkFinal(skeletonToCheck);
        }
    }
}
