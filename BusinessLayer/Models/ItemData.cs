using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
	public class ItemData
	{
		public int Id { get; set; }

		[ForeignKey("ReceiptId")]
		public int? ReceiptId { get; set; }
		public Receipt Receipt { get; set; }

		public decimal Quantity { get; set; }
		public decimal Price { get; set; }


		
	}
}
