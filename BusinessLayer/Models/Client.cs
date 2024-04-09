using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
	public class Client:IdentityUser
	{
		[Key]
		public int Id { get; set; }
		public decimal TotalPurchase { get; set; }
		public DateTime CreatedAt { get; set; }
		public virtual ICollection<Receipt> Receipts { get; set; }=new List<Receipt>();

	}
}



