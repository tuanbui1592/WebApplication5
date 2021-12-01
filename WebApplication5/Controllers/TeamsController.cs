using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication5.Models;
using WebApplication5.Utils;
using WebApplication5.ViewModels;

namespace WebApplication5.Controllers
{
    [Authorize(Roles = Role.Manager)]
    public class TeamsController : Controller
    {
        private ApplicationDbContext _context;

        public TeamsController()
        {
            _context = new ApplicationDbContext();
        }
        [HttpGet]
        // GET: Teams
        public ActionResult Index()
        {
            List<TeamUsersViewModel> viewModel = _context.UsersTeams
                .GroupBy(i => i.Team)
                .Select(res => new TeamUsersViewModel
                {
                    Team = res.Key,
                    Users = res.Select(u => u.User).ToList()
                })
                .ToList();


            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }

            var newTeam = new Team()
            {
                Name = team.Name
            };
            _context.Teams.Add(newTeam);
            _context.SaveChanges();

            return RedirectToAction("Index", "Teams");
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            var role = _context.Roles.SingleOrDefault(m => m.Name == Role.User);
            // tìm role có name = Role.User
            var viewModel = new UsersTeamsViewModel()
            {
                Teams = _context.Teams.ToList(),
                Users = _context.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddUser(UsersTeamsViewModel viewModel)
        {
            var model = new UserTeam
            {
                TeamId = viewModel.TeamId,
                UserId = viewModel.UserId
            };
            _context.UsersTeams.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index", "Teams");
        }

        [HttpGet]
        public ActionResult RemoveUser()
        {
            var users = _context.UsersTeams.Select(t => t.User)
                .Distinct()
                .ToList();
            var teams = _context.UsersTeams.Select(t => t.Team)
                .Distinct()
                .ToList();

            var viewModel = new UsersTeamsViewModel
            {
                Teams = teams,
                Users = users
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult RemoveUser(UsersTeamsViewModel viewModel)
        {
            var userTeam = _context.UsersTeams
                .SingleOrDefault(t => t.TeamId == viewModel.TeamId && t.UserId == viewModel.UserId);

            if (userTeam == null)
            {
                return HttpNotFound();
            }

            _context.UsersTeams.Remove(userTeam);
            _context.SaveChanges();

            return RedirectToAction("Index", "Teams");
        }

    }
}
