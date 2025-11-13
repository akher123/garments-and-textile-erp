
CREATE FUNCTION [dbo].[fnGetEmployeeAllLeaveInfo] (  

	@FromDate Datetime,
	@ToDate Datetime
)

RETURNS @retLeaveInformation TABLE 
(
	EmployeeId uniqueidentifier PRIMARY KEY NOT NULL,
    DaysInMonth int,
	PresentDays int
)

AS BEGIN

		
			  INSERT @retLeaveInformation Values('adb7d419-a37f-4e6d-b40a-3a1b0a66749f', 555, 666)

			  --SELECT EmployeeId, Sum(EmployeeLeave.AuthorizedTotalDays) [Total Days]
			  --FROM EmployeeLeave
			  --GROUP BY EmployeeLeave.EmployeeId;
	RETURN 
END


