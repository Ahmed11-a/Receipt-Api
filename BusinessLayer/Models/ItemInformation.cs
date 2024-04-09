using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
	public class ItemInformation
	{
		public int Id { get; set; }

		[ForeignKey("ItemData")]
		public int? ItemDataId { get; set; }
		public ItemData ItemData { get; set; }

		[ForeignKey("Discount")]
		public int? DiscountId { get; set; }
		public Discount Discount { get; set; }
		public decimal RateDiscount { get; set; }


		[ForeignKey("Tax")]
		public int? TaxId { get; set; }
		public Tax Tax { get; set; }
		public decimal RateTax { get; set; }

		

		public decimal Price { get; set; }
	}
}
