using BusinessLayer.DTO;
using BusinessLayer.Models;
using DataAccessLayer;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DiscountsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<DiscountsController> _logger;

		public DiscountsController(IUnitOfWork unitOfWork, ILogger<DiscountsController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}


		[HttpGet]
		public IActionResult GetDiscounts()
		{
			try
			{
				var discounts = _unitOfWork.Discount.GetAll();

				if (discounts == null || !discounts.Any())
				{
					return NotFound("No discounts found.");
				}
				var displaydiscounts = discounts.Select(discounts => new { discounts.Id, discounts.Description, discounts.Rate });

				return Ok(displaydiscounts);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieve discounts.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpPost]
		public IActionResult AddDiscount(DiscountDTO discountDTO)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest("Enter Valid Discount");
				}


				Discount discount = new Discount
				{
					Description = discountDTO.Description,
					Rate = discountDTO.Rate,
				};
				_unitOfWork.Discount.Add(discount);
				_unitOfWork.Save();

				return Ok(discount);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to add discount.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteDiscount(int id)
		{
			if (id == 0 || id == null)
				return BadRequest($"Discounts id:{id} is not valid");

			var discount = _unitOfWork.Discount.GetById(id);

			if (discount == null)
				return NotFound($"Discount with id {id} not found");
			try
			{
				_unitOfWork.Discount.Delete(discount);
				_unitOfWork.Save();
				return Ok(discount/*$"Discount with id {id} deleted successfully"*/);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete discount.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateDiscount(int id, DiscountDTO discountDTO)
		{
			if (id == 0 || id == null)
				return BadRequest($"Discounts id:{id} is not valid");

			var discount = _unitOfWork.Discount.GetById(id);

			if (discount == null)
				return NotFound($"Discount with id {id} not found");

			try
			{
				discount.Description = discountDTO.Description;
				discount.Rate = discountDTO.Rate;
				_unitOfWork.Discount.Update(discount);
				_unitOfWork.Save();
				//var displaydiscounts = new { discount.Id, discount.Description, discount.Rate };

				return Ok(discount);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete discount.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			if (id == 0 || id == null)
				return BadRequest($"Discounts id:{id} is not valid");

			var discount = _unitOfWork.Discount.GetById(id);

			if (discount == null)
				return NotFound($"Discount with id {id} not found");

			try
			{
				var displaydiscounts = new { discount.Id, discount.Description, discount.Rate };
				return Ok(displaydiscounts);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete discount.");
				return StatusCode(500, "Internal server error");
			}
		}



	}
}
