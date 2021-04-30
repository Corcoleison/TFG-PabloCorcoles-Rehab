using System.Collections.Generic;

namespace SIVIRE_Rehabilita.Model
{
    class StageCorrection : PostureStage
    {
        public StageCorrection() { this.Type = PostureStageType.StageCorrection; }

        public override List<Message> CheckPosture(EndPosture posture, Skeleton skeletonToCheck)
        {
            return posture.checkErrorMsgs(skeletonToCheck);
        }
    }
}
