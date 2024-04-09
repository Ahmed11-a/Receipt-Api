using BusinessLayer.DTO;
using BusinessLayer.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PresentationLayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<ItemsController> _logger;




		public ItemsController(IUnitOfWork unitOfWork, ILogger<ItemsController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;

		}


		[HttpGet]
		public IActionResult GetItem()
		{
			try
			{
				var itemDatas = _unitOfWork.ItemData.GetAll();

				if (itemDatas == null || !itemDatas.Any())
				{
					return NotFound("No item found.");
				}

				return Ok(itemDatas);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieve item.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpPost]
		public IActionResult AddItem(ItemDataDTO itemDataDTO)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest("Enter Valid item");
				}

				ItemData itemData = new ItemData
				{
					Price = itemDataDTO.Price,
					Quantity = itemDataDTO.Quantity,
				};
				_unitOfWork.ItemData.Add(itemData);
				_unitOfWork.Save();

				return Ok(itemData);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to add item.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteItem(int id)
		{
			if (id == 0 || id == null)
				return BadRequest($"Item id:{id} is not valid");

			var item = _unitOfWork.ItemData.GetById(id);

			if (item == null)
				return NotFound($"Item with id {id} not found");

			try
			{
				_unitOfWork.ItemData.Delete(item);
				_unitOfWork.Save();
				return Ok(item/*$"item with id {id} deleted successfully"*/);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete item.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateItem(int id, ItemDataDTO itemDataDTO)
		{
			if (id == 0)
				return BadRequest($"Item id: {id} is not valid");

			var item = _unitOfWork.ItemData.GetById(id);

			if (item == null)
				return NotFound($"Item with id {id} not found");

			try
			{
				item.Price = itemDataDTO.Price;
				item.Quantity = itemDataDTO.Quantity;


				//// Update discounts
				//if (itemDataDTO.Price > 0 && item.Discounts.Count() != 0)
				//{
				//	foreach (var discount in item.Discounts)
				//	{
				//		discount.Amount = itemDataDTO.Price * discount.Rate;
				//		_unitOfWork.Discount.Update(discount);
				//		_unitOfWork.Save();

				//	}
				//}

				//// Update taxes
				//if (itemDataDTO.Price > 0 && (item.Taxes.Count != 0))
				//{
				//	foreach (var tax in item.Taxes)
				//	{
				//		tax.Amount = itemDataDTO.Price * tax.Rate;
				//		_unitOfWork.Tax.Update(tax);
				//		_unitOfWork.Save();

				//	}
				//}



				_unitOfWork.ItemData.Update(item);
				_unitOfWork.Save();
				return Ok(item/*$"Update successful for item with ID: {item.Id}"*/);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to update item.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			if (id == 0 || id == null)
				return BadRequest($"Item id:{id} is not valid");

			var item = _unitOfWork.ItemData.GetById(id);

			if (item == null)
				return NotFound($"Item with id {id} not found");

			try
			{

				//var itemInfo = new ItemInfo
				//{
				//	Id = item.Id,
				//	Price = item.Price,
				//	Quantity = item.Quantity,

				//};
				//if (item.Discounts != null && item.Discounts.Any())
				//{
				//	foreach (var discount in item.Discounts)
				//	{
				//		DiscountInfo discountInfo = new DiscountInfo
				//		{
				//			Id = discount.Id,
				//			Amount = discount.Amount,
				//			Description = discount.Description,
				//			Rate = discount.Rate,
				//		};
				//		itemInfo.DiscountInfos.Add(discountInfo);
				//	}
				//}
				//if (item.Taxes != null && item.Taxes.Any())
				//{
				//	foreach (var tax in item.Taxes)
				//	{
				//		TaxInfo taxInfo = new TaxInfo
				//		{
				//			Id = tax.Id,
				//			Amount = tax.Amount,
				//			Rate = tax.Rate
				//		};
				//		itemInfo.TaxInfos.Add(taxInfo);
				//	}
				//}
				return Ok(item);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieval Item.");
				return StatusCode(500, "Internal server error");
			}
		}

	}
}
