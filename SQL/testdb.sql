--USE [testdb]
USE [1gb_articlesandphotos]
GO

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Customers') and o.name = 'FK_CUSTOMERS_CATEGORIES')
alter table Customers
   drop constraint FK_CUSTOMERS_CATEGORIES
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('Categories')
            and   type = 'U')
   drop table Categories
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('Customers')
            and   type = 'U')
   drop table Customers
GO

/*==============================================================*/
/* Table: Categories                                            */
/*==============================================================*/
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,	
	[CreDate] datetime NULL default GETDATE(),
    [IsActive] bit NULL default 1,
    CONSTRAINT [PK_CATEGORIES] PRIMARY KEY ([Id])
)
GO

/*==============================================================*/
/* Table: Customers                                             */
/*==============================================================*/
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,		
	[Name] [nvarchar](250) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[CreDate] datetime NULL default GETDATE(),
    [IsActive] bit NULL default 1,
	CONSTRAINT [PK_CUSTOMERS] PRIMARY KEY ([Id])
)
GO

ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_CUSTOMERS_CATEGORIES] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO

--ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_CUSTOMERS_CATEGORIES]
--GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CategoryInsert]
GO
CREATE PROCEDURE [dbo].[CategoryInsert]	
	 @Name as nvarchar(250)
AS
BEGIN
--	SET NOCOUNT ON;
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
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerInsert]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CustomerInsert]
GO
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
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCustomerByCategoryId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCustomerByCategoryId]
GO
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
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerInsertUDT]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CustomerInsertUDT]
GO

if exists (select 1
   FROM sys.table_types
   where name = 'BasicUDT')
DROP TYPE [dbo].[BasicUDT]
GO

CREATE TYPE [dbo].[BasicUDT] AS TABLE(	
	[Name] [varchar](128) NULL,
	[CategoryId] [int] NULL
)
GO

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
GO
