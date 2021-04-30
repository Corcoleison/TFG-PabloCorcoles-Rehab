using System.Collections.Generic;

namespace SIVIRE_Rehabilita.Model
{
    class StageGuide : PostureStage
    {
        public StageGuide() { this.Type = PostureStageType.StageGuide; }

        public override List<Message> CheckPosture(EndPosture posture, Skeleton skeletonToCheck)
        {
            return posture.checkGuideMsgs(skeletonToCheck);
        }
    }
}
