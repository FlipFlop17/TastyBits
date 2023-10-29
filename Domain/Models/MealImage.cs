namespace Domain.Models
{
    public class MealImage
    {
        public string Id { get; set; }
        public string MealId { get; set; }
        public string Data { get; set; }
        public string ImageName { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
