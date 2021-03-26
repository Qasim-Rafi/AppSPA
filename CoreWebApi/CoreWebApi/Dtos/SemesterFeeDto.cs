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
        public string StudentId { get; set; }
        public string SemesterId { get; set; }
        public string ClassId { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string Installments { get; set; }
        public string Remarks { get; set; }
    }
    public class SemesterFeeMappingDtoForEdit
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string SemesterId { get; set; }
        public string ClassId { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string Installments { get; set; }
        public string Remarks { get; set; }
    }
    public class SemesterFeeMappingDtoForList
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public string DiscountInPercentage { get; set; }
        public string Remarks { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string Installments { get; set; }
        public bool Active { get; set; }
    }
    public class SemesterFeeMappingDtoForDetail
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public string Remarks { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string Installments { get; set; }
        public bool Active { get; set; }
    }
    public class StudentBySemesterDtoForList
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }       
        public string SemesterName { get; set; }
        public string SemesterFeeAmount { get; set; }
        public string DiscountInPercentage { get; set; }
        public string FeeAfterDiscount { get; set; }
        public string Installments { get; set; }
    }
    public class FeeVoucherRecordDtoForList
    {
        public int Id { get; set; }     
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
        public string StudentName { get; set; }
        public string RegistrationNo { get; set; }
        public string BillNumber { get; set; }
        public string BillGenerationDate { get; set; }
        public string DueDate { get; set; }
        public string BillMonth { get; set; }
        public string SemesterSection { get; set; }
        public string ConcessionDetails { get; set; }
        public string FeeAmount { get; set; }
        public string MiscellaneousCharges { get; set; }
        public string TotalFee { get; set; }
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public string VoucherDetailIds { get; set; }
        public List<ExtraChargesForListDto> ExtraCharges { get; set; } = new List<ExtraChargesForListDto>();
    }

    public class ExtraChargesForListDto
    {
        public string ExtraChargesDetails { get; set; }
        public double ExtraChargesAmount { get; set; }
    }
    public class BankAccountForAddDto
    {
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
    }
    public class BankAccountForUpdateDto
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
    }
    public class BankAccountForListDto
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
        public string Month { get; set; }
    }
    public class BankAccountForDetailsDto
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
        public string Month { get; set; }
    }
}
