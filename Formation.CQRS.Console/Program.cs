using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using Formation.CQRS.Service.Model;

namespace Formation.CQRS.Consoles
{
    class Program
    {
        private static bool _continueProc = true;

        // The ThreadProc method is called when the thread starts.
        // It loops ten times, writing to the console and yielding
        // the rest of its time slice each time, and then ends.
        public static void ThreadProc() {

            var guid = Guid.NewGuid();
            int i = 0;

            var rand = new Random();

            float lat = (float) (rand.NextDouble() * 0.0002 - 0.0001);
            float lon = (float) (rand.NextDouble() * 0.0002 - 0.0001);

            var model = new GeoLocalisationModel
            {
                guid = guid.ToString(),
                latitude = (float) (rand.NextDouble() * 180 - 90),
                longitude = (float) (rand.NextDouble() * 180 - 90)
            };
            
            while (_continueProc)
            {
                Console.WriteLine("ThreadProc: {0}:{1}", guid.ToString(), i++);

                model.date = DateTime.Now;
                model.latitude += lat;
                model.longitude += lon;
                
                SendGeoLocalisation(model);

                // Yield the rest of the time slice.
                Thread.Sleep(100);
            }
        }

        public static void SendGeoLocalisation(GeoLocalisationModel model)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri("http://localhost:6001");
            
            HttpResponseMessage response;

            using (client)
            {
                string contentData = JsonSerializer.Serialize(model);
                using (StringContent httpContent = new StringContent(contentData, System.Text.Encoding.UTF8, "application/json"))
                {
                    response = client.PostAsync("/api/geolocalisation", httpContent).Result;
                }
            }
        }

        static void Main(string[] args)
        {
            var threadCount = 1;
            if (args.Length > 0)
            {
                threadCount = Int32.Parse(args[0]);
            }

            Console.WriteLine("Main thread: Start {0} threads.", threadCount);

            var threadList = new List<Thread>();
            for (var i = 0 ; i < threadCount; i++)
            {
                threadList.Add(new Thread(new ThreadStart(ThreadProc)));
            }

            threadList.ForEach(t => t.Start());

            Console.WriteLine("Main thread: Call Join(), to wait until ThreadProc ends.");

            Console.ReadLine();

            _continueProc = false;

            threadList.ForEach(t => t.Join());

            Console.WriteLine("Main thread: ThreadProc.Join has returned.  Press Enter to end program.");

            Console.ReadLine();
        }
    }
}
