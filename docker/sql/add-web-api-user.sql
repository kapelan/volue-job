IF NOT EXISTS (SELECT 1 FROM master.dbo.syslogins WHERE name = 'webapi')
	CREATE LOGIN webapi WITH PASSWORD = 'Password123'
ELSE
	ALTER LOGIN webapi WITH PASSWORD = 'Password123'

USE volue_local

IF NOT EXISTS (SELECT 1 FROM sys.database_principals where name = 'webapi')
BEGIN
	BEGIN TRANSACTION
		CREATE USER webapi FOR LOGIN webapi
		EXEC sp_addrolemember 'db_datareader', 'webapi'
		EXEC sp_addrolemember 'db_datawriter', 'webapi'
	COMMIT TRANSACTION
END