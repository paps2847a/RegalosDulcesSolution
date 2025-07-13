namespace DataModel.Models
{
    public record Filter()
    {
        public Dictionary<string, object> parameters { get; set; } = new Dictionary<string, object>();
    }
}