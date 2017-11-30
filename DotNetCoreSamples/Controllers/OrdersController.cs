using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCoreSamples.Data;
using DotNetCoreSamples.Data.Entities;
using DotNetCoreSamples.ViewModels;

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

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = new Order()
                    {
                        OrderDate = model.OrderDate,
                        OrderNumber = model.OrderNumber,
                        Id=model.OrderId
                    };

                    if (newOrder.OrderDate==DateTime.MinValue)
                    {
                        newOrder.OrderDate=DateTime.Now;
                        
                    }

                    _repository.AddEntity(newOrder);


                    if (_repository.SaveChanges())
                    {
                        //view modeli geri dönmek için

                        var vm = new OrderViewModel()
                        {
                            OrderId = newOrder.Id,
                            OrderDate = newOrder.OrderDate,
                            OrderNumber = newOrder.OrderNumber
                        };

                        return Created($"/api/orders/{vm.OrderId}", vm);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Sipariş kaydetme sırasında bir hata oluştu: {ex}");
            }

            return BadRequest("Sipariş kaydetme işlemi sırasında bir hata oluştu");
        }
    }
}