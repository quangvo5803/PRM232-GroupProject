namespace BusinessObject.DTOs.Admin
{
    public class StaticDto
    {
        public int TotalOrdersMonth { get; set; }
        public double EarningsMonth { get; set; }
        public int UserCount { get; set; }
        public double AvgRating { get; set; }
        public List<double> TotalPricesEachMonth { get; set; } = new List<double>();
        public List<string>? CategoriesName { get; set; } 
        public List<int>? TotalQuantity { get; set; } 

    }
}
