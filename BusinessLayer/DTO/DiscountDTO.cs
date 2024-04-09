using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
	public class DiscountDTO
	{
		

		[MaxLength(128)]
		public string Description { get; set; }
		public decimal Rate { get; set; }

	}
}
