using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication5.Models;

namespace WebApplication5.ViewModels
{
    public class UsersTeamsViewModel
    {
        public int TeamId { get; set; }
        public List<Team> Teams { get; set; }
        public string UserId { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
