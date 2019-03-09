using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiForAngular.Models;

namespace WebApiForAngular.Controllers
{
    public class OrdersController : ApiController
    {
        private Models.AppContext db = new Models.AppContext();

        // GET: api/Orders
        public object GetOrders()
        {
			var result =  (from a in db.Orders
						  join b in db.Custumers on a.CustomerId equals b.Id
						  select new
						  {
							  a.Id,
							  a.OrderNo,
							  Customer = b.Name,
							  a.PMethod,
							  a.GTotal
						  }).ToList();
            return result;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
			var order = (from a in db.Orders
						 where a.Id == id
						 select new { a.Id, a.OrderNo,
							 a.CustomerId, a.PMethod,
							 a.GTotal }).FirstOrDefault();

			var orderDetails = (from a in db.OrderItems where a.OrderId == id join b in db.Items on a.ItemId equals b.Id 
							   select new {
								   a.Id,
								   a.OrderId,
								   a.ItemId,
								  ItemName = b.Name,
								  b.Price,
								  a.Quantity,
								  Total = a.Quantity * b.Price

							   }).ToList();
            return Ok(new { order, orderDetails });
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order)
        {
			try
			{
				//if (!ModelState.IsValid)
				//{
				//	return BadRequest(ModelState);
				//}

				if (order.Id ==0)
				{
					db.Orders.Add(order);
				}
				else
				{
					db.Entry(order).State = EntityState.Modified;
				}

				foreach (var item in order.OrderItems)
				{
					if (item.Id == 0)
					{
						db.OrderItems.Add(item);
					}
					else
					{
						db.Entry(item).State = EntityState.Modified;
					}
				}
				if (!string.IsNullOrEmpty( order.DeletedOrderItemIds) )
				{
					foreach (var item in order.DeletedOrderItemIds.Split(',').Where(f=> !string.IsNullOrEmpty(f)))
					{
						var id = int.Parse( item);
						var deleteOrderItem = db.OrderItems.Find(id);
						db.OrderItems.Remove(deleteOrderItem);
					}
				}
				

				await db.SaveChangesAsync();

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
			return Ok();


		}

		// DELETE: api/Orders/5
		[ResponseType(typeof(Order))]
        public 
			IHttpActionResult DeleteOrder(int id)
        {
            Order order =  db.Orders.Include(y=> y.OrderItems).SingleOrDefault( x=> x.Id == id);
			foreach (var item in order.OrderItems.ToList())
			{
				db.OrderItems.Remove(item);

			}
            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}