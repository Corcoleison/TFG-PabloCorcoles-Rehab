using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SIVIRE_Rehabilita.Model
{
    public class XMLParser
    {
        private static Dictionary<string, JointType> jointTypes;
        
        private static int numSkeletons = 9;

        /// <summary>
        /// Initialize the dictionary with all the joint types
        /// </summary>
        private static void initializeDictionary()
        {
            jointTypes = new Dictionary<string, JointType>();

            // Get all joint types from the enumeration
            foreach (JointType type in Enum.GetValues(typeof(JointType)))
            {
                jointTypes.Add(type.ToString(), type);
            }
        }

        #region Parse_AppPreferences

        public static void loadAppPreferences(App app)
        {
            // Default preferences
            app.Gestures_IsEnabled = true;
            app.Sound_IsEnabled = true;
            app.ReadMsg_IsEnabled = true;
            app.ReadErrorMsg_IsEnabled = true;

            try
            {
                XDocument doc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "\\AppPreferences.xml");

                foreach (XElement user in doc.Element("Users").Elements("User"))
                {
                    if (user.Element("NickName").Value.Equals(app.CurrentUser.NickName))
                    {
                        XElement preferences = user.Element("Preferences");
                        app.Gestures_IsEnabled = bool.Parse(preferences.Element("GesturesEnabled").Value);
                        app.Sound_IsEnabled = bool.Parse(preferences.Element("SoundEnabled").Value);
                        app.ReadMsg_IsEnabled = bool.Parse(preferences.Element("ReadMsgEnabled").Value);
                    }
                }
            }
            catch (FileNotFoundException)  // If the file does not exist, this creates it
            {
                using (XmlTextWriter Writer = new XmlTextWriter(AppDomain.CurrentDomain.BaseDirectory + "\\AppPreferences.xml", Encoding.UTF8))
                {
                    Writer.WriteStartDocument();
                    Writer.Formatting = Formatting.Indented;
                    Writer.Indentation = 5;

                    Writer.WriteStartElement("Users");
                    Writer.WriteStartElement("User");

                    Writer.WriteElementString("NickName", app.CurrentUser.NickName);
                    Writer.WriteElementString("Password", app.CurrentUser.Password);
                    Writer.WriteStartElement("Preferences");
                    Writer.WriteElementString("GesturesEnabled", app.Gestures_IsEnabled.ToString());
                    Writer.WriteElementString("SoundEnabled", app.Sound_IsEnabled.ToString());
                    Writer.WriteElementString("ReadMsgEnabled", app.ReadMsg_IsEnabled.ToString());

                    Writer.WriteEndElement();
                    Writer.WriteEndDocument();
                    Writer.Flush();
                }
            }
        }

        public static void saveAppPreferences(App app)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\AppPreferences.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlElement rootNode = doc.DocumentElement;
            XmlNodeList users = doc.SelectNodes("Users/User");
            XmlNode newUser = createUserNode(app, doc);

            foreach (XmlNode item in users)
            {                
                if (item.FirstChild.InnerText == app.CurrentUser.NickName)
                {
                    XmlNode oldNode = item;
                    rootNode.ReplaceChild(newUser, oldNode);

                    //Delete a node.
                    //rootNode.RemoveChild(oldNode);

                    doc.Save(path);
                    return;
                }
            }            

            // New user to save
            rootNode.InsertAfter(newUser, rootNode.LastChild);
            doc.Save(path);
        }

        private static XmlNode createUserNode(App app, XmlDocument doc)
        {
            XmlElement newUser = doc.CreateElement("User");

            XmlElement nickName = doc.CreateElement("NickName");
            nickName.InnerText = app.CurrentUser.NickName;
            newUser.AppendChild(nickName);

            XmlElement password = doc.CreateElement("Password");
            password.InnerText = app.CurrentUser.Password;
            newUser.AppendChild(password);

            XmlElement preferences = doc.CreateElement("Preferences");
            newUser.AppendChild(preferences);

            XmlElement gesturesEnabled = doc.CreateElement("GesturesEnabled");
            gesturesEnabled.InnerText = app.Gestures_IsEnabled.ToString();
            preferences.AppendChild(gesturesEnabled);
            XmlElement soundEnabled = doc.CreateElement("SoundEnabled");
            soundEnabled.InnerText = app.Sound_IsEnabled.ToString();
            preferences.AppendChild(soundEnabled);
            XmlElement readMsgEnabled = doc.CreateElement("ReadMsgEnabled");
            readMsgEnabled.InnerText = app.ReadMsg_IsEnabled.ToString();
            preferences.AppendChild(readMsgEnabled);

            return newUser;
        }

        #endregion


        #region Parse_Exercise

        /// <summary>
        /// Parse the XML file
        /// </summary>
        /// <param name="path">path of the file</param>
        public static Skeleton loadSkeleton_Old(string path)
        {
            //loadExercise(path);
            path = AppDomain.CurrentDomain.BaseDirectory + path;
            XmlTextReader xmlReader = new XmlTextReader(path);
            Dictionary<JointType, Joint> joints = new Dictionary<JointType, Joint>();

            if (jointTypes == null)
                initializeDictionary();

            xmlReader.ReadToFollowing("Coordenadas");
            xmlReader.Read();

            while (xmlReader.Read() && !xmlReader.Name.Equals("Coordenadas"))
            {
                CameraSpacePoint position = new CameraSpacePoint();

                string nameJoint = xmlReader.Name;

                xmlReader.ReadToFollowing("X");
                xmlReader.Read();
                position.X = float.Parse(xmlReader.Value);

                xmlReader.ReadToFollowing("Y");
                xmlReader.Read();
                position.Y = float.Parse(xmlReader.Value);

                xmlReader.ReadToFollowing("Z");
                xmlReader.Read();
                position.Z = float.Parse(xmlReader.Value);

                // Add the new joint into skeleton
                joints[jointTypes[nameJoint]] = new Joint
                {
                    JointType = jointTypes[nameJoint],
                    Position = position,
                    TrackingState = TrackingState.Tracked
                };

                xmlReader.Read();
                xmlReader.Read();
                xmlReader.Read();
                xmlReader.Read();
            }

            return new Skeleton(joints);
        }

        /// <summary>
        /// Parse the XML file
        /// </summary>
        /// <param name="path">path of the file</param>
        public static Exercise loadExercise(string path)
        {
            if (jointTypes == null)
                initializeDictionary();

            XDocument doc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + path);

            var readExercise = doc.Element("Ejercicio");

            

            // Propiedades de Ejercicio
            XElement properties = readExercise.Element("Propiedades");
            var name = properties.Element("Nombre").Value;
            var numRepetitions = int.Parse(properties.Element("NumeroRepeticiones").Value);


            var positionMatters = bool.Parse(properties.Element("ImportaPosicion").Value); //¿Depende el ejercicio de la posición?


            XElement supportTools = properties.Element("HerramientasApoyo");
            var equilibriumScale = bool.Parse(supportTools.Element("BalanzaEquilibrio").Value);
            var guideVoice = bool.Parse(supportTools.Element("VozGuia").Value);
            //var marcas = properties.Element("Marcas");

            //var joints = loadJoints(properties.Element("Articulaciones"));

            // Se comprueban las articulaciones que se van a monitorizar
            //foreach (Posture posture in postures)
            //{
            //    foreach (JointType joint in joints)
            //    {
            //        if (!posture.Joints.Contains(joint))
            //            posture.Joints.Add(joint);
            //    }                
            //}

            var postures = new List<Posture>();
            var readPostures = readExercise.Element("Posturas").Descendants("Postura");
            foreach (XElement posture in readPostures)
            {
                postures.Add(loadPosture(posture, positionMatters));
            }

            return new Exercise(name, numRepetitions, postures, positionMatters);
        }

        public static Posture loadPosture(XElement readPosture, bool pMatters)
        {
            var skeleton = loadSkeleton(readPosture.Element("Esqueleto"));
            var transitions = loadTransitions(readPosture.Element("PosturasTransicion"), pMatters);            

            // Propiedades de Postura
            XElement properties = readPosture.Element("Propiedades");
            var name = properties.Element("Nombre").Value;

            // Hay que transformarla en errorThreshold
            var difficulty = int.Parse(properties.Element("Dificultad").Value);
            double errorThreshold;
            switch (difficulty)
            {
                case 10:
                    errorThreshold = 0.05;
                    break;
                case 5:
                    errorThreshold = 0.1;
                    break;
                case 1:
                    errorThreshold = 0.2;
                    break;
                default:
                    errorThreshold = 0.1;
                    break;
            }

            //var positionMatters = bool.Parse(properties.Element("ImportaPosicion").Value); //En principio, suponemos que es el ejercicio al completo independiente o dependiente de la posición
            var joints = loadJoints(properties.Element("Articulaciones"));
            var view = loadJoints(properties.Element("VistaPostura"));

            var messages = loadMessages(properties.Element("MensajesGuia"));
            messages.AddRange(loadMessages(properties.Element("MensajesError")));

            var posture = new EndPosture(name, skeleton, errorThreshold, messages, pMatters)
            {
                Transition = transitions,
                Joints = new Message(String.Empty, joints, MessageType.Error)
            };

            return posture;
        }

        public static Skeleton loadSkeleton(XElement readSkeleton)
        {
            var joints = new Dictionary<JointType, Joint>();

            foreach (XElement joint in readSkeleton.Elements())
            {
                var nameJoint = joint.Name.LocalName;
                var position = new CameraSpacePoint
                {
                    X = float.Parse(joint.Element("X").Value),
                    Y = float.Parse(joint.Element("Y").Value),
                    Z = float.Parse(joint.Element("Z").Value)
                };

                joints[jointTypes[nameJoint]] = new Joint
                {
                    JointType = jointTypes[nameJoint],
                    Position = position,
                    TrackingState = TrackingState.Tracked
                };
            }

            return new Skeleton(joints);
        }

        public static List<Posture> loadTransitions(XElement readTransitions, bool pMatters)
        {
            List<Posture> transitions = new List<Posture>();

            foreach (XElement transition in readTransitions.Elements("PosturaTransicion"))
            {
                var name = transition.Element("Nombre").Value;
                // Hay que transformarla en errorThreshold
                var difficulty = double.Parse(transition.Element("Dificultad").Value);
                difficulty = 0.1;
                var skeleton = loadSkeleton(transition.Element("Esqueleto"));
                List<Message> messages = loadMessages(transition.Element("MensajesGuia"));
                messages.AddRange(loadMessages(transition.Element("MensajesError")));

                transitions.Add(new TransitionPosture(name, skeleton, difficulty, messages, pMatters));
            }
            
            return transitions;
        }

        public static List<Message> loadMessages(XElement readMessages)
        {
            var messages = new List<Message>();

            foreach (XElement message in readMessages.Elements("Mensaje"))
            {
                var msg = message.Element("Texto").Value;
                var joints = loadJoints(message.Element("Articulaciones"));

                if (readMessages.Name.LocalName.Equals("MensajesGuia"))
                    messages.Add(new Message(msg, joints, MessageType.Guide));
                else
                    messages.Add(new Message(msg, joints, MessageType.Error));
            }

            return messages;
        }

        public static List<JointType> loadJoints(XElement readJoints)
        {
            var joints = new List<JointType>();
            foreach (XElement joint in readJoints.Elements())
            {
                var nameJoint = joint.Name.LocalName;
                if (bool.Parse(joint.Value))
                    joints.Add(jointTypes[nameJoint]);
            }

            return joints;
        }



        public static bool saveSkeleton(Skeleton skeleton)
        {
            try
            {
                XElement coordenadas = new XElement("Coordenadas");

                foreach (KeyValuePair<JointType, Joint> pair in skeleton.Joints)
                {
                    object[] position = {
                    new XElement("X", pair.Value.Position.X),
                    new XElement("Y", pair.Value.Position.Y),
                    new XElement("Z", pair.Value.Position.Z)
                };

                    coordenadas.Add(new XElement(pair.Key.ToString(), position));
                }

                XDocument miXML = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("InformacionGesto", coordenadas)
                );

                numSkeletons++;

                miXML.Save(AppDomain.CurrentDomain.BaseDirectory + "skeleton_" + numSkeletons + ".xml");
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
