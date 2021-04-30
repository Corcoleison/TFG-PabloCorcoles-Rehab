using System;
using System.Collections.Generic;

namespace SIVIRE_Rehabilita.Model
{ 
    public abstract class PostureStage
    {
        public PostureStageType Type;

        /// <summary>
        /// Factory Method
        /// </summary>
        public static PostureStage getStage(PostureStageType type)
        {
            switch (type)
            {
                case PostureStageType.StageCheckTransistions:
                    return new StageCheckTransitions();
                case PostureStageType.StageGuide:
                    return new StageGuide();
                case PostureStageType.StageCorrection:
                    return new StageCorrection();
                case PostureStageType.StageFinalCheck:
                    return new StageFinalCheck();
                default:
                    throw new ArgumentException("Stage does not exist");
            }
        }

        public abstract List<Message> CheckPosture(EndPosture posture, Skeleton skeletonToCheck);
    }
}
