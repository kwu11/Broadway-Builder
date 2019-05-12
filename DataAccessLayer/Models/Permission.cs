using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    [Table("Permissions")]
    public class Permission
    {
        public Permission(string permission, bool disabled)
        {
            PermissionName = permission.ToLower();
            this.Disabled = disabled;
        }
        public Permission()
        {

        }
  
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Enums.PermissionsEnum PermissionID { get; set; }
        [Required]
        public string PermissionName { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public bool Disabled { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
