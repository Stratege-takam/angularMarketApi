using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiForAngular.Models
{
	public class OrderItem
	{
		[Key]
		public int Id { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; }
		public int ItemId { get; set; }
		public Item Item { get; set; }
		public double Quantity { get; set; }

	}
}