using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication5.Models;

namespace WebApplication5.ViewModels
{
    public class TeamUsersViewModel
    {
        public Team Team { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
