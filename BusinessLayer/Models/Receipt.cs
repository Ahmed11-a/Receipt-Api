using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
	public class Receipt
	{
		public int Id { get; set; }
		public int? Linked { get; set; }
		[ForeignKey("Linked")]
		public Receipt LinkedReceipt { get; set; }
		[ForeignKey("Client")]
		public int? ClientKey { get; set; }
		public Client Client { get; set; }

		[MaxLength(24)]
		public string Type { get; set; }
		public decimal TotalSales { get; set; }
		public decimal TotalDiscount { get; set; }
		public decimal TotalTax { get; set; }
		public decimal TotalAmount { get; set; }
		public DateTime CreatedAt { get; set; }

		public virtual ICollection<ItemData> ItemData { get; set; }
	}
}
