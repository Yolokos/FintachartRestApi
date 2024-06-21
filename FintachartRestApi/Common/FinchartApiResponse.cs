namespace FintachartRestApi.Common
{
    public class FinchartApiResponse
    {
        public Paging paging { get; set; }
        public List<AssetData> data { get; set; }

        public class Paging
        {
            public int? page { get; set; }
            public int? pages { get; set; }
            public int? items { get; set; }
        }

        public class AssetData
        {
			public string id { get; set; }
			public string? symbol { get; set; }
			public string? kind { get; set; }
            public string? exchange { get; set; }
            public string? description { get; set; }
            public float? tickSize { get; set; }
            public string? currency { get; set; }
            public string? baseCurrency { get; set; }
            public Dictionary<string, Mapping> mappings { get; set; }
        }

        public class Mapping
        {
            public string symbol { get; set; }
            public string exchange { get; set; }
            public int? defaultOrderSize { get; set; }
        }
    }
}
