namespace DataModel.Models
{
    public class SendDataGroupMsg
    {
        public string GroupsIds { get; set; } = string.Empty;
        public string Msg { get; set; } = string.Empty;
        public TimeSpan HourRecord { get; set; }
    }
}
