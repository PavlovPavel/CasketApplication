using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using CasketApplication.Models;

namespace CasketApplication.Controllers
{
    public class HomeController : Controller
    {

        /// <summary>
        /// Статус сервера (для простоты, думаю данные будут в БД)
        /// </summary>
        public static SiteStatusModels Status = new SiteStatusModels() { Work = true, PlanWorkTime = new List<SiteDateTimeIntervalModels>() };

        public ActionResult Index()
        {
            return View(Status);
        }

        [HttpPost]
        public ActionResult Index(SiteStatusModels model)
        {
            Status = model;

            if (Status.PlanWorkTime == null)
                Status.PlanWorkTime = new List<SiteDateTimeIntervalModels>();

            ModelState.Clear();
            StatusController.PlanWorkTimeChanged();
            return View(Status);
        }

        [HttpGet]
        public ActionResult AddDateTimeInterval()
        {
            ViewBag.Title = "Добавление интервала технических работ";
            return View("AddOrEditDateTimeInterval", new SiteDateTimeIntervalModels() { Identifier = Guid.NewGuid(), BeginTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(15) });
        }

        [HttpPost]
        public ActionResult AddDateTimeInterval(SiteDateTimeIntervalModels model)
        {
            if (!Status.PlanWorkTime.Any(x => x.Identifier.Equals(model.Identifier)))
            {
                Status.PlanWorkTime.Add(model);
                Status.PlanWorkTime = Status.PlanWorkTime.OrderBy(x => x.BeginTime).ToList();
                StatusController.PlanWorkTimeChanged();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditDateTimeInterval(Guid guidSiteDateTimeIntervalModels)
        {
            ViewBag.Title = "Редактирование интервала технических работ";
            return View("AddOrEditDateTimeInterval", Status.PlanWorkTime.FirstOrDefault(x => x.Identifier.Equals(guidSiteDateTimeIntervalModels)));
        }

        [HttpGet]
        public ActionResult DeleteDateTimeInterval(Guid guidSiteDateTimeIntervalModels)
        {
            var planWorkTime = Status.PlanWorkTime.FirstOrDefault(x => x.Identifier.Equals(guidSiteDateTimeIntervalModels));
            if (planWorkTime != null)
            {
                Status.PlanWorkTime.Remove(planWorkTime);
                StatusController.PlanWorkTimeChanged();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditDateTimeInterval(SiteDateTimeIntervalModels model)
        {
            var planWorkTime = Status.PlanWorkTime.FirstOrDefault(x => x.Identifier.Equals(model.Identifier));
            if (planWorkTime != null)
            {
                planWorkTime.BeginTime = model.BeginTime;
                planWorkTime.EndTime = model.EndTime;
                Status.PlanWorkTime = Status.PlanWorkTime.OrderBy(x => x.BeginTime).ToList();
                StatusController.PlanWorkTimeChanged();
            }

            return RedirectToAction("Index");
        }
    }
}
