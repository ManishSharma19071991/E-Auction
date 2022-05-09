using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EAuction.Domain.Seller;
using EAuction.Service.BidsService;
using EAuction.Service.Model;
using EAuction.Service.ProductService;
using EAuction.Service.SellerService;
using Microsoft.AspNetCore.Mvc;

namespace EAuction.API.Controllers
{
    [Route("api/[controller]")]
    public class SellerController : Controller
    {
        private readonly SellerService _sellerService;        
        private readonly BidService _bidService;

        public SellerController(SellerService sellerService, BidService bidService)
        {
            _sellerService = sellerService;            
            _bidService = bidService;
        }

        /// <summary>
        /// Get All seller information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/getAllSeller")]
        public async Task<IActionResult> GetAllSeller()
        {
            return Ok(await _sellerService.GetAllSeller());
        }

        /// <summary>
        /// Add seller.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/add-seller")]
        public async Task<IActionResult> AddSeller([FromBody] SellerInfo value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sellerService.AddSeller(value);

            return Created("/api/DataEventRecord", result);
        }            
  

    }
}
