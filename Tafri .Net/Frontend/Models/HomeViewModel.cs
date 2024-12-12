namespace Frontend.Models
{
    public class HomeViewModel
    {
        public List<Destination> Destinations { get; set; }
    }

    public class Destination
    {
        public string DestinationName { get; set; }
        public string DestinationImgUrl { get; set; }
    }

}
