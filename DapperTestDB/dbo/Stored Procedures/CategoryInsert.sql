CREATE PROCEDURE [dbo].[CategoryInsert]
	@Name as nvarchar(250)
AS
BEGIN
--  SET NOCOUNT ON;
	INSERT INTO [dbo].[Categories]
           ([Name]
           --,[CreDate]
           --,[IsActive]
		   )
     VALUES
           (@Name
           --,@CreDate
           --,@IsActive
		   )
	SELECT @@IDENTITY
--	SET NOCOUNT OFF;
END
--RETURN 0
