using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace ExpensesSummary.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Nature
    {
        Restaurant, 
        Hotel, 
        Misc
    }
}
