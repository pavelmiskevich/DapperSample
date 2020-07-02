/*
В базе данных должна быть предусмотрена файловая группа MEMORY_OPTIMIZED_DATA,
чтобы можно было создать оптимизированный для памяти объект.
*/

--CREATE TYPE [dbo].[UserDefinedTableType1] AS TABLE
--(
--	Id INT NOT NULL IDENTITY PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT=131072), 
--	Name VARCHAR(128)
--) WITH (MEMORY_OPTIMIZED = ON)