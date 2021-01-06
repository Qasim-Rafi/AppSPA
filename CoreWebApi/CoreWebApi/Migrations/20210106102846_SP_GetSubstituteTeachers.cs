using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class SP_GetSubstituteTeachers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string SPGetSubstituteTeachers = @"      
								IF (OBJECT_ID('SP_GetSubstituteTeachers') IS NOT NULL)
								  DROP PROCEDURE SP_GetSubstituteTeachers
								GO
								CREATE PROCEDURE SP_GetSubstituteTeachers
								@SlotIdParam int
								AS
								BEGIN
	
								-- Declare Cursor
								DECLARE 
								@SubjectId int,
								@SubjectName VARCHAR(MAX), 
								@ClassId  int,
								@ClassName Varchar(Max),
								@TimeSlotId Int,
								@TeacherId Int,
								@Day nvarchar(15),
								@StartTime time,
								@EndTime time

								DECLARE db_SubstituteTeacher CURSOR FOR 

								-- Populate the cursor with your logic
								Select s.Id SubjectId, s.Name SubjectName, cs.ClassId, c.Name ClassName, ca.Id TimeslotId, ca.TeacherId, lt.Day, lt.StartTime, lt.EndTime   from ClassLectureAssignment ca
								inner join Subjects s on  s.Id = ca.SubjectId
								inner join ClassSections cs on cs.Id = ca.ClassSectionId
								inner join Class c on c.Id = cs.ClassId
								inner join LectureTiming lt on ca.LectureId = lt.Id
								where TeacherId is null 
								and ca.Id = @SlotIdParam

								-- Open the Cursor
								OPEN db_SubstituteTeacher

								-- 3 - Fetch the next record from the cursor
								FETCH NEXT FROM db_SubstituteTeacher into 
								@SubjectId,
								@SubjectName ,
								@ClassId  ,
								@ClassName ,
								@TimeSlotId ,
								@TeacherId,
								@Day,
								@StartTime,
								@EndTime

								-- Set the status for the cursor
								WHILE @@FETCH_STATUS = 0  
								BEGIN  	
									--SELECT @SubjectId subjectId,
									--@SubjectName subjectName ,
									--@ClassId classId ,
									--@ClassName className ,
									--@TimeSlotId slotID ,
									--@TeacherId teacherID,
									--@Day day,
									--@StartTime startTime,
									--@EndTime endTime
	
	
									Select TeacherId, u.FullName
									from  users u 
									inner join TeacherExperties exp
									on u.Id = exp.TeacherId 
									where exp.FromToLevels LIKE '%' + convert(varchar,@ClassName) + '%'
									AND SubjectId = @SubjectId
	
			
									-- 5 - Fetch the next record from the cursor
 									FETCH NEXT FROM db_SubstituteTeacher into 
									@SubjectId,
									@SubjectName,
									@ClassId ,
									@ClassName,
									@TimeSlotId,
									@TeacherId,
									@Day,
									@StartTime,
									@EndTime
								END 
								-- 6 - Close the cursor
								CLOSE db_SubstituteTeacher  
								-- 7 - Deallocate the cursor
								DEALLOCATE db_SubstituteTeacher 

								END";
            migrationBuilder.Sql(SPGetSubstituteTeachers);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string SPGetSubstituteTeachers = @"Drop Procedure SP_GetSubstituteTeachers";
            migrationBuilder.Sql(SPGetSubstituteTeachers);
        }
    }
}
