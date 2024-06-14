namespace axians.contacts.services.Models
{
    public class GetAllContactsRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string Term { get; set; }
    }
}