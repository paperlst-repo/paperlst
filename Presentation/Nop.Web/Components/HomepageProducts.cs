using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class HomepageProductsViewComponent : NopViewComponent
    {
        private readonly IAclService _aclService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IStoreMappingService _storeMappingService;

        public HomepageProductsViewComponent(IAclService aclService,
            IProductModelFactory productModelFactory,
            IProductService productService,
            IStoreMappingService storeMappingService)
        {
            this._aclService = aclService;
            this._productModelFactory = productModelFactory;
            this._productService = productService;
            this._storeMappingService = storeMappingService;
        }

        //public IViewComponentResult Invoke(int? productThumbPictureSize)
        //{
        //    var products = _productService.GetAllProductsDisplayedOnHomePage();
        //    //ACL and store mapping
        //    products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
        //    //availability dates
        //    products = products.Where(p => _productService.ProductIsAvailable(p)).OrderByDescending(o => o.CreatedOnUtc).ToList();

        //    if (!products.Any())
        //        return Content("");

        //    var model = _productModelFactory.PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();

        //    return View(model);
        //}

        public IViewComponentResult Invoke(int? productThumbPictureSize, int items, int page, bool recommended)
        {
            var products = new List<Product>();
            if (recommended)
            {
                products = _productService.GetAllProductsDisplayedOnHomePage().Where(p => p.AllowCustomerReviews).OrderByDescending(o => o.CreatedOnUtc).Take(items).ToList();
            }
            else
            {
                products = _productService.GetAllProductsDisplayedOnHomePage().OrderByDescending(o => o.CreatedOnUtc).Skip((page - 1) * items).Take(items).ToList();
            }

            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //availability dates
            //products = products.Where(p => _productService.ProductIsAvailable(p)).ToList();

            if (!products.Any())
                return Content("");

            var model = _productModelFactory.PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();

            return View(model);
        }
    }
}