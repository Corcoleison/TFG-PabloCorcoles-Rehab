using Microsoft.Kinect;

namespace SIVIRE_Rehabilita.Gestures
{
    /// <summary>
    /// Represents a single gesture segment which uses relative positioning of body parts to detect a gesture.
    /// </summary>
    public interface IGestureSegment
    {
        /// <summary>
        /// Updates the current gesture.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns>A GestureSegmentResult based on whether the gesture part has been completed.</returns>
        GestureSegmentResult update(Body body);
    }
}
