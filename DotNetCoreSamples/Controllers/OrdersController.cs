using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCoreSamples.Data;

namespace DotNetCoreSamples.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(IRepository repository, ILogger<OrdersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Sipariş listeme sırasında bir hata oluştu: {ex}");
                return BadRequest("Sipariş listeme sırasında bir hata oluştu");

            }

        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);

                if (order != null)
                {
                    return Ok(order);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Sipariş listeme sırasında bir hata oluştu: {ex}");
                return BadRequest("Sipariş listeme sırasında bir hata oluştu");

            }

        }
    }
}