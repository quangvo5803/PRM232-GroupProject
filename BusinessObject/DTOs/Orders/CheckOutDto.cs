using DataAccess.Entities.Application;

namespace BusinessObject.DTOs.Orders
{
    public class CheckOutDto
    {
        public required int ProductId { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required int Count { get; set; }
        public string? ProductAvatar { get; set; }
    }
}
