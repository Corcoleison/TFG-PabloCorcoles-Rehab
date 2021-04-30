using Microsoft.Kinect;
using System;

namespace SIVIRE_Rehabilita.Gestures
{
    public abstract class Gesture
    {
        protected int windowSize = 50;  // Default value is 50 frames
        protected IGestureSegment[] segments;

        int currentSegment = 0;
        int frameCount = 0;

        public event EventHandler gestureRecognized;

        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="body">The skeleton data.</param>
        public void update(Body body)
        {
            GestureSegmentResult result = segments[currentSegment].update(body);

            if (result == GestureSegmentResult.Succeeded)
            {
                if (currentSegment + 1 < segments.Length)
                {
                    currentSegment++;
                    frameCount = 0;
                }
                else
                {
                    if (gestureRecognized != null)
                    {
                        gestureRecognized(this, new EventArgs());
                        Reset();
                    }
                }
            }
            else if (result == GestureSegmentResult.Failed && frameCount == windowSize)
            {
                Reset();
            }
            else
            {
                frameCount++;
            }
        }

        /// <summary>
        /// Resets the current gesture.
        /// </summary>
        void Reset()
        {
            currentSegment = 0;
            frameCount = 0;
        }
    }
}
