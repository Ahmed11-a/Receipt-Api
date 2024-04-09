using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
	public class Invoice
	{
		public int Id { get; set; }

		[ForeignKey("ReceiptId")]
		public int? ReceiptId { get; set; }
		public Receipt Receipt { get; set; }


		[ForeignKey("ItemData")]
		public int? ItemDataId { get; set; }
		public ItemData ItemData { get; set; }

		public decimal Quantity { get; set; }
		public decimal UnitPrice { get; set; }

		public string Type { get; set; }
	}
}
