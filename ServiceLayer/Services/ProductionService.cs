using DataAccessLayer;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace ServiceLayer.Services
{
   public class ProductionService
    {
        private readonly BroadwayBuilderContext _dbContext;

        public ProductionService(BroadwayBuilderContext dbcontext)
        {
            _dbContext = dbcontext;
        } 

        public void CreateProduction(Production production)
        {
            _dbContext.Productions.Add(production);
        }

        public Production GetProduction(int productionId)
        {
            return _dbContext.Productions
                .Where(o => o.ProductionID == productionId)
                //.Include(o => o.ProductionDateTime) //  Only Needed if lazy loading is off and you need to get the production date times
                .FirstOrDefault(); //if item doesn't exist it returns null Todo: throw a specific exception
        }

        // Returns a list of productions by a previous date. 
        // If a theaterid is passed then it will only return past productions by that theater id
        public List<Production> GetProductionsByPreviousDate(DateTime previousDate, int? theaterID)
        {
            var pastProductionsQuery = _dbContext.Productions
                .Include(production => production.ProductionDateTime)
                .Where(production => production.ProductionDateTime.Where(productionDateTime => productionDateTime.Date <= previousDate).Any());

            if (theaterID != null)
            {
                pastProductionsQuery = pastProductionsQuery
                    .Where(theater => theater.TheaterID == theaterID);
            }

            return pastProductionsQuery.ToList();
        }

        public List<Production> GetProductionsByCurrentAndFutureDate(DateTime currentDate)
        {
            return _dbContext.Productions
                .Include(production => production.ProductionDateTime)
                .Where(production => production.ProductionDateTime.Where(productionDateTime => productionDateTime.Date >= currentDate).Any())
                .ToList();
        }

        public Production UpdateProduction(Production production)
        {
            Production currentProduction = _dbContext.Productions
                 .Where(o => o.ProductionID == production.ProductionID)
                 .FirstOrDefault(); //gives you first production that satisfies the where. 
                //if item doesn't exist it returns null Todo: throw a specific exception

            if (currentProduction != null)
            {
                currentProduction.ProductionName = production.ProductionName;
                currentProduction.DirectorFirstName = production.DirectorFirstName;
                currentProduction.DirectorLastName = production.DirectorLastName;
                currentProduction.Street = production.Street;
                currentProduction.City = production.City;
                currentProduction.StateProvince = production.StateProvince;
                currentProduction.Country = production.Country;
                currentProduction.Zipcode = production.Zipcode;
            } else
            {
                //throw an exception
            }

            return currentProduction;
        }


        public void DeleteProduction(Production production)
        {
            Production productionToDelete = _dbContext.Productions
                .Where(o => o.ProductionID == production.ProductionID)
                .FirstOrDefault(); //gives you first production that satisfies the where
                //if item doesn't exist it returns null Todo: throw a specific exception

            // If the production found is not null, delete the production
            if (productionToDelete != null)
            {
                _dbContext.Productions.Remove(productionToDelete);
            }
            else
            {
                //throw an exception
            }
        }

        public void UploadProgram(int productionId, string extension, HttpPostedFile postedFile)
        {
            var filePath = HostingEnvironment.MapPath("~/Programs/Production" + productionId + "/" + productionId + extension);
            //check if prodid exists in database because we dont store data for things that don't exist by getting it and check if that variable is null. if it is null then it doesnt exist
            //
            var subdir = HostingEnvironment.MapPath("~/Programs/Production" + productionId);

            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }
          
            postedFile.SaveAs(filePath);
        }

        public void UploadPhoto(int productionId, int count, string extension, HttpPostedFile postedFile)
        {
            var filePath = HostingEnvironment.MapPath("~/Photos/Production" + productionId + "/" + count + extension);

            //var filePath = HostingEnvironment.MapPath("~/ProductionPhotos/" + productionId + "-" + count + extension);
            //postedFile.SaveAs(filePath);

            var subdir = HostingEnvironment.MapPath("~/Photos/Production" + productionId);

            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }
            postedFile.SaveAs(filePath);
        }

        public void CreateProductionDateTime(ProductionDateTime productionDateTime)
        {
            _dbContext.ProductionDateTimes.Add(productionDateTime);
        }

        public ProductionDateTime UpdateProductionDateTime(ProductionDateTime productionDateTime)
        {
            ProductionDateTime currentProductionDateTime = _dbContext.ProductionDateTimes
                .Where(prodDateTime => prodDateTime.ProductionDateTimeId == productionDateTime.ProductionDateTimeId)
                .FirstOrDefault();

            if (currentProductionDateTime != null)
            {
                currentProductionDateTime.Date = productionDateTime.Date;
                currentProductionDateTime.Time = productionDateTime.Time;
            } else
            {
                //throw an exception
            }

            return currentProductionDateTime;

        }

        public void DeleteProductionDateTime(ProductionDateTime productionDateTime)
        {
            ProductionDateTime productionDateTimeToDelete = _dbContext.ProductionDateTimes
                .Where(o => o.ProductionDateTimeId == productionDateTime.ProductionDateTimeId)
                .FirstOrDefault();//gives you first production date time that satisfies the where
                                  //if item doesn't exist it returns null Todo: throw a specific exception

            // If the production date time found is not null, delete the production
            if (productionDateTimeToDelete != null)
            {
                _dbContext.ProductionDateTimes.Remove(productionDateTimeToDelete);
            }
            else
            {
                //throw an exception
            }

        }
    }
}
