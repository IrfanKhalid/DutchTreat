using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Migrations;
using DutchTreat.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrderController> logger;
        private readonly IMapper imapper;
        private readonly UserManager<StoreUser> userManager;

        public OrderController(IDutchRepository repository, ILogger<OrderController> logger,IMapper imapper, UserManager<StoreUser> userManager)
        {
            this.repository = repository;
            this.logger = logger;
            this.imapper = imapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAllOrders (){
            try
            {

               var OrdderData= repository.GetAllOrder();
                if (OrdderData != null)
                {
                    return Ok(imapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(OrdderData));
                }
                else return NotFound();
            }
            catch(Exception EX)
            {
                logger.LogError($"Request failed due to {EX}");
                return BadRequest("Request Failed");
            }
            }
        [HttpGet]
        public IActionResult GetAllOrdersByUserName()
        {
            try
            {
                var username = User.Identity.Name;
                var OrdderData = repository.GetAllOrderByUserName(username);
                if (OrdderData != null)
                {
                    return Ok(imapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(OrdderData));
                }
                else return NotFound();
            }
            catch (Exception EX)
            {
                logger.LogError($"Request failed due to {EX}");
                return BadRequest("Request Failed");
            }
        }
        [HttpGet("{id:int}")]
            public IActionResult GetOrderById(int id)
        {
            try
            {
                return Ok(repository.GetOrderById(id));
            }
            catch(Exception ex)
            {
                logger.LogError($"{ex}");
                return null;
            }
        }

        [HttpPost]
        public IActionResult AddOrderData([FromBody]OrderViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var orderData = new Order()
                    {
                        Id = data.OrderId,
                        OrderDate = data.OrderDate == DateTime.MinValue ? DateTime.Now : data.OrderDate,
                        OrderNumber = data.OrderNumber
                    };
                    repository.AddModel(orderData);
                    if (repository.saveAll())
                    {
                        var vm = new OrderViewModel()
                        {
                            OrderDate = orderData.OrderDate,
                            OrderId = orderData.Id,
                            OrderNumber = orderData.OrderNumber
                        };
                        return Created($"/api/Order/{vm.OrderId}", vm);
                    }
                }
                return BadRequest("Bad Request");
            }
            catch(Exception ex)
            {
                logger.LogError($"{ex}");
                return BadRequest("Bad Request");
            }

        }

    }
}