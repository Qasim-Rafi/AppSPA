using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<bool> AttendanceExists(int userId)
        {
            if (await _context.Attendances.AnyAsync(x => x.UserId == userId))
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
        public async Task<string> AddAttendance(List<AttendanceDtoForAdd> list)
        {
            try
            {
                foreach (var attendance in list)
                {

                    var objToCreate = new Attendance
                    {
                        Present = attendance.Present,
                        Absent = attendance.Absent,
                        Late = attendance.Late,
                        Comments = attendance.Comments,
                        UserId = attendance.UserId,
                        ClassSectionId = attendance.ClassSectionId,
                        CreatedDatetime = DateTime.Now
                    };

                    await _context.Attendances.AddAsync(objToCreate);
                    await _context.SaveChangesAsync();
                }
               

                return StatusCodes.Status200OK.ToString();
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
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
                    dbObj.Present = attendance.Present;
                    dbObj.Absent = attendance.Absent;
                    dbObj.Late = attendance.Late;
                    await _context.SaveChangesAsync();
                }
                return dbObj;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }
    }
}
