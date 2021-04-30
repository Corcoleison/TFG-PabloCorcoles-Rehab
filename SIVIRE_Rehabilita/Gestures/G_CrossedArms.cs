using Microsoft.Kinect;

namespace SIVIRE_Rehabilita.Gestures
{
    public class G_CrossedArms : Gesture
    {
        public G_CrossedArms()
        {
            segments = new IGestureSegment[]
            {
                new StartSegment(),
                new CrossSegment(),
                new FinalSegment()
            };
        }

        /// <summary>
        /// Both hands in their position with Lasso state
        /// </summary>
        private class StartSegment : IGestureSegment
        {
            public GestureSegmentResult update(Body body)
            {
                if (body.HandRightState == HandState.Lasso && body.HandLeftState == HandState.Lasso)
                {
                    if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y &&
                        body.Joints[JointType.HandLeft].Position.Y > body.Joints[JointType.ElbowLeft].Position.Y &&
                        body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.HandLeft].Position.X)
                    {
                        return GestureSegmentResult.Succeeded;
                    }
                }

                return GestureSegmentResult.Failed;
            }
        }

        /// <summary>
        /// Right hand on the left of left hand
        /// </summary>
        private class CrossSegment : IGestureSegment
        {
            public GestureSegmentResult update(Body body)
            {
                if (body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.HandLeft].Position.X)
                {
                    return GestureSegmentResult.Succeeded;
                }

                return GestureSegmentResult.Failed;
            }
        }

        /// <summary>
        /// Left hand on the left of right hand
        /// </summary>
        private class FinalSegment : IGestureSegment
        {
            public GestureSegmentResult update(Body body)
            {
                if (body.HandRightState == HandState.Lasso && body.HandLeftState == HandState.Lasso)
                {
                    if (body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.HandLeft].Position.X)
                    {
                        return GestureSegmentResult.Succeeded;
                    }
                }

                return GestureSegmentResult.Failed;
            }
        }
    }
}
