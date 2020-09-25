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
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DataContext _context;
        public AttendanceRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AttendanceExists(string name)
        {
            if (await _context.Attendances.AnyAsync(x => x.Comments == name))
                return true;
            return false;
        }
        public async Task<Attendance> GetAttendance(int id)
        {
            var attendance = await _context.Attendances.FirstOrDefaultAsync(u => u.Id == id);

            return attendance;
        }

        public async Task<IEnumerable<Attendance>> GetAttendances()
        {
            var attendances = await _context.Attendances.ToListAsync();
           
            return attendances;
        }
        public async Task<Attendance> AddAttendance(AttendanceDtoForAdd attendance)
        {
            try
            {
                var objToCreate = new Attendance
                {
                    Present = attendance.Present,
                    Absent = attendance.Absent,
                    Late = attendance.Late,
                    Comments = attendance.Comments,
                    UserId = _context.Users.First().Id,
                    CreatedDatetime = DateTime.Now
                };

                await _context.Attendances.AddAsync(objToCreate);
                await _context.SaveChangesAsync();

                return objToCreate;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<Attendance> EditAttendance(int id, AttendanceDtoForEdit attendance)
        {
            try
            {
                Attendance dbObj = _context.Attendances.FirstOrDefault(s => s.Id.Equals(id));
                if (dbObj != null)
                {
                    dbObj.Comments = attendance.Comments;
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
