using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext dbcontext;
        private readonly IWebHostEnvironment hostingEnvoirnment;
        private readonly UserManager<StoreUser> storeUser;

        public DutchSeeder(DutchContext dbcontext, IWebHostEnvironment hostingEnvoirnment, UserManager<StoreUser> storeUser)
        {
            this.dbcontext = dbcontext;
            this.hostingEnvoirnment = hostingEnvoirnment;
            this.storeUser = storeUser;
        }

        public async Task SeedAsync()
        {
            dbcontext.Database.EnsureCreated();
            StoreUser user = await  storeUser.FindByEmailAsync("irfan@gmail.com");
            if(user == null)
            {
                user = new StoreUser()
                {
                     FirstName="irfan",
                      UserName="Irfan",
                     Email= "irfan@gmail.com"
                };
                var userresult= await storeUser.CreateAsync(user, "Ab12@");
                if(userresult != IdentityResult.Success)
                {
                    throw new InvalidOperationException("User is not created");
                }
            }
            if (!dbcontext.Products.Any())
            {
                var filePath = Path.Combine(hostingEnvoirnment.ContentRootPath, "Data/art.json");
                var jsonData = File.ReadAllText(filePath);
                var Productsdata = JsonConvert.DeserializeObject<IEnumerable<Product>>(jsonData);
                dbcontext.Products.AddRange(Productsdata);


                var OrdersData = dbcontext.Orders.Where(s => s.Id == 1).FirstOrDefault();
                if(OrdersData != null)
                {
                    OrdersData.UserInformation = user;
                    OrdersData.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Id=Productsdata.FirstOrDefault().Id,
                           // =Productsdata.FirstOrDefault().Title,
                            Quantity=4,
                            UnitPrice=Productsdata.FirstOrDefault().Price,
                            Product=Productsdata.FirstOrDefault()

                        }
                    };
                }
                dbcontext.SaveChanges();



            }
        }
    }
}
