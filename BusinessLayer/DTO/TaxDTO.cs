using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    public class TaxDTO
	{
		[MaxLength(24)]
		public string TaxType { get; set; }
		public decimal Rate { get; set; }
	}
}
