using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/order/{orderid}/OrderItem")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IDutchRepository repository;
        private readonly IMapper mapper;
        private readonly ILogger<OrderItemController> logger;

        public OrderItemController(IDutchRepository repository, IMapper mapper, ILogger<OrderItemController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get(int orderid)
        {
            try
            {
                var orders = repository.GetOrderById(orderid);
                if (orders != null)
                    return Ok(mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(orders.Items));
                else
                    return BadRequest("No data Found");
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex}");
                return BadRequest("Bad Request");
            }
        }

    }
}