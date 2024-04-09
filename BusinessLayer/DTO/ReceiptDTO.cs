using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
	public class ReceiptDTO
	{

		public List<ReceiptItem> ReceiptItems { get; set; }=new List<ReceiptItem>();
	}

	public class ReceiptItem
	{
		public int Id { get; set; }
		public decimal Quantity { get; set; }
	}
}
