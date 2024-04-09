using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public interface IUnitOfWork
	{
		IDiscount Discount { get; }
		ITax Tax { get; }
		IItemData ItemData { get; }
		IReceipt Receipt { get; }
		IItemInformation ItemInfo { get; }
		IClient Client { get; }
		IInvoice Invoice { get; }
		void Save();
	}
}
