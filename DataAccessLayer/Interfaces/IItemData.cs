using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public interface IItemData : IRepository<ItemData>
	{
		void Update(ItemData itemData);
		//ItemData GetItemAndInclud(int id);
		//IEnumerable<ItemData> GetAllItemAndInclud();

	}
}
