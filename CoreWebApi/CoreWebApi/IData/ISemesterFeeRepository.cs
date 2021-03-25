using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ISemesterFeeRepository
    {
        Task<bool> SemesterExists(string name);
        Task<ServiceResponse<object>> AddSemester(SemesterDtoForAdd model);
        Task<ServiceResponse<object>> UpdateSemester(SemesterDtoForEdit model);
        Task<ServiceResponse<object>> GetSemester();
        Task<ServiceResponse<object>> GetSemesterById(int id);
        Task<ServiceResponse<object>> PostSemester(SemesterDtoForPost model);
        Task<ServiceResponse<object>> AddSemesterFeeMapping(SemesterFeeMappingDtoForAdd model);
        Task<ServiceResponse<object>> UpdateSemesterFeeMapping(SemesterFeeMappingDtoForEdit model);
        Task<ServiceResponse<object>> GetSemesterFeeMapping();
        Task<ServiceResponse<object>> GetSemesterFeeMappingById(int id);
        //Task<ServiceResponse<object>> SearchStudentsBySemesterClassId(int semId, int classId);
        Task<ServiceResponse<object>> AddFeeVoucherDetails(FeeVoucherDetailForAddDto model);
        Task<ServiceResponse<object>> UpdateFeeVoucherDetails(FeeVoucherDetailForUpdateDto model);
        Task<ServiceResponse<object>> GetFeeVoucherDetails();
        Task<ServiceResponse<object>> GetFeeVoucherDetailsById(int id);
        Task<ServiceResponse<object>> GenerateFeeVoucher(int bankAccountId);
        Task<ServiceResponse<object>> GetStudentsBySemester(int id);
        Task<ServiceResponse<object>> GetAllBankAccount();
        Task<ServiceResponse<object>> GetBankAccountById(int id);
        Task<ServiceResponse<object>> AddBankAccount(BankAccountForAddDto model);
        Task<ServiceResponse<object>> UpdateBankAccount(int id, BankAccountForUpdateDto model);
        Task<ServiceResponse<object>> DeleteBankAccount(int id);
    }
}
