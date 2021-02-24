using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPS.Core.Models;
using RPS.Core.Models.Dto;
using RPS.Data;
using RPS.Web.Models.Routing;
using RPS.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPS.Web
{
    public class BacklogController : Controller
    {
        private const int CURRENT_USER_ID = 21; //Fake user id for demo

        private readonly IPtUserRepository rpsUserRepo;
        private readonly IPtItemsRepository rpsItemsRepo;
        private readonly IPtTasksRepository rpsTasksRepo;
        private readonly IPtCommentsRepository rpsCommentsRepo;

        public BacklogController(
            IPtUserRepository rpsUserData,
            IPtItemsRepository rpsItemsData,
            IPtTasksRepository rpsTasksData,
            IPtCommentsRepository rpsCommentsData
            )
        {
            rpsUserRepo = rpsUserData;
            rpsItemsRepo = rpsItemsData;
            rpsTasksRepo = rpsTasksData;
            rpsCommentsRepo = rpsCommentsData;
        }

        // GET: Backlog
        public ActionResult Index()
        {
            //return RedirectToAction("Items", new RouteValueDictionary(
            //    new { controller = "Backlog", action = "Main", preset = "Open" }));
            return RedirectToAction("Items", new { preset = "Open" });
            //return View();

        }

        [Route("Items/{preset}")]
        public ActionResult Items(PresetEnum preset)
        {
            IEnumerable<PtItem> items = null;
            switch (preset)
            {
                case PresetEnum.My:
                    items = rpsItemsRepo.GetUserItems(CURRENT_USER_ID);
                    break;
                case PresetEnum.Open:
                    items = rpsItemsRepo.GetOpenItems();
                    break;
                case PresetEnum.Closed:
                    items = rpsItemsRepo.GetClosedItems();
                    break;
                default:
                    items = rpsItemsRepo.GetOpenItems();
                    break;
            }
            return View("Items", items);
        }

        [Route("{id:int}/Details")]
        public ActionResult Details(int id)
        {
            var item = rpsItemsRepo.GetItemById(id);
            var users = rpsUserRepo.GetAll();
            var currentUser = users.Single(u => u.Id == CURRENT_USER_ID);

            ViewBag.screen = DetailScreenEnum.Details;
            ViewBag.users = users;
            ViewBag.currentUser = currentUser;

            return View("Details", item);
        }

        // POST: Backlog/Detail/5
        [HttpPost]
        [Route("{id:int}/Details")]
        public ActionResult Details(int id, PtItemDetailsVm vm)
        {
            var item = rpsItemsRepo.GetItemById(id);
            var users = rpsUserRepo.GetAll();
            ViewBag.screen = DetailScreenEnum.Details;
            ViewBag.users = users;

            try
            {

                // TODO: Add update logic here
                var updatedItem = rpsItemsRepo.UpdateItem(vm.ToPtUpdateItem());

                return View("Details", updatedItem);
            }
            catch
            {
                return View("Details", item);
            }
        }

        [Route("{id:int}/Tasks")]
        public ActionResult Tasks(int id)
        {
            var item = rpsItemsRepo.GetItemById(id);
            var users = rpsUserRepo.GetAll();
            ViewBag.screen = DetailScreenEnum.Tasks;
            ViewBag.users = users;

            return View("Details", item);
        }

        [HttpPost]
        [Route("{id:int}/Tasks")]
        public ActionResult Tasks(int id, PtItemTasksVm vm)
        {
            ViewBag.screen = DetailScreenEnum.Tasks;

            try
            {
                PtNewTask taskNew = new PtNewTask
                {
                    ItemId = id,
                    Title = vm.NewTaskTitle
                };

                rpsTasksRepo.AddNewTask(taskNew);

                return RedirectToAction("Tasks");
            }
            catch
            {
                return RedirectToAction("Tasks");
            }
        }

        [HttpPost]
        [Route("{id:int}/TaskUpdate/{taskId:int}")]
        public ActionResult TaskUpdate(int id, int taskId, string title, bool? completed)
        {
            ViewBag.screen = DetailScreenEnum.Tasks;

            try
            {
                PtUpdateTask uTask = new PtUpdateTask
                {
                    Id = taskId,
                    ItemId = id,
                    Title = title,
                    Completed = completed.HasValue ? completed.Value : false
                };
                rpsTasksRepo.UpdateTask(uTask);
                return RedirectToAction("Tasks");
            }
            catch
            {
                return RedirectToAction("Tasks");
            }
        }

        [HttpPost]
        [Route("{id:int}/TaskDelete/{taskId:int}")]
        public ActionResult TaskDelete(int id, int taskId, PtItemTasksVm vm)
        {
            ViewBag.screen = DetailScreenEnum.Tasks;

            try
            {
                var result = rpsTasksRepo.DeleteTask(taskId, id);
                return RedirectToAction("Tasks");
            }
            catch
            {
                return RedirectToAction("Tasks");
            }
        }

        [Route("{id:int}/Chitchat")]
        public ActionResult Chitchat(int id)
        {
            var item = rpsItemsRepo.GetItemById(id);
            var users = rpsUserRepo.GetAll();
            var currentUser = users.Single(u => u.Id == CURRENT_USER_ID);

            ViewBag.screen = DetailScreenEnum.Chitchat;
            ViewBag.users = users;
            ViewBag.currentUser = currentUser;

            return View("Details", item);
        }

        [HttpPost]
        [Route("{id:int}/Chitchat")]
        public ActionResult Chitchat(int id, PtItemCommentsVm vm)
        {
            ViewBag.screen = DetailScreenEnum.Chitchat;

            try
            {
                PtNewComment commentNew = new PtNewComment
                {
                    ItemId = id,
                    Title = vm.NewCommentText,
                    UserId = CURRENT_USER_ID
                };

                rpsCommentsRepo.AddNewComment(commentNew);

                return RedirectToAction("Chitchat");
            }
            catch
            {
                return RedirectToAction("Chitchat");
            }
        }

        // GET: Backlog/Create
        public ActionResult Create()
        {
            var vm = new PtNewItemVm();
            return View(vm);
        }

        // POST: Backlog/Create
        [HttpPost]
        public ActionResult Create(PtNewItemVm vm)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var newItem = vm.ToPtNewItem();
                    newItem.UserId = CURRENT_USER_ID;

                    rpsItemsRepo.AddNewItem(newItem);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(vm);
                }


            }
            catch
            {
                return View(vm);
            }
        }

        // GET: Backlog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Backlog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
