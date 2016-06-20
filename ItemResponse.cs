using System.Collections.Generic;

namespace DiabloInterfaceAPI
{
    public class ItemInfo
    {
        public string ItemName { get; set; }
        public List<string> Properties { get; set; }
    }

    public class ItemResponse
    {
        public bool IsValid { get; set; }
        public bool Success { get; set; }
        public List<ItemInfo> Items { get; set; }
    }
}
