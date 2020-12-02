﻿using CoreWebApi.Dtos;
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
        ServiceResponse<object> _serviceResponse;

        public AttendanceRepository(DataContext context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();

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
        public async Task<string> AddAttendance(string LoggedIn_BranchId, List<AttendanceDtoForAdd> list)
        {

            foreach (var attendance in list)
            {
                var attendanceExist = _context.Attendances.Where(m => m.CreatedDatetime.Date == DateTime.Now.Date && m.UserId == attendance.UserId).FirstOrDefault();
                if (attendanceExist != null)
                {
                    attendanceExist.Present = attendance.Present;
                    attendanceExist.Absent = attendance.Absent;
                    attendanceExist.Late = attendance.Late;
                    attendanceExist.Comments = attendance.Comments;
                    attendanceExist.UserId = attendance.UserId;
                    attendanceExist.ClassSectionId = attendance.ClassSectionId;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    var objToCreate = new Attendance
                    {
                        Present = attendance.Present,
                        Absent = attendance.Absent,
                        Late = attendance.Late,
                        Comments = attendance.Comments,
                        UserId = attendance.UserId,
                        ClassSectionId = attendance.ClassSectionId,
                        CreatedDatetime = DateTime.Now,
                        SchoolBranchId = Convert.ToInt32(LoggedIn_BranchId)
                    };

                    await _context.Attendances.AddAsync(objToCreate);
                    await _context.SaveChangesAsync();
                }
            }


            return StatusCodes.Status200OK.ToString();

        }
        public async Task<Attendance> EditAttendance(int id, AttendanceDtoForEdit attendance)
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

        public Task<IEnumerable<Attendance>> GetAttendanceToDisplay(int typeId, int? classSectionId, string date)
        {
            throw new NotImplementedException();
        }
    }
}
