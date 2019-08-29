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
using ServiceLayer.Exceptions;
using System.Configuration;

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
            if (!_dbContext.Theaters.Where(o => o.TheaterID == production.TheaterID).Any())
            {
                throw new NullNotFoundException($"Theater does not exist! with id: {production.TheaterID}");
            }
            _dbContext.Productions.Add(production);

        }

        public Production GetProduction(int productionId)
        {
           var productionByIdQuery = _dbContext.Productions
                .Where(o => o.ProductionID == productionId)
                //.Include(o => o.ProductionDateTime) //  Only Needed if lazy loading is off and you need to get the production date times
                .FirstOrDefault();

            if (productionByIdQuery != null)
            {
                return productionByIdQuery;
            }
                throw new ProductionNotFoundException($"Production does not exist! with id: {productionId}");

        }

        // Returns a list of productions by a previous date. 
        // If a theaterid is passed then it will only return past productions by that theater id
        public List<Production> GetProductionsByPreviousDate(DateTime previousDate, int? theaterID, int pageNum, int pageSize)
        {
            var pastProductionsQuery = _dbContext.Productions
                .Include(production => production.ProductionDateTime)
                .Where(production => production.ProductionDateTime
                    .Where(productionDateTime => productionDateTime.Date <= previousDate).Any());

            if (theaterID != null)
            {
                pastProductionsQuery = pastProductionsQuery
                    .Where(theater => theater.TheaterID == theaterID);
            }

            pastProductionsQuery = pastProductionsQuery
                .OrderByDescending(production => production.ProductionDateTime
                    .OrderByDescending(o => o.Date)
                    .FirstOrDefault().Date);

            return pastProductionsQuery
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .ToList();
        }

        public List<Production> GetProductionsByCurrentAndFutureDate(DateTime currentDate, int? theaterID, int pageNum, int pageSize)
        {
            var currentAndFutureProductionsQuery = _dbContext.Productions
                .Include(production => production.ProductionDateTime)
                .Where(production => production.ProductionDateTime.Where(productionDateTime => productionDateTime.Date >= currentDate).Any());

            if (theaterID != null)
            {
                currentAndFutureProductionsQuery = currentAndFutureProductionsQuery
                    .Where(theater => theater.TheaterID == theaterID);
            }

            currentAndFutureProductionsQuery = currentAndFutureProductionsQuery
                .OrderByDescending(production => production.ProductionDateTime
                    .OrderByDescending(o => o.Date)
                    .FirstOrDefault().Date);

            return currentAndFutureProductionsQuery
                .Skip((pageNum - 1) * pageSize).Take(pageSize)
                .ToList();
        }

        public Production UpdateProduction(Production production)
        {
            Production currentProduction = _dbContext.Productions
                 .Where(o => o.ProductionID == production.ProductionID)
                 .FirstOrDefault(); // Gives you first production that satisfies the where.

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

                return currentProduction;

            }
            throw new ProductionNotFoundException($"Production does not exist! with id: {production.ProductionID}");
        }


        public void DeleteProduction(int productionID)
        {
            Production productionToDelete = _dbContext.Productions
                .Where(o => o.ProductionID == productionID)
                .FirstOrDefault(); //gives you first production that satisfies the where
                //if item doesn't exist it returns null

            if (productionToDelete != null)
            {
                _dbContext.Productions.Remove(productionToDelete);
            }
            else
            {
                throw new ProductionNotFoundException($"Production does not exist! with ID: {productionID}");
            }
        }

        public void SaveProgram(int productionId, HttpPostedFileBase postedFile)
        {

            /*
             * When using a Virtual Directory
             */
            //var filePath = HostingEnvironment.MapPath("~/Programs/Production" + productionId + "/" + productionId + extension);
            //var subdir = HostingEnvironment.MapPath("~/Programs/Production" + productionId);
            //var dir = HostingEnvironment.MapPath("~/Programs/");

            var extension = Path.GetExtension(postedFile.FileName);

            var currentDirectory = ConfigurationManager.AppSettings["FileDir"];

            var dir = Path.Combine(currentDirectory, "Programs/");
            var subdir = Path.Combine(dir, $"Production{productionId}/");
            var filePath = Path.Combine(subdir, $"{productionId}{extension}");

            // If production does not exist then we will not save a program
            if (!_dbContext.Productions.Where(o => o.ProductionID == productionId).Any())
            {
                throw new ProductionNotFoundException($"Production does not exist! with id: {productionId}");
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }

            postedFile.SaveAs(filePath);
        }

        public void SavePhoto(int productionId, HttpPostedFileBase postedFile)
        {
           
           /*
            * When using a Virtual Directory
            */
            //var filePath = HostingEnvironment.MapPath("~/Photos/Production" + productionId + "/" + count + extension);
            //var currentDirectory = Directory.GetCurrentDirectory();
            //var subdir = HostingEnvironment.MapPath("~/Photos/Production" + productionId);

            var extension = Path.GetExtension(postedFile.FileName);

            var currentDirectory = ConfigurationManager.AppSettings["FileDir"];

            var dir = Path.Combine(currentDirectory, "Photos/");
            var subdir = Path.Combine(dir, $"Production{productionId}/");

            // If production does not exist then we will not save the photos
            if (!_dbContext.Productions.Where(o => o.ProductionID == productionId).Any())
            {
                throw new ProductionNotFoundException($"Production does not exist! with id: {productionId}");
            }

            if (!Directory.Exists(subdir))
            {
                Directory.CreateDirectory(subdir);
            }

            var fileCount = Directory.GetFiles(subdir).Count();

            var filePath = Path.Combine(subdir, $"{productionId}-{fileCount}{extension}");

 

            postedFile.SaveAs(filePath);
        }

        public void CreateProductionDateTime(int productionId, ProductionDateTime productionDateTime)
        {
            if (!_dbContext.Productions.Where(o => o.ProductionID == productionId).Any())
            {
                throw new ProductionNotFoundException($"Production does not exist! with id: {productionId}");
            }

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
                throw new ProductionDateTimeNotFoundException($"Production Date Time does not exist! with ID: {productionDateTime.ProductionDateTimeId}");
            }

            return currentProductionDateTime;

        }

        public void DeleteProductionDateTime(ProductionDateTime productionDateTime)
        {
            ProductionDateTime productionDateTimeToDelete = _dbContext.ProductionDateTimes
                .Where(o => o.ProductionDateTimeId == productionDateTime.ProductionDateTimeId)
                .FirstOrDefault();//gives you first production date time that satisfies the where

            if (productionDateTimeToDelete != null)
            {
                _dbContext.ProductionDateTimes.Remove(productionDateTimeToDelete);
            }
            else
            {
                throw new ProductionDateTimeNotFoundException($"Production Date Time does not exist! with ID: {productionDateTime.ProductionDateTimeId}");
            }

        }
    }
}
