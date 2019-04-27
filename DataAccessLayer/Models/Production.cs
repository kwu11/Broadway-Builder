using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataAccessLayer
{
    [Table("Productions")]
    public class Production
    {
        [Key]
        // [Column(Order = 1)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductionID { get; set; }
        [Required]
        public string ProductionName { get; set; }
        [Required]
        public string DirectorFirstName { get; set; }
        [Required]
        public string DirectorLastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StateProvince { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Zipcode { get; set; }

        // [Key]
        // [Column(Order = 2)]
        public int TheaterID { get; set; }
        public Theater theater { get; set; }

        #region -- Relationship --

        public ICollection<ProductionJobPosting> ProductionJobPostings { get; set; }

        public ICollection<ProductionDateTime> ProductionDateTime { get; set; }

        #endregion

        // Overloaded Constructor
        public Production(int theaterId, string productionName, string directorFirstName, string directorLastName, string street, string city, string stateProvince, string country, string zipcode)
        {
            //ProductionID = Guid.NewGuid();
            TheaterID = theaterId;
            ProductionName = productionName;
            DirectorFirstName = directorFirstName;
            DirectorLastName = directorLastName;
            Street = street;
            City = city;
            StateProvince = stateProvince;
            Country = country;
            Zipcode = zipcode;
           
        }

        // Parameterless constructor due to EF needing it because we provided an overloading constructor
        public Production()
        {

        }

    }
}
