CREATE PROCEDURE [dbo].[GetCustomerByCategoryId]
	@CategoryId as int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT   [Id]
			,[Name]
			,[CategoryId]
			,[CreDate]
			,[IsActive]
	FROM [dbo].[Customers]
	WHERE [CategoryId] = @CategoryId
	SET NOCOUNT OFF;
END
--RETURN 0
