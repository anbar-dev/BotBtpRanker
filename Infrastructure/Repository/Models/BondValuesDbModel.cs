namespace Infrastructure.Repository.Models
{
    public class BondValuesDbModel
    {
        public int? BondId { get; set; }
        public double Price { get; set; }
        public double Yield { get; set; }
        public DateTime UtcTime { get; }
    }
}
