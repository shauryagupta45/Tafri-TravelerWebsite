namespace Frontend.Models
{

    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public User User { get; set; }
        public Package Package { get; set; }
    }

}