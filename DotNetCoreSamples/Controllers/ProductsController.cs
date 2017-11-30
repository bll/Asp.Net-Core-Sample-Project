using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreSamples.Data;
using Microsoft.Extensions.Logging;
using DotNetCoreSamples.Data.Entities;

namespace DotNetCoreSamples.Controllers
{
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetAllProduct());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ürün listeme sırasında bir hata oluştu: {ex}");
                return BadRequest("Ürün listeme sırasında bir hata oluştu");
               
            }
          
        }
    }
}