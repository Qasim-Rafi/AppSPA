using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class GenericDto
    {
        [JsonIgnore]
        internal string LoggedIn_UserId { get; set; }
        [JsonIgnore]
        internal string LoggedIn_BranchId { get; set; }
    }
}
