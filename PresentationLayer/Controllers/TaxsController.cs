using BusinessLayer.DTO;
using BusinessLayer.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaxsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<TaxsController> _logger;

		public TaxsController(IUnitOfWork unitOfWork, ILogger<TaxsController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult GetTaxs()
		{
			try
			{
				var taxs = _unitOfWork.Tax.GetAll();

				if (taxs == null || !taxs.Any())
				{
					return NotFound("No taxs found.");
				}

				return Ok(taxs);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieve taxs.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpPost]
		public IActionResult AddTax(TaxDTO taxDTO)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest("Enter Valid tax");
				}

				Tax tax = new Tax
				{
					Rate = taxDTO.Rate,
					TaxType = taxDTO.TaxType,
				};
				_unitOfWork.Tax.Add(tax);
				_unitOfWork.Save();

				return Ok(tax);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to add tax.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteTax(int id)
		{
			if (id == 0 || id == null)
				return BadRequest($"Tax id:{id} is not valid");

			var tax = _unitOfWork.Tax.GetById(id);

			if (tax == null)
				return NotFound($"Tax with id {id} not found");

			try
			{
				_unitOfWork.Tax.Delete(tax);
				_unitOfWork.Save();
				return Ok(tax/*$"Tax with id {id} deleted successfully"*/);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete Tax.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateTax(int id, TaxDTO taxDTO)
		{
			if (id == 0 || id == null)
				return BadRequest($"Tax id:{id} is not valid");

			var tax = _unitOfWork.Tax.GetById(id);

			if (tax == null)
				return NotFound($"Tax with id {id} not found");

			try
			{
				tax.Rate = taxDTO.Rate;
				tax.TaxType = taxDTO.TaxType;
				_unitOfWork.Tax.Update(tax);
				_unitOfWork.Save();
				return Ok(tax);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete tax.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			if (id == 0 || id == null)
				return BadRequest($"Tax id:{id} is not valid");

			var tax = _unitOfWork.Tax.GetById(id);

			if (tax == null)
				return NotFound($"Tax with id {id} not found");

			try
			{

				return Ok(tax);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete tax.");
				return StatusCode(500, "Internal server error");
			}
		}



	}
}
