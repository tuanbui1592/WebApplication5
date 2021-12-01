using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class UserTeam
    {
        [Key, Column(Order = 1)]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
