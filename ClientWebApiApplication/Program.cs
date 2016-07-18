using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;


/*
 * Так как в задаче не указано сколько клиентов (моб. приложений) и как часто они опрашивают сервис,
 * то получение статуса выполнено в одном методе 
 * Для оптимизации можно разбить запрос на 2 и входящим параметром указывать дату (по),
 * чтобы не тянуть на клиента весь массив дат технических работ
 */

namespace ProductStoreClient
{
    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }

    class Program
    {
        static void Main()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                // Тестировалось из под студии, порт гвоздями :)
                client.BaseAddress = new Uri("http://localhost:19812/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET

                while (true)
                {
                    // Получение чистого ответа, для описания протокола
                    //HttpClient webClient = new HttpClient();
                    //var t = await webClient.GetStringAsync("http://localhost:19812/api/Status/PlanWorkTime");


                    HttpResponseMessage response = await client.GetAsync("api/Status/PlanWorkTime");
                    if (response.IsSuccessStatusCode)
                    {
                        Status status = await response.Content.ReadAsAsync<Status>();
                        Console.WriteLine("state = {0}", status.State.ToString());

                        if (status.PlanWorkTime != null && status.PlanWorkTime.Count > 0)
                        {
                            Console.WriteLine("Даты планируемых работ");
                            for (int i = 0; i < status.PlanWorkTime.Count; i++)
                            {
                                Console.WriteLine(String.Format("Дата начала {0}", status.PlanWorkTime[i].BeginTime));
                                Console.WriteLine(String.Format("Дата окончания {0}", status.PlanWorkTime[i].EndTime));
                                Console.WriteLine();
                            }
                        }
                    }
                    Thread.Sleep(5000);
                }
                Console.ReadLine();
            }
        }
    }

    public enum State
    {
        NorWork = 0,
        Work = 1,
    }

    public class Status
    {
        /// <summary>
        /// Состояние сервера (1 - работает, 0 не работает)
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Даты планируемых работ
        /// </summary>
        public List<DateTimeInterval> PlanWorkTime { get; set; }
    }

    public class DateTimeInterval
    {
        /// <summary>
        /// Дата начала тех. работ
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Дата окончания тех. работ
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}