using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Id
    {
        public string? server { get; set; }
        public string? user { get; set; }
        public string? _serialized { get; set; }
    }

    public class Root
    {
        public Id id { get; set; }
        public string? number { get; set; }
        public bool isBusiness { get; set; }
        public object? isEnterprise { get; set; }
        public object? labels { get; set; }
        public object? name { get; set; }
        public string? pushname { get; set; }
        public object? sectionHeader { get; set; }
        public object? shortName { get; set; }
        public object? statusMute { get; set; }
        public string? type { get; set; }
        public object? verifiedLevel { get; set; }
        public object? verifiedName { get; set; }
        public bool isMe { get; set; }
        public bool isUser { get; set; }
        public bool isGroup { get; set; }
        public bool isWAContact { get; set; }
        public bool isMyContact { get; set; }
        public bool isBlocked { get; set; }
    }
}
