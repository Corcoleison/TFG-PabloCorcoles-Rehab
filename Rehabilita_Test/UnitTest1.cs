using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SIVIRE_Rehabilita.Model;
using Microsoft.Kinect;

namespace Rehabilita_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [TestCategory("Skeleton")]
        public void Skeleton_Equals_a()
        {
            bool expect_result, real_result;

            Skeleton skel1 = XMLParser.loadSkeleton_Old("\\skeleton_1.xml");
            Skeleton skel2 = XMLParser.loadSkeleton_Old("\\skeleton_2.xml");

            expect_result = false;
            real_result = skel1.Equals(skel2);

            Assert.AreEqual(expect_result, real_result);
        }

        [TestMethod]
        [TestCategory("Skeleton")]
        public void Skeleton_Equals_b()
        {
            bool expect_result, real_result;

            Skeleton skel1 = XMLParser.loadSkeleton_Old("\\skeleton_1.xml");
            Skeleton skel2 = XMLParser.loadSkeleton_Old("\\skeleton_1.xml");

            expect_result = true;
            real_result = skel1.Equals(skel2);

            Assert.AreEqual(expect_result, real_result);
        }

        [TestMethod]
        [TestCategory("Message")]
        public void Message_Equals_Name()
        {
            bool expect_result, real_result;

            Message msg1 = new Message("Coloca bien el brazo derecho", null, MessageType.Error);
            Message msg2 = new Message("Coloca bien el brazo izquierdo", null, MessageType.Error);

            expect_result = false;
            real_result = msg1.Equals(msg2);

            Assert.AreEqual(expect_result, real_result);
        }

        [TestMethod]
        [TestCategory("Message")]
        public void Message_Equals_Type()
        {
            bool expect_result, real_result;

            Message msg1 = new Message("Coloca bien el brazo derecho", null, MessageType.Guide);
            Message msg2 = new Message("Coloca bien el brazo derecho", null, MessageType.Error);

            expect_result = false;
            real_result = msg1.Equals(msg2);

            Assert.AreEqual(expect_result, real_result);
        }

        [TestMethod]
        [TestCategory("Message")]
        public void Message_Equals_Joints()
        {
            bool expect_result, real_result;

            List<JointType> rightArm = new List<JointType>();
            rightArm.Add(JointType.HandRight);
            rightArm.Add(JointType.ElbowRight);
            rightArm.Add(JointType.ShoulderRight);

            Message msg1 = new Message("Coloca bien el brazo derecho", rightArm, MessageType.Error);

            List<JointType> lefttArm = new List<JointType>();
            lefttArm.Add(JointType.HandLeft);
            lefttArm.Add(JointType.ElbowLeft);
            lefttArm.Add(JointType.ShoulderLeft);

            Message msg2 = new Message("Coloca bien el brazo derecho", lefttArm, MessageType.Error);

            expect_result = false;
            real_result = msg1.Equals(msg2);

            Assert.AreEqual(expect_result, real_result);
        }

        [TestMethod]
        [TestCategory("Message")]
        public void Message_Equals_OK()
        {
            bool expect_result, real_result;

            List<JointType> rightArm = new List<JointType>();
            rightArm.Add(JointType.HandRight);
            rightArm.Add(JointType.ElbowRight);
            rightArm.Add(JointType.ShoulderRight);

            Message msg1 = new Message("Coloca bien el brazo derecho", rightArm, MessageType.Error);
            Message msg2 = new Message("Coloca bien el brazo derecho", rightArm, MessageType.Error);

            expect_result = true;
            real_result = msg1.Equals(msg2);

            Assert.AreEqual(expect_result, real_result);
        }

        [TestMethod]
        [TestCategory("XMLParser")]
        public void XMLParser_LoadExercise()
        {
            Exercise expect_exercise, real_exercise;
            List<Posture> postures = new List<Posture>();            

            List<JointType> rightArm = new List<JointType>();
            List<JointType> lefttArm = new List<JointType>();
            List<JointType> head = new List<JointType>();
            List<JointType> leftLeg = new List<JointType>();

            List<Message> messages = new List<Message>();
            rightArm.Add(JointType.HandRight);
            rightArm.Add(JointType.ElbowRight);
            rightArm.Add(JointType.ShoulderRight);
            messages.Add(new Message("Coloca bien el brazo derecho", rightArm, MessageType.Error));
            
            lefttArm.Add(JointType.HandLeft);
            lefttArm.Add(JointType.ElbowLeft);
            lefttArm.Add(JointType.ShoulderLeft);
            messages.Add(new Message("Coloca bien el brazo izquierdo", lefttArm, MessageType.Error));

            
            head.Add(JointType.Head);
            messages.Add(new Message("Coloca bien la cabeza", head, MessageType.Error));
            
            leftLeg.Add(JointType.FootLeft);
            leftLeg.Add(JointType.KneeLeft);
            leftLeg.Add(JointType.HipLeft);
            messages.Add(new Message("Coloca bien la pierna izquierda", leftLeg, MessageType.Error));  


            List<Message> messagesPosture1 = new List<Message>();
                messagesPosture1.AddRange(messages);
                messagesPosture1.Add(new Message("Baja el brazo derecho", rightArm, MessageType.Guide));
                messagesPosture1.Add(new Message("Baja el brazo izquierdo", lefttArm, MessageType.Guide));


            List<Message> messagesPosture2 = new List<Message>();
                messagesPosture2.AddRange(messages);
                messagesPosture2.Add(new Message("Levanta el brazo derecho un poco más", rightArm, MessageType.Guide));

                EndPosture posture2 = new EndPosture("Brazo derecho levantado", XMLParser.loadSkeleton_Old("\\skeleton_2.xml"), 0.1, messagesPosture2);
                    List<Message> msgsTransitions = new List<Message>();
                    msgsTransitions.Add(new Message("Levanta el brazo horizontalmente", rightArm, MessageType.Guide));
                    List<Posture> transitions = new List<Posture>();
                    transitions.Add(new TransitionPosture("Brazo derecho un poco levantado en horizontal", XMLParser.loadSkeleton_Old("\\skeleton_10.xml"), 0.1, msgsTransitions));
                posture2.Transition = transitions;


            List<Message> messagesPosture3 = new List<Message>();
                messagesPosture3.AddRange(messages);
                messagesPosture3.Add(new Message("Ahora levanta el brazo izquierdo", lefttArm, MessageType.Guide));



                postures.Add(new EndPosture("Posicion normal", XMLParser.loadSkeleton_Old("\\skeleton_1.xml"), 0.1, messagesPosture1));
            postures.Add(posture2);
            postures.Add(new EndPosture("Ambos brazos levantados", XMLParser.loadSkeleton_Old("\\skeleton_3.xml"), 0.1, messagesPosture3));

            expect_exercise = new Exercise("Ejercicio Test", 2, postures);
            real_exercise = XMLParser.loadExercise("\\ejercicioTest.xml");

            Assert.AreEqual(expect_exercise.Name, real_exercise.Name);

            for (int i = 0; i < real_exercise.Postures.Count; i++)
            {
                Assert.IsTrue(expect_exercise.Postures[i].Name.Equals(real_exercise.Postures[i].Name));
                Assert.IsTrue(expect_exercise.Postures[i].Skeleton.Equals(real_exercise.Postures[i].Skeleton));

                Assert.IsTrue(real_exercise.Postures[i].GuideMsgs.Count == expect_exercise.Postures[i].GuideMsgs.Count);
                if (real_exercise.Postures[i].GuideMsgs.Count != 0)
                    for (int j = 0; j < real_exercise.Postures[i].GuideMsgs.Count; j++)
                    {
                        Assert.IsTrue(expect_exercise.Postures[i].GuideMsgs[j].Equals(real_exercise.Postures[i].GuideMsgs[j]));
                    }

                Assert.IsTrue(real_exercise.Postures[i].ErrorMsgs.Count == expect_exercise.Postures[i].ErrorMsgs.Count);
                if (real_exercise.Postures[i].ErrorMsgs.Count != 0)
                    for (int j = 0; j < real_exercise.Postures[i].ErrorMsgs.Count; j++)
                    {
                        Assert.IsTrue(expect_exercise.Postures[i].ErrorMsgs[j].Equals(real_exercise.Postures[i].ErrorMsgs[j]));
                    }
            }
        }
    }
}
