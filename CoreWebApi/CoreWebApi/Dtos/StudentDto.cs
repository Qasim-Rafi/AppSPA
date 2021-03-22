﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class StudentDto
    {
    }
    public class StudentFeeDtoForAdd
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassSectionId { get; set; }
        public string Remarks { get; set; }
        public bool Paid { get; set; }
    }
    public class CurrentMonthStudentFeeDtoForList
    {
        public bool CurrentMonthPaidStatus { get; set; }
        public List<StudentFeeDtoForList> AllMonthPaidStatus{ get; set; } = new List<StudentFeeDtoForList>();
    }
    public class StudentFeeDtoForList
    {
        public int StudentId { get; set; }
        public string Student { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassSection { get; set; }
        public string Remarks { get; set; }
        public bool Paid { get; set; }
        public string Month { get; set; }
    }
    public class StudentForFeeListDto
    {
        public int Id { get; set; }        
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string DateofBirth { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string OtherState { get; set; }      
        public string RollNumber { get; set; }
        public int ClassSectionId { get; set; }
        public string ClassSection { get; set; }
        public bool Paid { get; set; }
        public int FeeId { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
    public class FeeVoucherDetailForAddDto
    {
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
        public string PaymentTerms { get; set; }
    }
    public class FeeVoucherDetailForUpdateDto
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
        public string PaymentTerms { get; set; }
    }
    public class FeeVoucherDetailForListDto
    {
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string BankDetails { get; set; }
        public string PaymentTerms { get; set; }
    }
}
