﻿using System;
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
        public Role(string role)
        {
            RoleType = role;
        }
        /// <summary>
        /// The overloaded constructor for creating a role
        /// </summary>
        /// <param name="role">The role that we are creating</param>
        public Role()
        {
            RoleType = null;
        }
        [Key]
        public string RoleType { get; set; }
        // The collection of users associated to a role
        public ICollection<User> User { get; set; }
    }
}
