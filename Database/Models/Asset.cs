using Database.Models.Base;

namespace Database.Models
{
	public class Asset : BaseEntity
	{
		public string? Symbol { get; set; }
		public string? Kind { get; set; }
		public string? Exchange { get; set; }
		public string? Description { get; set; }
		public float? TickSize { get; set; }
		public string? Currency { get; set; }
		public string? BaseCurrency { get; set; }
		public Dictionary<string, AssetMapping> Mappings { get; set; }
	}
}
