using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class AdminDto
    {
    }
    public class RequisitionForAddDto
    {       
        public string RequestComment { get; set; }       
    }
    public class RequisitionForUpdateDto
    {
        public int Id { get; set; }
        public string ApproveComment { get; set; }
        public string Status { get; set; }
    }
    public class RequisitionForListDto
    {
        public int Id { get; set; }
        public int RequestById { get; set; }
        public string RequestBy { get; set; }
        public string RequestDateTime { get; set; }
        public string RequestComment { get; set; }
        public int ApproveById { get; set; }
        public string ApproveBy { get; set; }
        public string ApproveDateTime { get; set; }
        public string ApproveComment { get; set; }
        public string Status { get; set; }
    }
}
