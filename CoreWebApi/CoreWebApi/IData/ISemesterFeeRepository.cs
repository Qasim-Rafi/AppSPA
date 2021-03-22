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
        Task<ServiceResponse<object>> AddSemesterFee(SemesterFeeMappingDtoForAdd model);
        Task<ServiceResponse<object>> UpdateSemesterFee(SemesterFeeMappingDtoForEdit model);
        Task<ServiceResponse<object>> GetSemesterFee();
        Task<ServiceResponse<object>> GetSemesterFeeById(int id);
        Task<ServiceResponse<object>> SearchStudentsBySemesterClassId(int semId, int classId);
        Task<ServiceResponse<object>> AddFeeVoucherDetails(FeeVoucherDetailForAddDto model);
        Task<ServiceResponse<object>> UpdateFeeVoucherDetails(FeeVoucherDetailForUpdateDto model);
        Task<ServiceResponse<object>> GetFeeVoucherDetails();
        Task<ServiceResponse<object>> GenerateFeeVoucher();

    }
}
