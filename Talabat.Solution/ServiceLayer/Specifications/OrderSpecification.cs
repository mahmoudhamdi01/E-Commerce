using CoreLayer.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Specifications
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string Email):base(O=>O.UserEmail == Email)
        {
            AddIncludes(O => O.DeliveryMethod);
            AddIncludes(O => O.Items);
            AddOrderByDesc(O => O.OrderDate);
        }

        public OrderSpecification(Guid Id) : base(O => O.Id == Id)
        {
            AddIncludes(O => O.DeliveryMethod);
            AddIncludes(O => O.Items);
        }
    }
}
