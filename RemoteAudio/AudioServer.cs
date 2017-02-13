using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RemoteAudio
{
    public class AudioServer
    {
        private static AudioServer instance;
        private List<Stream> networkClients;

        public AudioServer() {
            instance = this;
            networkClients = new List<Stream>();
        }

        public static AudioServer getInstance() {
            return instance;
        }

        public List<Stream> getNetworkClients()
        {
            return networkClients;
        }

        public void start() {
            // 开始监听http请求
            string baseUrl = "http://*:" + ConfigurationManager.AppSettings["port"] + "/";
            WebApp.Start<StartUp>(baseUrl);
            Console.WriteLine("Server listening on " + ConfigurationManager.AppSettings["port"]);
        }

        public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            lock(networkClients) {
                networkClients.Add(outputStream);
            }
            Program.addClient(outputStream);
            await Task.Run(() => {
                while(networkClients.IndexOf(outputStream) != -1)
                {
                    Thread.Sleep(1000);
                }
            });
            Console.WriteLine("removed");
        }
    }
}
