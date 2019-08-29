using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    [Table("Roles")]
    public class Role
    {
        /// <summary>
        /// The overloaded constructor for creating a role
        /// </summary>
        /// <param name="role">The role that we are creating</param>
        public Role()
        {

        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Enums.RoleEnum RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Required]
        public bool isEnabled { get; set; }

        
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
