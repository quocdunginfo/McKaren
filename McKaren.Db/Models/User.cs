using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace McKaren.Db.Models
{
    [Table("McKaren.Users")]
    public class User
    {
        public User()
        {
            this.Roles = new List<Role>();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public virtual List<Role> Roles { get; set; }
        public string AuthProvider { get; set; }
        public bool Active { get; set; }
    }
}