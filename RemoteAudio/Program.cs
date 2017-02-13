using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using System.IO;
using CSCore.MediaFoundation;
using CSCore.Codecs;
using System.Threading;
using CSCore.Streams;

namespace RemoteAudio
{
    class Program
    {
        private static AudioServer audioServer = null;
        private static WasapiCapture capture = null;

        public static void addClient(Stream httpStream) {
            if (audioServer == null || capture == null || capture.RecordingState != RecordingState.Recording)
                return;
            Console.WriteLine("addClient called");

            Task.Run(() => {
                using (var encoder = MediaFoundationEncoder.CreateMP3Encoder(capture.WaveFormat, httpStream, 48000))
                {
                    capture.DataAvailable += (s, e) =>
                    {
                        encoder.Write(e.Data, e.Offset, e.ByteCount);
                    };
                    // block here
                    while (true)
                        Thread.Sleep(100);
                }
            });
        }

        static void Main(string[] args)
        {
            using (capture = new WasapiLoopbackCapture())
            {

                capture.Initialize();
                audioServer = new AudioServer();
                audioServer.start();
                capture.Start();
                Console.ReadKey();
                capture.Stop();

            }
        }
    }
}

