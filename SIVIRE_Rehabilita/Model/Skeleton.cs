using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SIVIRE_Rehabilita.Model
{
    public class Skeleton
    {
        #region Constants

        /// <summary>
        /// Width of output drawing
        /// </summary>
        const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joints
        /// </summary>
        const double JointThickness = 15;

        /// <summary>
        /// Thickness of drawn bones lines
        /// </summary>
        const double BoneThickness = 15;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        const double BodyCenterThickness = 10;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        const float InferredZPositionClamp = 0.1f;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Brush used for drawing joints that don't fit in a posture
        /// </summary>        
        readonly Brush wrongJointBrush = Brushes.Red;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        readonly Pen trackedBonePen = new Pen(Brushes.Green, BoneThickness);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        readonly Pen finalCheckBonePen = new Pen(Brushes.Navy, 30);

        /// <summary>
        /// Pen used for drawing bones that don't fit in a posture
        /// </summary>
        readonly Pen wrongBonePen = new Pen(Brushes.Red, BoneThickness);

        #endregion


        #region Atrributes

        

        /// <summary>
        /// definition of bones
        /// </summary>
        static IReadOnlyDictionary<JointType, JointType> bones;
        
        /// <summary>
        /// body of the skeleton
        /// </summary>
        Body body;

        /// <summary>
        /// 3D points of the joints
        /// </summary>
        IReadOnlyDictionary<JointType, Joint> joints;

        List<Message> activeErrors = new List<Message>();

        #endregion


        #region Properties

        public static IReadOnlyDictionary<JointType, JointType> Bones
        {
            get
            {
                if (bones == null)
                {
                    // a bone defined as a line between two joints
                    Dictionary<JointType, JointType> bonesAux = new Dictionary<JointType, JointType>();

                    // Torso
                    //bonesAux.Add(JointType.Head, JointType.Neck);
                    //bonesAux.Add(JointType.Neck, JointType.SpineShoulder);
                    bonesAux.Add(JointType.Head, JointType.SpineShoulder);
                    bonesAux.Add(JointType.SpineShoulder, JointType.SpineMid);
                    bonesAux.Add(JointType.SpineMid, JointType.SpineBase);
                    bonesAux.Add(JointType.ShoulderRight, JointType.SpineShoulder);
                    bonesAux.Add(JointType.ShoulderLeft, JointType.SpineShoulder);
                    bonesAux.Add(JointType.HipRight, JointType.SpineBase);
                    bonesAux.Add(JointType.HipLeft, JointType.SpineBase);

                    // Right Arm
                    bonesAux.Add(JointType.ElbowRight, JointType.ShoulderRight);
                    bonesAux.Add(JointType.HandRight, JointType.ElbowRight);
                    //bonesAux.Add(JointType.WristRight, JointType.ElbowRight);
                    //bonesAux.Add(JointType.HandRight, JointType.WristRight);
                    //bonesAux.Add(JointType.HandTipRight, JointType.HandRight);
                    //bonesAux.Add(JointType.ThumbRight, JointType.WristRight);

                    // Left Arm
                    bonesAux.Add(JointType.ElbowLeft, JointType.ShoulderLeft);
                    bonesAux.Add(JointType.HandLeft, JointType.ElbowLeft);
                    //bonesAux.Add(JointType.WristLeft, JointType.ElbowLeft);
                    //bonesAux.Add(JointType.HandLeft, JointType.WristLeft);
                    //bones.Add(JointType.HandTipLeft, JointType.HandLeft);
                    //bones.Add(JointType.ThumbLeft, JointType.WristLeft);

                    // Right Leg
                    bonesAux.Add(JointType.KneeRight, JointType.HipRight);
                    bonesAux.Add(JointType.FootRight, JointType.KneeRight);
                    //bonesAux.Add(JointType.AnkleRight, JointType.KneeRight);
                    //bonesAux.Add(JointType.FootRight, JointType.AnkleRight);

                    // Left Leg
                    bonesAux.Add(JointType.KneeLeft, JointType.HipLeft);
                    bonesAux.Add(JointType.FootLeft, JointType.KneeLeft);
                    //bonesAux.Add(JointType.AnkleLeft, JointType.KneeLeft);
                    //bonesAux.Add(JointType.FootLeft, JointType.AnkleLeft);

                    bones = bonesAux;
                }

                return bones;
            }
        }

        public IReadOnlyDictionary<JointType, Joint> Joints
        {
            get
            {
                if (this.body != null)
                    return this.body.Joints;
                else
                    return this.joints;
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Create a skeleton from a body captured by kinect
        /// </summary>
        /// <param name="body"></param>
        public Skeleton(Body body)
        {
            this.body = body;
        }

        /// <summary>
        /// Create a skeleton from a collection of joints
        /// </summary>
        /// <param name="bodyJoints"></param>
        public Skeleton(IReadOnlyDictionary<JointType, Joint> bodyJoints)
        {
            this.joints = bodyJoints;
        }

        #endregion


        #region Public methods

        public override bool Equals(object obj)
        {
            IReadOnlyDictionary<JointType, Joint> auxJoints;
            Skeleton skeleton2 = (Skeleton)obj;

            if (this.body != null)
                auxJoints = this.body.Joints;
            else
                auxJoints = this.joints;

            foreach (JointType type in Enum.GetValues(typeof(JointType)))
            {
                bool equals = auxJoints[type].Position.X == skeleton2.Joints[type].Position.X;
                equals = auxJoints[type].Position.Y == skeleton2.Joints[type].Position.Y;
                equals = auxJoints[type].Position.Z == skeleton2.Joints[type].Position.Z;
                if (!equals)
                    return false;
            }

            return true;
        }
        public override int GetHashCode()
        {
            if (this.body != null)
                return this.body.Joints.GetHashCode();
            else
                return this.joints.GetHashCode();
        }

        /// <summary>
        /// Check if this skeleton fits in a posture and draw it
        /// </summary>
        /// <param name="posture">posture to check</param>
        /// <param name="kinectSensor">current kinect sensor</param>
        /// <param name="drawingGroup">canvas to draw the skeleton</param>
        /// <returns></returns>
        public List<Message> checkAndDrawSkeleton(Posture posture, KinectSensor kinectSensor, DrawingGroup drawingGroup)
        {
            this.activeErrors = posture.checkPosture(this);
            this.drawSkeleton(kinectSensor, drawingGroup, posture.CurrentStage);

            return this.activeErrors;
        }

        /*//Draw profile skeleton
        int width; int max;

        public void DrawProfileSkeleton(KinectSensor kinectSensor, DrawingGroup drawingGroup, Pen drawingPen)
        {
            width = kinectSensor.ColorFrameSource.FrameDescription.Width;
            max = kinectSensor.ColorFrameSource.DepthMaxReliableDistance;

            this.DrawSkeleton(kinectSensor, drawingGroup, drawingPen);
        }*/
        
        /// <summary>
        /// Draw a skeleton in a canvas
        /// </summary>
        /// <param name="kinectSensor">current kinect sensor</param>
        /// <param name="drawingGroup">canvas to draw the skeleton</param>
        public void drawSkeleton(KinectSensor kinectSensor, DrawingGroup drawingGroup, PostureStageType stage)
        {
            // convert the joint points to depth (display) space
            Dictionary<JointType, Point> colorPoints = this.convertJointPoints(this.Joints, kinectSensor.CoordinateMapper);

            using (DrawingContext dc = drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Transparent, null, new Rect(0.0, 0.0,
                    kinectSensor.ColorFrameSource.FrameDescription.Width, kinectSensor.ColorFrameSource.FrameDescription.Height));

                // Draw the bones
                foreach (var bone in Bones)
                {
                    this.drawBone(this.Joints, colorPoints, bone.Key, bone.Value, dc, stage);
                }

                // Draw the joints
                foreach (JointType joint in Bones.Keys)
                {
                    this.drawJoint(joint, colorPoints, dc, stage);
                }
                this.drawJoint(JointType.SpineBase, colorPoints, dc, stage);

                //this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                //this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
            }

            // prevent drawing outside of our render area
            drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0,
                        kinectSensor.ColorFrameSource.FrameDescription.Width, kinectSensor.ColorFrameSource.FrameDescription.Height));
        }

        #endregion


        #region Drawing methods

        /// <summary>
        /// Turn 3D points of the joints into 2D points of depth frame
        /// </summary>
        /// <param name="joints">collection of joints to convert</param>
        /// <param name="coordinateMapper">allows the conversion</param>
        /// <returns></returns>
        private Dictionary<JointType, Point> convertJointPoints(IReadOnlyDictionary<JointType, Joint> joints, CoordinateMapper coordinateMapper)
        {
            Dictionary<JointType, Point> colorPoints = new Dictionary<JointType, Point>();

            foreach (JointType jointType in joints.Keys)
            {
                // sometimes the depth(Z) of an inferred joint may show as negative
                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                CameraSpacePoint position = joints[jointType].Position;

                if (position.Z < 0)
                {
                    position.Z = InferredZPositionClamp;
                }

                ColorSpacePoint depthSpacePoint = coordinateMapper.MapCameraPointToColorSpace(position);
                // Draw profile skeleton
                //depthSpacePoint.X = ((position.Z * 1000) * width) / max;
                colorPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
            }

            return colorPoints;
        }

        /// <summary>
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void drawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, 
            JointType jointType0, JointType jointType1, DrawingContext drawingContext, PostureStageType stage)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];
            Pen drawPen = null;

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked || joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }            

            if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            {
                switch (stage)
                {
                    case PostureStageType.None:
                        //drawPen = new Pen(new SolidColorBrush(Color.FromArgb(100, 0, 255, 255)), 25);
                        drawPen = new Pen(new SolidColorBrush(Color.FromArgb(150, 50, 255, 50)), 25);
                        break;
                    case PostureStageType.StageGuide:
                        if (isWrongBone(joint0.JointType, joint1.JointType))
                        {
                            //drawPen = finalCheckBonePen;
                            drawPen = new Pen(new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)), 25);
                        }
                        else
                        {
                            drawPen = new Pen(new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)), 25);
                        }
                        break;
                    case PostureStageType.StageCorrection:
                        if (isWrongBone(joint0.JointType, joint1.JointType))
                        {
                            drawPen = new Pen(new SolidColorBrush(Color.FromArgb(150, 255, 65, 65)), 30);
                        }
                        break;
                    case PostureStageType.StageFinalCheck:
                        drawPen = finalCheckBonePen;
                        break;
                    default:
                        drawPen = new Pen(new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)), 25);
                        break;
                }
            }

            if (drawPen != null)
            {
                drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
            }
        }
        
        private void drawJoint(JointType joint, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, PostureStageType stage)
        {
            Brush backgroundBrush = null;
            Pen borderPen = new Pen(Brushes.White, 5);
            double thickness = JointThickness;

            if (this.Joints[joint].TrackingState == TrackingState.Tracked)
            {
                switch (stage)
                {
                    case PostureStageType.None:
                        backgroundBrush = Brushes.Green;
                        borderPen = new Pen(Brushes.LightGreen, 5);
                        break;
                    case PostureStageType.StageCheckTransistions:
                        backgroundBrush = Brushes.Silver;
                        //backgroundBrush = new SolidColorBrush(Color.FromArgb(190, 145, 145, 145));
                        borderPen = new Pen(Brushes.White, 5);
                        break;
                    case PostureStageType.StageGuide:
                        backgroundBrush = Brushes.Silver;
                        break;
                    case PostureStageType.StageCorrection:
                        if (isWrongJoint(joint))
                        {
                            backgroundBrush = Brushes.Red;
                            borderPen = new Pen(Brushes.LightCoral, 5);
                            thickness += 5;
                        }
                        //else
                            //backgroundBrush = Brushes.LimeGreen;
                        break;
                    case PostureStageType.StageFinalCheck:
                        backgroundBrush = Brushes.Navy;
                        thickness += 5;
                        break;
                    default:
                        break;
                }
            }
            /*else if (trackingState == TrackingState.Inferred)
            {
                backgroundBrush = this.inferredJointBrush;
            }*/

            if (backgroundBrush != null)
            {
                drawingContext.DrawEllipse(backgroundBrush, borderPen, jointPoints[joint], thickness, thickness);
            }
        }

        /// <summary>
        /// Check if a joint did not pass the verification
        /// </summary>
        /// <param name="joint">joint to check</param>
        /// <returns></returns>
        private bool isWrongJoint(JointType joint)
        {
            if (this.activeErrors.Count == 0)
                return false;

            foreach (Message msg in this.activeErrors)
            {
                if (msg.Joints.Contains(joint))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if a bone did not pass the verification
        /// </summary>
        /// <param name="joint0">first joint of the bone</param>
        /// <param name="joint1">second joint of the bone</param>
        /// <returns></returns>
        private bool isWrongBone(JointType joint0, JointType joint1)
        {
            if (this.activeErrors.Count == 0)
                return false;

            foreach (Message msg in this.activeErrors)
            {
                if (msg.Joints.Contains(joint0) && msg.Joints.Contains(joint1))
                    return true;
            }

            return false;
        }

        #endregion

    }
}
