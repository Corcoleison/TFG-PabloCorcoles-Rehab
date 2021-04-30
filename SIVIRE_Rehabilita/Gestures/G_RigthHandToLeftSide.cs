using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIVIRE_Rehabilita.Gestures
{
    public class G_RigthHandToLeftSide : Gesture
    {
        public G_RigthHandToLeftSide()
        {
            segments = new IGestureSegment[]
            {
                new StartGesture(),
                new FinalSegment()
            };
        }

        /// <summary>
        /// Right hand in its position with Lasso state
        /// </summary>
        private class StartGesture : IGestureSegment
        {
            public GestureSegmentResult update(Body body)
            {
                if (body.HandRightState == HandState.Lasso && body.HandLeftState != HandState.Lasso)
                {
                    if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y &&
                        body.Joints[JointType.HandRight].Position.X > body.Joints[JointType.ShoulderRight].Position.X)
                    {
                        return GestureSegmentResult.Succeeded;
                    }
                }

                return GestureSegmentResult.Failed;
            }
        }

        /// <summary>
        /// Right hand on the left side its position with Lasso state
        /// </summary>
        private class FinalSegment : IGestureSegment
        {
            public GestureSegmentResult update(Body body)
            {
                if (body.HandRightState == HandState.Lasso)
                {
                    if (body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ElbowRight].Position.Y &&
                        body.Joints[JointType.HandRight].Position.X < body.Joints[JointType.SpineShoulder].Position.X)
                    {
                        return GestureSegmentResult.Succeeded;
                    }
                }

                return GestureSegmentResult.Failed;
            }
        }
    }
}
