CREATE PROCEDURE [dbo].[CustomerInsertUDT]
	 @table BasicUDT readonly
AS
BEGIN
--	SET NOCOUNT ON;
	INSERT INTO [dbo].[Customers]
           ([Name]
		   ,[CategoryId]
           --,[CreDate]
           --,[IsActive]
		   )
     SELECT [Name], [CategoryId] FROM @table;
	SELECT @@IDENTITY
--	SET NOCOUNT OFF;
END
--RETURN 0
