using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCoreSamples.Data;
using DotNetCoreSamples.Data.Entities;
using DotNetCoreSamples.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreSamples.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IRepository repository, ILogger<OrdersController> logger,
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            var username = User.Identity.Name;

            try
            {
                var results = _repository.GetAllOrdersByUser(username, includeItems);
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
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
                var order = _repository.GetOrderById(User.Identity.Name, id);

                if (order != null)
                {
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
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
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);


                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;

                    }

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

                    newOrder.User = currentUser;
                    _repository.AddEntity(newOrder);


                    if (_repository.SaveChanges())
                    {
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
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