using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace McKaren.Db.Models
{
    [Table("McKaren.Permissions")]
    public class Permission
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual List<Role> Roles { get; set; } 
    }
}