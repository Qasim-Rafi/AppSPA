using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly DataContext _context;
        public LeaveRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> LeaveExists(string name)
        {
            if (await _context.Leaves.AnyAsync(x => x.Details == name))
                return true;
            return false;
        }
        public async Task<Leave> GetLeave(int id)
        {
            var leave = await _context.Leaves.FirstOrDefaultAsync(u => u.Id == id);
            return leave;
        }

        public async Task<IEnumerable<Leave>> GetLeaves()
        {
            var leaves = await _context.Leaves.ToListAsync();
            return leaves;
        }
        public async Task<Leave> AddLeave(LeaveDtoForAdd leave)
        {
            try
            {
                var objToCreate = new Leave
                {
                    Details = leave.Details,
                    FromDate = leave.FromDate,
                    ToDate = leave.ToDate,
                };

                await _context.Leaves.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                return objToCreate;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<Leave> EditLeave(int id, LeaveDtoForEdit leave)
        {
            try
            {
                Leave dbObj = _context.Leaves.FirstOrDefault(s => s.Id.Equals(id));
                if (dbObj != null)
                {
                    dbObj.Details = leave.Details;
                    await _context.SaveChangesAsync();
                }
                return dbObj;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
