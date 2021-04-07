using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class TutorProfile
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string GradeLevels { get; set; }
        public string About { get; set; }
        public string Education { get; set; }
        public string WorkHistory { get; set; }
        public string WorkExperience { get; set; }
        public string AreasToTeach { get; set; }
        public int LanguageFluencyRate { get; set; }
        public int CommunicationSkillRate { get; set; }
        public bool Active { get; set; }
        public int SchoolBranchId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedById { get; set; }

        [ForeignKey("SchoolBranchId")]
        public virtual SchoolBranch SchoolBranch { get; set; }
    }
}
