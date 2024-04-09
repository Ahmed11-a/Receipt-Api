using BusinessLayer.DTO;
using BusinessLayer.Models;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace PresentationLayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReceiptsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<ReceiptsController> _logger;

		public ReceiptsController(IUnitOfWork unitOfWork, ILogger<ReceiptsController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		[HttpPost]
		public IActionResult Add(ReceiptDTO receiptDTO, int? clientKey = null)
		{
			try
			{
				// Check if the model state is valid
				if (!ModelState.IsValid)
				{
					return BadRequest("Enter Valid Data");
				}

				// Create new receipt and receiptInfo objects
				Receipt receipt = new Receipt
				{
					Type = "return",
					CreatedAt = DateTime.Now,
				};

				ReceiptInfo receiptInfo = new ReceiptInfo
				{
					CreatedAt = receipt.CreatedAt,
					Type = receipt.Type,
					ItemInfos = new List<ItemInfos>()
				};

				// Process receipt items if available
				if (receiptDTO.ReceiptItems != null && receiptDTO.ReceiptItems.Any())
				{
					foreach (var iterator in receiptDTO.ReceiptItems)
					{
						// Retrieve item information for each receipt item
						var itemInformation = _unitOfWork.ItemInfo.GetAllByFilter(i => i.ItemDataId == iterator.Id);

						if (itemInformation != null && itemInformation.Any())
						{
							var id = iterator.Id;
							foreach (var iteminfo in itemInformation)
							{
								var itemInfos = new ItemInfos();
								// Update receipt total discount and tax
								receipt.TotalDiscount += iteminfo.RateDiscount;
								receipt.TotalTax += iteminfo.RateTax;
								itemInfos.Discount += iteminfo.RateDiscount;
								itemInfos.Tax += iteminfo.RateTax;

								// Add item information to receiptInfo
								if (id == iteminfo.ItemDataId)
								{
									itemInfos.Id = iterator.Id;
									itemInfos.Price = iteminfo.Price;
									itemInfos.Quantity = iterator.Quantity;

									receiptInfo.ItemInfos.Add(itemInfos);
								}
								id = 0;
							}

						}

						// Retrieve item data and update receipt total sales
						var item = _unitOfWork.ItemData.GetById(iterator.Id);
						var totalSales = item.Price;
						// Update receipt total amount
						receipt.TotalAmount += (totalSales + receipt.TotalDiscount - receipt.TotalTax) * iterator.Quantity;
						receipt.TotalSales += item.Price * iterator.Quantity;
					}
				}

				// Update receiptInfo with calculated totals
				receiptInfo.TotalAmount = receipt.TotalAmount;
				receiptInfo.TotalDiscount = receipt.TotalDiscount;
				receiptInfo.TotalTax = receipt.TotalTax;
				receiptInfo.TotalSales = receipt.TotalSales;

				// Update Total Purchase for client if clientKey is provided
				if (clientKey != null && clientKey != 0)
				{
					var client = _unitOfWork.Client.GetById(clientKey);
					if (client != null)
					{
						client.TotalPurchase += receiptInfo.TotalAmount;
						_unitOfWork.Client.Update(client);
						_unitOfWork.Save();
					}
				}

				// Add receipt and save changes
				_unitOfWork.Receipt.Add(receipt);
				_unitOfWork.Save();
				receiptInfo.Id = receipt.Id;
				// Add invoices for each item in the receipt and save changes
				foreach (var item in receiptInfo.ItemInfos)
				{
					var invoice = new Invoice
					{
						ItemDataId = item.Id,
						Type = receiptInfo.Type,
						ReceiptId = receipt.Id,
						Quantity = item.Quantity,
						UnitPrice = item.Price,
					};
					_unitOfWork.Invoice.Add(invoice);
					_unitOfWork.Save();
				}

				return Ok(receiptInfo);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to add receipt.");
				return StatusCode(500, "Internal server error.");
			}
		}


		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			// Retrieve the receipt with included related entities
			var receipt = _unitOfWork.Receipt.GetAndInclud(id);

			// Check if the receipt exists
			if (receipt == null)
			{
				return NotFound($"Receipt with ID {id} was not found.");
			}

			// Create a new ReceiptInfo object and populate it with receipt data
			var receiptInfo = new ReceiptInfo
			{
				Id = receipt.Id,
				TotalAmount = receipt.TotalAmount,
				TotalDiscount = receipt.TotalDiscount,
				TotalTax = receipt.TotalTax,
				TotalSales = receipt.TotalSales,
				Type = receipt.Type,
				CreatedAt = receipt.CreatedAt,
				ItemInfos = new List<ItemInfos>()
			};

			// Retrieve all invoices associated with the receipt
			var invoice = _unitOfWork.Invoice.GetAllByFilter(i => i.ReceiptId == receipt.Id);

			// Check if there are any invoices
			if (invoice != null && invoice.Any())
			{
				// Loop through each invoice
				foreach (var iterator in invoice)
				{
					// Retrieve item information for the current invoice
					var item = _unitOfWork.ItemInfo.GetAllByFilter(i => i.ItemDataId == iterator.ItemDataId);

					// Loop through each item in the item information
					foreach (var item2 in item)
					{
						// Create a new ItemInfos object and populate it with item data
						var itemInfos = new ItemInfos
						{
							Id = item2.Id,
							Price = item2.Price,
							Discount = item2.RateDiscount,
							Tax = item2.RateTax,
							Quantity = iterator.Quantity,
						};

						// Add the item information to the list in the receipt info
						receiptInfo.ItemInfos.Add(itemInfos);
					}
				}
			}

			return Ok(receiptInfo);
		}

		[HttpGet("GetByType")]
		public IActionResult GetReceipts(string type, int page)
		{
			try
			{
				int pageSize = 10;
				int skip = (page - 1) * pageSize;

				var receipts = _unitOfWork.Receipt
					.GetAllByFilter(r => r.Type == type)
					.Skip(skip)
					.Take(pageSize)
					.Select(receipt => new ReceiptInfo
					{
						TotalAmount = receipt.TotalAmount,
						TotalDiscount = receipt.TotalDiscount,
						TotalTax = receipt.TotalTax,
						TotalSales = receipt.TotalSales,
						Type = receipt.Type,
						CreatedAt = receipt.CreatedAt,
						ItemInfos = new List<ItemInfos>()
					})
					.ToList();

				foreach (var receipt in receipts)
				{
					var invoiceList = _unitOfWork.Invoice.GetAllByFilter(i => i.ReceiptId == receipt.Id);
					foreach (var invoice in invoiceList)
					{
						var itemInfosList = _unitOfWork.ItemInfo.GetAllByFilter(itemInfo => itemInfo.ItemDataId == invoice.ItemDataId);
						foreach (var itemInfo in itemInfosList)
						{
							receipt.ItemInfos.Add(new ItemInfos
							{
								Id = itemInfo.Id,
								Price = itemInfo.Price,
								Discount = itemInfo.RateDiscount,
								Tax = itemInfo.RateTax,
								Quantity = invoice.Quantity
							});
						}
					}
				}

				if (!receipts.Any())
					return NotFound($"No receipts found with type {type}.");

				return Ok(receipts);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieve receipts.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpPost("Return")]
		public IActionResult ReturnReceipt(ReturnReceiptDTO returnReceiptDTO)
		{
			try
			{
				var client = _unitOfWork.Client.GetById(returnReceiptDTO.ClientId);
				if (client == null)
				{
					return NotFound($"This client {returnReceiptDTO.ClientId} is not associated with this invoice. ");
				}
				// Retrieve the original sales receipt
				var originalReceipt = _unitOfWork.Receipt.GetById(returnReceiptDTO.ReceiptId);

				// Check if the original receipt exists
				if (originalReceipt == null)
				{
					return NotFound($"Original sales receipt with ID {returnReceiptDTO.ReceiptId} was not found.");
				}

				var listInvoice = _unitOfWork.Invoice.Search(i => i.ReceiptId == originalReceipt.Id && i.ItemDataId == returnReceiptDTO.ItemId);
				if (listInvoice == null)
				{
					return NotFound($"Item with ID {returnReceiptDTO.ItemId} was not found.");
				}
				if (returnReceiptDTO.Quantity > listInvoice.Quantity)
				{
					return NotFound($"The returned quantity {returnReceiptDTO.Quantity} exceeds the purchased quantity.");
				}
				else if (returnReceiptDTO.Quantity == 0)
				{
					return NotFound("Enter the quantity you want to return.");
				}

				var receipt = new Receipt();
				if (returnReceiptDTO.Quantity <= listInvoice.Quantity)
				{
					var salse = listInvoice.Quantity * listInvoice.UnitPrice;

					receipt.ClientKey = returnReceiptDTO.ClientId;
					receipt.Type = "return";
					receipt.TotalSales = salse;
					receipt.CreatedAt = DateTime.Now;
					receipt.Linked = originalReceipt.Id;
					receipt.TotalAmount = originalReceipt.TotalAmount - salse;

					_unitOfWork.Receipt.Add(receipt);
					_unitOfWork.Save();
					listInvoice.Quantity -= returnReceiptDTO.Quantity;
					_unitOfWork.Invoice.Update(listInvoice) ;
					_unitOfWork.Save();
				}

				// Update the client's total purchase amount by subtracting the returned amount			
				client.TotalPurchase -= originalReceipt.TotalAmount;
				_unitOfWork.Client.Update(client);
				_unitOfWork.Save();

				var receiptDisplay = new { receipt.Id, receipt.ClientKey, receipt.CreatedAt, receipt.TotalAmount, receipt.Type };
				return Ok(receiptDisplay);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to process return receipt.");
				return StatusCode(500, "Internal server error.");
			}
		}



		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var receipt = _unitOfWork.Receipt.GetById(id);

			// Check if the receipt exists
			if (receipt == null)
			{
				return NotFound($"Receipt with ID {id} was not found.");
			}

			try
			{
				var listInvocie = _unitOfWork.Invoice.GetAllByFilter(i => i.ReceiptId == receipt.Id);
				if (listInvocie != null)
				{
					_unitOfWork.Invoice.DeleteRange(listInvocie);
					_unitOfWork.Save();
				}
				// Delete the receipt from the database
				_unitOfWork.Receipt.Delete(receipt);
				_unitOfWork.Save();

				return Ok($"Receipt with ID {id} has been deleted successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete receipt.");
				return StatusCode(500, "Internal server error.");
			}
		}

		[HttpPost("ApplyDiscount")]
		public IActionResult ApplyDiscount(DiscountItemDTO discountItemDTO)
		{
			try
			{
				// Validate input
				if (discountItemDTO == null || discountItemDTO.ItemId == 0 || discountItemDTO.DiscountId == 0)
				{
					return BadRequest("Please provide valid item and discount IDs.");
				}

				// Retrieve item and discount
				var item = _unitOfWork.ItemData.GetById(discountItemDTO.ItemId);
				var discount = _unitOfWork.Discount.GetById(discountItemDTO.DiscountId);

				// Check if item and discount exist
				if (item == null)
				{
					return NotFound($"Item with ID {discountItemDTO.ItemId} was not found.");
				}
				if (discount == null)
				{
					return NotFound($"Discount with ID {discountItemDTO.DiscountId} was not found.");
				}

				// Check if discount has already been applied to the item
				var existingItem = _unitOfWork.ItemInfo.Search(ti => ti.ItemDataId == item.Id && ti.DiscountId == discount.Id);
				if (existingItem != null)
				{
					return Conflict($"The discount amount of {existingItem.RateDiscount} has already been applied to item {item.Id}.");
				}

				// Create or update item information
				var itemInformation = _unitOfWork.ItemInfo.Search(ti => ti.ItemDataId == item.Id && ti.DiscountId == null);
				if (itemInformation == null)
				{
					itemInformation = new ItemInformation
					{
						ItemDataId = item.Id,
						Price = item.Price,
						DiscountId = discount.Id,
						RateDiscount = discount.Rate,

					};
					_unitOfWork.ItemInfo.Add(itemInformation);
				}
				else
				{
					itemInformation.DiscountId = discount.Id;
					itemInformation.Price = item.Price;
					itemInformation.RateDiscount = discount.Rate;
					_unitOfWork.ItemInfo.Update(itemInformation);
				}

				_unitOfWork.Save();

				return Ok($"Discount of {itemInformation.RateDiscount} applied to item {item.Id} successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to assign discount to item.");
				return StatusCode(500, "Internal server error.");
			}
		}


		[HttpPost("ApplyTax")]
		public IActionResult ApplyTax(TaxItemDTO taxItemDTO)
		{
			try
			{
				// Validate input
				if (taxItemDTO == null || taxItemDTO.ItemId == 0 || taxItemDTO.TaxId == 0)
				{
					return BadRequest("Please provide valid item and tax IDs.");
				}

				// Retrieve item and tax
				var item = _unitOfWork.ItemData.GetById(taxItemDTO.ItemId);
				var tax = _unitOfWork.Tax.GetById(taxItemDTO.TaxId);

				// Check if item and tax exist
				if (item == null)
				{
					return NotFound($"Item with ID {taxItemDTO.ItemId} was not found.");
				}
				if (tax == null)
				{
					return NotFound($"Tax with ID {taxItemDTO.TaxId} was not found.");
				}

				// Check if tax has already been applied to the item
				var existingItem = _unitOfWork.ItemInfo.Search(ti => ti.ItemDataId == item.Id && ti.TaxId == tax.Id);
				if (existingItem != null)
				{
					return Conflict($"The tax amount of {existingItem.RateTax} has already been applied to item {item.Id}.");
				}

				// Create or update item information
				var itemInformation = _unitOfWork.ItemInfo.Search(ti => ti.ItemDataId == item.Id && ti.TaxId == null);
				if (itemInformation == null)
				{
					itemInformation = new ItemInformation
					{
						ItemDataId = item.Id,
						TaxId = tax.Id,
						Price = item.Price,
						RateTax = tax.Rate
					};
					_unitOfWork.ItemInfo.Add(itemInformation);
				}
				else
				{
					itemInformation.TaxId = tax.Id;
					itemInformation.Price = item.Price;
					itemInformation.RateTax = tax.Rate;
					_unitOfWork.ItemInfo.Update(itemInformation);
				}

				_unitOfWork.Save();

				return Ok($"Tax of {tax.Rate} applied to item {item.Id} successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to assign tax to item.");
				return StatusCode(500, "Internal server error.");
			}
		}
	}
}




	








