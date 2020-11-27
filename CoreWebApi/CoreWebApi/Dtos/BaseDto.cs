using AutoMapper;
using System.Text.Json.Serialization;

namespace CoreWebApi.Dtos
{
    public class BaseDto
    {
        [JsonIgnore]
        [IgnoreMap]
        internal string LoggedIn_UserId { get; set; }
        [JsonIgnore]
        [IgnoreMap]
        internal string LoggedIn_BranchId { get; set; }
    }
    public class ChipsDto
    {
        public int Value { get; set; }
        public string Display { get; set; }
    }
}
