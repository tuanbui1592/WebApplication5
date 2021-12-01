using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.Models;
using WebApplication5.Utils;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
    [Authorize(Roles = Role.User)]
    public class TodosController : Controller
    {
        private ApplicationDbContext _context;

        public TodosController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Todos
        [HttpGet]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var todos = _context.Todos
                .Include(t => t.Category)
                //add category
                .Where(t => t.UserId == userId)
                .ToList();   //lay thong tin tu database

            return View(todos);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var categories = _context.Categories.ToList(); //add category
            var viewModel = new TodoCategoriesViewModel()
            {
                Categories = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(TodoCategoriesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //add category
                var viewModel = new TodoCategoriesViewModel
                {
                    Todo = model.Todo,
                    Categories = _context.Categories.ToList()
                };
                return View(viewModel);
            }

            var userId = User.Identity.GetUserId();

            var newTodo = new Todo()
            {
                Description = model.Todo.Description,
                DueDate = model.Todo.DueDate,
                //add category
                CategoryId = model.Todo.CategoryId,
                //add userid
                UserId = userId
            };

            // add todo vào database, sau khi add sẽ save changes

            _context.Todos.Add(newTodo);
            _context.SaveChanges();
            //sau khi save sẽ redirect về trang index
            return RedirectToAction("Index", "Todos");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            // kiểm tra xem id có tồn tại hay không
            var todoInDb = _context.Todos
                .SingleOrDefault(t => t.Id == id && t.UserId == userId);
            // nếu không
            if (todoInDb == null)
            {
                return HttpNotFound();
            }
            // nếu có
            _context.Todos.Remove(todoInDb);
            _context.SaveChanges();
            return RedirectToAction("Index", "Todos");
        }

        [HttpGet]
        // đi tìm id, nếu tìm không ra -> httpnotfound
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var todoInDb = _context.Todos
                .Include(t => t.Category)
                .SingleOrDefault(t => t.Id == id && t.UserId == userId);
            if (todoInDb == null)
            {
                return HttpNotFound();
            }

            return View(todoInDb);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var todoInDb = _context.Todos
                .SingleOrDefault(t => t.Id == id && t.UserId == userId);
            if (todoInDb == null)
            {
                return HttpNotFound();
            }

            var viewModel = new TodoCategoriesViewModel
            {
                Todo = todoInDb,
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(TodoCategoriesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new TodoCategoriesViewModel
                {
                    Todo = model.Todo,
                    Categories = _context.Categories.ToList()
                };
                return View(viewModel);
            }
            var userId = User.Identity.GetUserId();
            var todoInDb = _context.Todos
                .SingleOrDefault(t => t.Id == model.Todo.Id && t.UserId == userId);
            if (todoInDb == null)
            {
                return HttpNotFound();
            }

            todoInDb.Description = model.Todo.Description;
            todoInDb.DueDate = model.Todo.DueDate;
            todoInDb.CategoryId = model.Todo.CategoryId;
            _context.SaveChanges();

            return RedirectToAction("Index", "Todos");
        }

    }
}
