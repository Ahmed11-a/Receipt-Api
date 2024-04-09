using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Models; // Importing models from the BusinessLayer
using DataAccessLayer.Repository; // Importing repository from the DataAccessLayer
using System;
using DataAccessLayer;
using BusinessLayer.DTO;
using DataAccessLayer.Interfaces;

namespace PresentationLayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public ClientsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		//Add a new client
		[HttpPost]
		public IActionResult Add(ClientDTO clientDTO)
		{
			try
			{
				if (clientDTO == null)
				{
					return BadRequest("Please provide valid client data.");
				}
				Client client = new Client
				{
					UserName = clientDTO.Name,
					CreatedAt = DateTime.UtcNow,
					PhoneNumber = clientDTO.Phone,
				};

				_unitOfWork.Client.Add(client);
				_unitOfWork.Save();
				var clientInfo = new {client.Id, client.UserName, client.PhoneNumber, client.TotalPurchase,client.CreatedAt };
				return Ok(clientInfo);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		// Update existing client data
		[HttpPut("{id}")]
		public IActionResult Update(int id, ClientDTO clientDTO)
		{
			try
			{
				var existingClient = _unitOfWork.Client.GetById(id);
				if (existingClient == null)
				{
					return NotFound($"Client with ID {id} was not found.");
				}

				existingClient.UserName = clientDTO.Name;
				existingClient.PhoneNumber = clientDTO.Phone;

				_unitOfWork.Client.Update(existingClient);
				_unitOfWork.Save();
				var clientInfo = new { existingClient.Id, existingClient.UserName, existingClient.PhoneNumber, existingClient.TotalPurchase, existingClient.CreatedAt };
				return Ok(existingClient);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		//Delete an existing client
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				var existingClient = _unitOfWork.Client.GetById(id);
				if (existingClient == null)
				{
					return NotFound($"Client with ID {id} was not found.");
				}

				_unitOfWork.Client.Delete(existingClient);
				_unitOfWork.Save();
				var clientInfo = new { existingClient.Id, existingClient.UserName, existingClient.PhoneNumber, existingClient.TotalPurchase, existingClient.CreatedAt };
				return Ok(clientInfo);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		// api/clients/search_text
		[HttpGet]
		public IActionResult GetClient(string search)
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					return BadRequest("Please provide a search text.");
				}

				var client = _unitOfWork.Client.GetAllByFilter(e => e.UserName.Contains(search) || e.PhoneNumber.Contains(search));

				if (client == null)
				{
					return NotFound($"Client with search text '{search}' was not found.");
				}

				var clientInfos = client.Select(client => new { client.Id, client.UserName, client.PhoneNumber, client.TotalPurchase, client.CreatedAt });
				return Ok(clientInfos);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}

