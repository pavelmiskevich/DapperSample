/*
Шаблон скрипта после развертывания							
--------------------------------------------------------------------------------------
 В данном файле содержатся инструкции SQL, которые будут добавлены в скрипт построения.		
 Используйте синтаксис SQLCMD для включения файла в скрипт после развертывания.			
 Пример:      :r .\myfile.sql								
 Используйте синтаксис SQLCMD для создания ссылки на переменную в скрипте после развертывания.		
 Пример:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DELETE FROM dbo.Customers;
DELETE FROM dbo.Categories;

INSERT INTO dbo.Categories(Name)
VALUES ('CategoryName1'),
('CategoryName2')

INSERT INTO dbo.Customers(Name, CategoryId)
VALUES ('CustomersName1', 1),
('CustomersName2', 2) --@@IDENTITY