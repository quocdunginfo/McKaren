using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace McKaren.Db.Models
{
    [Table("McKaren.Roles")]
    public class Role
    {
        public Role()
        {
            this.Permissions = new List<Permission>();
            this.Users = new List<User>();
        }
        [Key]
        [Required]
        public string Id { get; set; }
        public string Description { get; set; }
        public virtual List<Permission> Permissions { get; set; }
        public virtual List<User> Users { get; set; }
    }
}