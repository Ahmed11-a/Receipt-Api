//using BusinessLayer.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BusinessLayer.DTO
//{
//	public class ItemInfo
//	{
//		public int Id { get; set; }
//		public decimal Quantity { get; set; }
//		public decimal Price { get; set; }
//		public List<DiscountInfo> DiscountInfos { get; set; }=new List<DiscountInfo>();
//		public List<TaxInfo> TaxInfos { get; set; } = new List<TaxInfo>();	

//	}

//	public class TaxInfo
//	{
//		public int Id { get; set; }
//		public decimal Amount { get; set; }
//		public string TaxType { get; set; }
//		public decimal Rate { get; set; }
//	}

//	public class DiscountInfo
//	{
//		public int Id { get; set; }
//		public decimal Amount { get; set; }
//		public string Description { get; set; }
//		public decimal Rate { get; set; }
//	}
//}
