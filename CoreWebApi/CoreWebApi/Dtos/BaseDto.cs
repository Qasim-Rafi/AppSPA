﻿using AutoMapper;
using System.Text.Json.Serialization;

namespace CoreWebApi.Dtos
{
    public class BaseDto
    {
        [JsonIgnore]
        [IgnoreMap]
        internal int LoggedIn_UserId1 { get; set; }
        [JsonIgnore]
        [IgnoreMap]
        internal int LoggedIn_BranchId1 { get; set; }
    }
   
    public class ChipsDto
    {
        public int Value { get; set; }
        public string Display { get; set; }
    }
}
