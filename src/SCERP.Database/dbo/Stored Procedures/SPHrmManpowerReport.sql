
-- =============================================
-- Author:		<Md.Akheruzzaman>
-- Create date: <04/10/2015>
-- Description:	<> exec SPHrmManpowerReport 196
-- =============================================
CREATE PROCEDURE [dbo].[SPHrmManpowerReport]

						 
AS

BEGIN
	
	SET NOCOUNT ON;
	Select DeptID, 
	isnull(SecID,0) as SecID, 
	isnull(LineID,0) as LineID, 
	Department, Section,   Line ,
	Count(EmployeeCardID) as [Total Employee],
	sum(GrossSalary) as Gross,
	Sum(BasicSalary) as Basic,
	'Plummy Fashions Limited'  as CompanyName,
	'North Norshingpur, Kashipur, Fatullah, Narayanganj' AS FullAddress	
    from VEmployee
    where isactive=1 and [Status]=1
    Group by DeptID, SecID, LineID, Department, Section, Line
   order by DeptID, SecID, LineID
							
																	 													
END




