using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs.OrderDetail
{
    public class OrderDetailCreateRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
