using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiForAngular.Models
{
	public class Item
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }
	}
}