namespace shopapp.webapi.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public decimal? Price { get; set; }  
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}