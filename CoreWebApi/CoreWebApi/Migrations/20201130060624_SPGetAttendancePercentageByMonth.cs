using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreWebApi.Migrations
{
    public partial class SPGetAttendancePercentageByMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string SPGetAttendancePercentageByMonth = @"
                               
                                CREATE PROC GetAttendancePercentageByMonth
                                @SchoolBranchID int,
                                @UserTypeID int
                                AS
                                BEGIN

                                Declare @TotalStudents int

                                Select @TotalStudents = count(*)
                                from Users
                                where SchoolBranchId = @SchoolBranchID and Active = 1 and UserTypeId = @UserTypeID


                                Select MONTH, count(Month) as MonthNumber,CEILING (cast( sum(daywiseaverage) as float) /cast( count(Month) as float)) as Percentage from
                                (SELECT Month(CreatedDatetime) Month,CONVERT(char(10), CreatedDatetime,111) Date , count(*) DateWiseCount
                                , CEILING((cast(count(*) as float) / Cast(@TotalStudents as float)) * 100) daywiseaverage
                                FROM Attendances
                                WHERE DATEPART(w,CreatedDatetime) NOT IN (7,1)
                                And Present = 1
                                group by Month(CreatedDatetime), CONVERT(char(10), CreatedDatetime,111)) as query1

                                Group by Month

                                END";
            migrationBuilder.Sql(SPGetAttendancePercentageByMonth);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string SPGetAttendancePercentageByMonth = @"Drop Procedure GetAttendancePercentageByMonth";
            migrationBuilder.Sql(SPGetAttendancePercentageByMonth);
        }
    }
}
