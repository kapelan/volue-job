IF NOT EXISTS (SELECT 1 FROM master.dbo.syslogins WHERE name = 'calculator')
	CREATE LOGIN calculator WITH PASSWORD = 'Password123'
ELSE
	ALTER LOGIN calculator WITH PASSWORD = 'Password123'

USE volue_local

IF NOT EXISTS (SELECT 1 FROM sys.database_principals where name = 'calculator')
BEGIN
	BEGIN TRANSACTION
		CREATE USER calculator FOR LOGIN calculator
		EXEC sp_addrolemember 'db_datareader', 'calculator'
		EXEC sp_addrolemember 'db_datawriter', 'calculator'
	COMMIT TRANSACTION
END