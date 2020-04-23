using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<ProductController> logger;

        public ProductController(IDutchRepository repository, ILogger<ProductController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult  GetAllProduct()
        {
            try
            {
                return Ok( repository.GetProduct());
            }
            catch(Exception ex) 
            {
                logger.LogError($"There is an issue at {ex.Message},{ex}");
                return BadRequest("Failed to get product");
            }
        }

    }
}