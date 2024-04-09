using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public interface IDiscount : IRepository<Discount>
	{
		void Update(Discount discount);


	}
}
