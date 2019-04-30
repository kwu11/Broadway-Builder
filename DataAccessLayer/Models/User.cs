using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    [Table("Users")]
    public class User
    {
        /// <summary>
        /// The defauly constructor that the User instance needs to work.
        /// Sets all the variables to be null.
        /// </summary>
        public User()
        {
            this.Username = null;
            this.FirstName = null;
            this.LastName = null;
            this.DateOfBirth = new DateTime();
            this.StreetAddress = null;
            this.City = null;
            this.StateProvince = null;
            this.Country = null;
            this.Age = 0;
            this.isEnabled = false;
            this.UserGuid = Guid.NewGuid();
        }

        /// <summary>
        /// The constructor for creating a new User instance.
        /// </summary>
        /// <param name="email">The username that a user will use</param>
        /// <param name="password">The password of the user</param>
        /// <param name="dob">The date of birth of the user</param>
        /// <param name="city">The city that the user lives at</param>
        /// <param name="stateProvince">The state or province that the user lives at</param>
        /// <param name="country">The country that the user lives at</param>
        /// <param name="role">The role that the user will have</param>
        /// <param name="isEnabled">The status that the user account can have</param>
        public User(string email, string firstName, string lastName, int age, DateTime dob, string streetAddress, string city, string stateProvince, string country, bool isEnabled,Guid userGuid)
        {
            this.Username = email.ToLower();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
            this.DateOfBirth = dob;
            this.StreetAddress = streetAddress;
            this.City = city;
            this.StateProvince = stateProvince;
            this.Country = country;
            this.isEnabled = isEnabled;
            this.UserGuid = userGuid;
        }

        [Key]
        [Required]
        //[Column(Order =1)]
        public int UserId { get; set; }


        [Required]
        //[Key]
        //[Column(Order = 2)]
        [StringLength(450)]
        [Index(IsClustered =false,IsUnique =true)]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string StateProvince { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public bool isEnabled { get; set; }

        [Required]
        public Guid UserGuid { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
        
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<Resume> Resumes { get; set; }
    }
}
