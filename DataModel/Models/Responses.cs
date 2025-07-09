namespace DataModel.Models
{
    public record Responses
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "-";
        public int StatusCode { get; set; } = 200;
        public object? Data { get; set; } = null;
    }
}
