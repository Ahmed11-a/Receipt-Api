using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public interface ITax : IRepository<Tax>
	{
		void Update(Tax tax);
	}
}
