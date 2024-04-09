using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
	public class Discount
	{
		
		public int Id { get; set; }

		
		[MaxLength(128)]
		public string Description { get; set; }
		public decimal Rate { get; set; }


	}
}
