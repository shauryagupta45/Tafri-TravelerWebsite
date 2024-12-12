namespace API.Collections
{
    public class UpdatePackageCollection
    {
        public int SupplierId { get; set; }
        public string PackageName { get; set; }

        public string PackageDesc { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public string FASL { get; set; }

        public int Duration { get; set; }

        public int PackagePrice { get; set; }

        public int Quantity { get; set; }

        public int PackageId { get; set; }

    }
}
