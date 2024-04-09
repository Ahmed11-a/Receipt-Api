using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
	public class Tax
	{
		public int Id { get; set; }
		[MaxLength(24)]
		public string TaxType { get; set; }
		public decimal Rate { get; set; }


	}
}
