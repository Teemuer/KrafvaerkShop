using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krafvaerk_shop.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Krafvaerk_shop.Controllers
{
    public class ProductController : Controller
    {
        private IRepositoryWrapper _repositoryWrapper;
        public ProductController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public ActionResult Index()
        {
            return View("List");
        }

        // GET: ProductList/Details/5
        public ActionResult Details(int id)
        {
            var product = _repositoryWrapper.IProductRepository.GetPageProduct(id);
            if (product != null)
            {
                return View(product);
            }
            return View(nameof(List));
        }

        // GET: ProductList/List
        public ActionResult List(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                return View(_repositoryWrapper.IProductRepository.SelectPageProductsByCondition(x => x.CategoryId == categoryId));
            }
            else
            {
                return View(_repositoryWrapper.IProductRepository.SelectPageProducts());
            }
        }
    }
}