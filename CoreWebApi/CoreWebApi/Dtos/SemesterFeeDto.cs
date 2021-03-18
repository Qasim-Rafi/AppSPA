using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class SemesterFeeDto
    {
    }
    public class SemesterDtoForAdd
    {
        public string Name { get; set; }
        public string FeeAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DueDate { get; set; }
        public string LateFeePlentyAmount { get; set; }
        public string LateFeeValidityInDays { get; set; }
    }
    public class SemesterDtoForEdit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeeAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DueDate { get; set; }
        public string LateFeePlentyAmount { get; set; }
        public string LateFeeValidityInDays { get; set; }
    }
    public class SemesterDtoForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeeAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DueDate { get; set; }
        public string LateFeePlentyAmount { get; set; }
        public string LateFeeValidityInDays { get; set; }
        public bool Active { get; set; }
        public bool Posted { get; set; }
    }
    public class SemesterDtoForDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeeAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DueDate { get; set; }
        public string LateFeePlentyAmount { get; set; }
        public string LateFeeValidityInDays { get; set; }
        public bool Active { get; set; }
        public bool Posted { get; set; }
    }
    public class SemesterDtoForPost
    {
        public int Id { get; set; }
        public bool Posted { get; set; }
    }
    public class SemesterFeeMappingDtoForAdd
    {
        public string ClassId { get; set; }
        public string SemesterId { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string InstallmentAmount { get; set; }
    }
    public class SemesterFeeMappingDtoForEdit
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string SemesterId { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string InstallmentAmount { get; set; }
    }
    public class SemesterFeeMappingDtoForList
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string SemesterId { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string InstallmentAmount { get; set; }
        public bool Active { get; set; }
    }
    public class SemesterFeeMappingDtoForDetail
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public string SemesterId { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string InstallmentAmount { get; set; }
        public bool Active { get; set; }
    }
}
