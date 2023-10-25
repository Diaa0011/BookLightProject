using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLight.Models;
using Newtonsoft.Json.Bson;

namespace BookLight.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatus(int id, string orderStatus, string paymentStatus=null);
        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);


	}
}
