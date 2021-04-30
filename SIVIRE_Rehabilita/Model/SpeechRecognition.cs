using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.Runtime.InteropServices;

namespace SIVIRE_Rehabilita.Model
{
    class SpeechRecognition
    {
        /// <summary>
        /// Active Kinect sensor.
        /// </summary>
        private KinectSensor kinectSensor;

        /// <summary>
        /// Stream for 32b-16b conversion.
        /// </summary>
        private KinectAudioStream convertStream;

        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine speechEngine;

        private void StartRecognition()
        {
            // Only one sensor is supported
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                // open the sensor
                kinectSensor.Open();

                // grab the audio stream
                IReadOnlyList<AudioBeam> audioBeamList = kinectSensor.AudioSource.AudioBeams;
                System.IO.Stream audioStream = audioBeamList[0].OpenInputStream();

                // create the convert stream
                convertStream = new KinectAudioStream(audioStream);
            }
            else
            {
                // on failure, set the status text
                //this.statusBarText.Text = Properties.Resources.NoKinectReady;
                return;
            }

            RecognizerInfo ri = TryGetKinectRecognizer();

            if (null != ri)
            {
                speechEngine = new SpeechRecognitionEngine(ri.Id);

                // GRAMMAR
                var commands = new Choices();
                commands.Add(new SemanticResultValue("salir", "EXIT"));
                commands.Add(new SemanticResultValue("adiós", "EXIT"));
                commands.Add(new SemanticResultValue("ejercicio", "EXERCISE"));
                
                var gb = new GrammarBuilder { Culture = ri.Culture };
                gb.Append(commands);
                var g = new Grammar(gb);
                speechEngine.LoadGrammar(g);

                speechEngine.SpeechRecognized += this.SpeechRecognized;
                speechEngine.SpeechRecognitionRejected += this.SpeechRejected;
        
                // let the convertStream know speech is going active
                convertStream.SpeechActive = true;

                speechEngine.SetInputToAudioStream(
                    this.convertStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            else
            {
                //this.statusBarText.Text = Properties.Resources.NoSpeechRecognizer;
            }
        }

        private void StopRecognition()
        {
            if (null != this.convertStream)
            {
                convertStream.SpeechActive = false;
            }

            if (null != this.speechEngine)
            {
                speechEngine.SpeechRecognized -= this.SpeechRecognized;
                speechEngine.SpeechRecognitionRejected -= this.SpeechRejected;
                speechEngine.RecognizeAsyncStop();
            }
        }

        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
        /// process audio from Kinect device.
        /// </summary>
        /// <returns>
        /// RecognizerInfo if found, <code>null</code> otherwise.
        /// </returns>
        private static RecognizerInfo TryGetKinectRecognizer()
        {
            IEnumerable<RecognizerInfo> recognizers;

            // This is required to catch the case when an expected recognizer is not installed.
            // By default - the x86 Speech Runtime is always expected. 
            try
            {
                recognizers = SpeechRecognitionEngine.InstalledRecognizers();
            }
            catch (COMException)
            {
                return null;
            }

            foreach (RecognizerInfo recognizer in recognizers)
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "es-ES".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.3;

            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                switch (e.Result.Semantics.Value.ToString())
                {
                    case "EXIT":
                        break;
                }
            }
        }

        /// <summary>
        /// Handler for rejected speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {

        }
    }
}
