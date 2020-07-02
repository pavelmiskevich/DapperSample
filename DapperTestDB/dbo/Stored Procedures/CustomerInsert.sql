CREATE PROCEDURE [dbo].[CustomerInsert]
	 @Name as nvarchar(250)
	,@CategoryId as int
AS
BEGIN
--	SET NOCOUNT ON;
	INSERT INTO [dbo].[Customers]
           ([Name]
		   ,[CategoryId]
           --,[CreDate]
           --,[IsActive]
		   )
     VALUES
           (@Name
		   ,@CategoryId
           --,@CreDate
           --,@IsActive
		   )
	SELECT @@IDENTITY
--	SET NOCOUNT OFF;
END
--RETURN 0
