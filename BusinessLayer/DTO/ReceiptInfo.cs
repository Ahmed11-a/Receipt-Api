using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
	public class ReceiptInfo
	{
		public int Id { get; set; }
		public string Type { get; set; }
		public decimal TotalSales { get; set; }
		public decimal TotalDiscount { get; set; }
		public decimal TotalTax { get; set; }
		public decimal TotalAmount { get; set; }
		public DateTime CreatedAt { get; set; }

		public List<ItemInfos> ItemInfos { get; set; }= new List<ItemInfos>();
	}

	public class ItemInfos
	{
		public int Id { get; set; }
		public decimal Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Discount { get; set; }
		public decimal Tax { get; set; }
	}
}
