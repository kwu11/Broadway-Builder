using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class RolePermission
    {
        public RolePermission()
        {

        }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public bool isEnabled { get; set; }

        [Key]
        [Required]
        [Column(Order =1)]
        public Enums.PermissionsEnum PermissionID { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public Enums.RoleEnum RoleID { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
