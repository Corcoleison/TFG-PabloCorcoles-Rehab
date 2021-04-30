using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SIVIRE_Rehabilita.Model
{
    public class Message
    {
        public string Msg { get; protected set; }

        List<JointType> joints;
        public List<JointType> Joints { get { return this.joints; } }

        MessageType type;
        public MessageType Type { get { return this.type; } }

        public Message(string msg, List<JointType> joints, MessageType type)
        {
            this.Msg = msg;
            this.joints = new List<JointType>();
            this.type = type;

            if (joints != null)
            {
                this.joints.AddRange(joints);

                // Some joints does not take in account
                if (joints.Contains(JointType.ElbowRight) && joints.Contains(JointType.HandRight))
                    this.joints.Add(JointType.WristRight);
                if (joints.Contains(JointType.ElbowLeft) && joints.Contains(JointType.HandLeft))
                    this.joints.Add(JointType.WristLeft);
                if (joints.Contains(JointType.KneeRight) && joints.Contains(JointType.FootRight))
                    this.joints.Add(JointType.AnkleRight);
                if (joints.Contains(JointType.KneeLeft) && joints.Contains(JointType.FootLeft))
                    this.joints.Add(JointType.AnkleLeft);
                if (joints.Contains(JointType.Head) && joints.Contains(JointType.SpineShoulder))
                    this.joints.Add(JointType.Neck);
            }
        }

        public bool isOk(Skeleton postureSkeleton, Skeleton skeletonToCheck, double errorThreshold)
        {
            foreach (JointType joint in this.joints)
            {
                if (joint != JointType.WristRight && joint != JointType.WristLeft &&
                    joint != JointType.AnkleRight && joint != JointType.AnkleLeft && joint != JointType.Neck)
                {
                    if (!checkJoint(skeletonToCheck.Joints[joint], postureSkeleton, errorThreshold))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Check if a joint matchs with the same joint of the posture
        /// </summary>
        /// <param name="jointToCheck">joint to be checked</param>
        /// <returns></returns>
        private bool checkJoint(Joint jointToCheck, Skeleton skeletonPosture, double errorThreshold)
        {
            Point3D center = get3DPointOfJoint(skeletonPosture.Joints[jointToCheck.JointType]);
            Point3D point = get3DPointOfJoint(jointToCheck);

            // Check a point is inside of a sphere
            double r2 = Math.Pow(errorThreshold, 2);
            double result = Point3D.Subtract(point, center).LengthSquared;

            return result <= r2;
        }

        private Point3D get3DPointOfJoint(Joint joint)
        {
            return new Point3D
            {
                X = joint.Position.X,
                Y = joint.Position.Y,
                Z = joint.Position.Z
            };
        }

        public override bool Equals(object obj)
        {
            Message msg2 = (Message)obj;

            if (!this.Msg.Equals(msg2.Msg))
                return false;

            if (!this.type.Equals(msg2.type))
                return false;

            foreach (JointType joint in this.joints)
            {
                if (!msg2.joints.Contains(joint))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
