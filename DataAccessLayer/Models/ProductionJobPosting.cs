using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer
{
    public class ProductionJobPosting
    {
        //[Key]
        //[Column(Order = 1)]
        public int ProductionID { get; set; }
        //[Key]
        //[Column(Order = 2)]
        //public Guid TheaterID { get; set; }
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column(Order = 3)]
        public int HelpWantedID { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Hours { get; set; }
        [Required]
        public string Requirements { get; set; }
        public Production Production { get; set; }

        public ProductionJobPosting(int productionId, string position, string description, string title, string hour, string requirement)
        {
            this.ProductionID = productionId;
            //this.HelpWantedID = Guid.NewGuid();
            //this.TheaterID = theaterID;
            //this.DateCreated = dateTime;
            this.Position = position;
            this.Description = description;
            this.Title = title;
            this.Hours = hour;
            this.Requirements = requirement;
        }

        public ProductionJobPosting()
        {
            this.Position = "";
            this.Description = "";
            this.Title = "";
            this.Hours = "";
            this.Requirements = "";
        }
    }
}

