﻿using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route("api/vi/[controller]")]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;
		private readonly DiscountGrpcService _discountGrpcService;

		public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
		{
			_basketRepository = basketRepository;
			_discountGrpcService = discountGrpcService;
		}

		[HttpGet("{userName}", Name = "GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			var basket = await _basketRepository.GetBasket(userName);
			return Ok(basket ?? new ShoppingCart(userName));
		}

		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
		{
			// TODO: Communicate with Discount Grpc (added as connected service as Client)
			// Then calculate shopping price and push into shopping cart
			foreach (var item in basket.Items) 
			{
				var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
				item.Price -= coupon.Amount;
			}

			return Ok(await _basketRepository.UpdateBasket(basket));
		}

		[HttpDelete]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteBasket(string username)
		{
			await _basketRepository.DeleteBasket(username);
			return Ok();
		}
	}
}
