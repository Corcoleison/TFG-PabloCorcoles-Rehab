namespace SIVIRE_Rehabilita.Gestures
{
    public class GestureFactory
    {
        public static Gesture getGesture(GestureType gesture)
        {
            switch (gesture)
            {
                case GestureType.CrossedArms:
                    return new G_CrossedArms();
                case GestureType.RigthHandToLeftSide:
                    return new G_RigthHandToLeftSide();
                default:
                    return null;
            }
        }
    }
}
