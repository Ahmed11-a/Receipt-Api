using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public interface IRepository<T> where T : class
	{
		T GetById(int? id);
		IEnumerable<T> GetAll();
		void Add(T entity);
		void Delete(T entity);
		void DeleteRange(IEnumerable<T> list);

		T Search(Expression<Func<T, bool>>? filter);
		IEnumerable<T> GetAllByFilter(Expression<Func<T, bool>>? filter);

	}
}
