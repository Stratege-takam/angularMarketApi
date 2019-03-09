using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiForAngular.Models
{
	public class Order
	{
		public int Id { get; set; }
		public string OrderNo { get; set; }
		public int CustomerId { get; set; }
		public Customer Custumer { get; set; }
		public string PMethod { get; set; }
		public double GTotal { get; set; }

		[NotMapped]
		public string DeletedOrderItemIds { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }
	}
}