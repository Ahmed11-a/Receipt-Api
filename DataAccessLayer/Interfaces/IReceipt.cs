using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public interface IReceipt:IRepository<Receipt>
	{
		Receipt GetAndInclud(int id);
		IEnumerable<Receipt> GetAllAndInclud();
	}
}
