using Microsoft.AspNetCore.Mvc;
using RPS.Core.Models.Dto;
using RPS.Data;
using RPS.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPS.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IPtDashboardRepository rpsDashRepo;

        public DashboardController(IPtDashboardRepository rpsDashData)
        {
            rpsDashRepo = rpsDashData;
        }

        // GET: Dashboard
        public ActionResult Index(int? userId, int? months)
        {
            ViewBag.userId = userId;
            ViewBag.months = months;

            DateTime start = months.HasValue ? DateTime.Now.AddMonths(months.Value * -1) : DateTime.Now.AddYears(-5);
            DateTime end = DateTime.Now;

            PtDashboardFilter filter = new PtDashboardFilter
            {
                DateStart = start,
                DateEnd = end,
                UserId = userId.HasValue ? userId.Value : 0
            };

            var statusCounts = rpsDashRepo.GetStatusCounts(filter);

            PtDashboardVm vm = new PtDashboardVm(statusCounts);

            if (months.HasValue)
            {
                vm.DateStart = filter.DateStart;
                vm.DateEnd = filter.DateEnd;
            }

            return View(vm);
        }
    }
}
