using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Caching;

namespace CasketApplication.Controllers
{
    public class StatusController : ApiController
    {
        private static Random rnd = new Random();

        [HttpGet]
        [ActionName("PlanWorkTime")]
        public Status PlanWorkTime()
        {
            var responce = GetFromCache("Status", () =>
            {
                // Получить статус и даты планируемых работ
                return GetStatus();
            });
            return responce;
        }

        /// <summary>
        /// Обновляем кеш (данные изменились)
        /// </summary>
        public static void PlanWorkTimeChanged()
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove("Status");
        }

        /// <summary>
        /// Получение статуса и время планируемых работ (к примеру из БД)
        /// </summary>
        private static Status GetStatus()
        {
            CasketApplication.Models.SiteStatusModels status = HomeController.Status;
            var newStatus = new Status();
            newStatus.State = status.Work ? State.Work : State.NorWork;
            newStatus.PlanWorkTime = (status.PlanWorkTime ?? new List<CasketApplication.Models.SiteDateTimeIntervalModels>())
                .Where(x => x.BeginTime >= DateTime.Now)
                .Select(x => new DateTimeInterval() { BeginTime = x.BeginTime, EndTime = x.EndTime }).ToList();
            return newStatus;
        }

        /// <summary>
        /// Получение данных из кеша
        /// </summary>
        /// <typeparam name="TEntity">Тип</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="valueFactory">Метод получения данных</param>
        /// <returns></returns>
        private TEntity GetFromCache<TEntity>(string key, Func<TEntity> valueFactory) where TEntity : class
        {
            ObjectCache cache = MemoryCache.Default;
            var newValue = new Lazy<TEntity>(valueFactory);
            var value = cache.AddOrGetExisting(key, newValue, DateTimeOffset.Now.AddDays(1)) as Lazy<TEntity>;
            return (value ?? newValue).Value;
        }
    }

    /// <summary>
    /// Состояние сервера
    /// </summary>
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