using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Krafvaerk_shop.Models;
using Microsoft.EntityFrameworkCore;
using Krafvaerk_shop.Interfaces;

namespace Krafvaerk_shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;
   
        public HomeController(ILogger<HomeController> logger, IRepositoryWrapper repositoryWrapper)
        {
            _logger = logger;
            _repositoryWrapper = repositoryWrapper;
        }

        public IActionResult Index()
        {
            List<PageProduct> products = _repositoryWrapper.IProductRepository.SelectNewestPageProducts(3);
            if (products.Any())
            {
                return View(new HomeViewModel(products));
            }
            return View(new HomeViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
