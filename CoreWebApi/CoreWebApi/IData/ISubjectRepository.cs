using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetSubjects();
        Task<Subject> GetSubject(int id);
        Task<bool> SubjectExists(string username);
        Task<Subject> AddSubject(Subject subject);
        Task<Subject> EditSubject(int id, Subject subject);
    }
}
