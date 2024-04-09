using BusinessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
	public class ApplicationDbContext : IdentityDbContext<Client>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			

			modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
			modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
			modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();



			


		}
		public DbSet<Discount> Discounts { get; set; }
		public DbSet<Tax> Taxes { get; set; }
		public DbSet<ItemData> itemDatas { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		//public DbSet<TaxItem> TaxItems { get; set; }
		public DbSet<Receipt> Receipts { get; set; }
		public DbSet<ItemInformation> ItemInformation { get; set; }
		public DbSet<Client> Clients { get; set; }




	}
}
