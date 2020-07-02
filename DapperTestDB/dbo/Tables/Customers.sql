CREATE TABLE [dbo].[Customers]
(
	[Id] INT NOT NULL IDENTITY,
	[Name] [NVARCHAR](250) NOT NULL,
	[CategoryId] [INT] NOT NULL,
	[CreDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [IsActive] BIT NOT NULL default 1, 
	CONSTRAINT [PK_CUSTOMERS] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Customers_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id]),
)
