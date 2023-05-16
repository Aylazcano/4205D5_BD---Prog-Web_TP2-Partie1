-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--				Création de la BD
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE MASTER;
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name='TP2_SussyKart')
BEGIN
    DROP DATABASE TP2_SussyKart
END
CREATE DATABASE TP2_SussyKart
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Configurer FILESTREAM avec SQL Server
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart
GO

EXEC sp_configure filestream_access_level, 2 RECONFIGURE

ALTER DATABASE TP2_SussyKart
ADD FILEGROUP FG_Avatars CONTAINS FILESTREAM;
GO
ALTER DATABASE TP2_SussyKart
ADD FILE (
	NAME = FG_Avatars,
	FILENAME = 'C:\EspaceLabo\FG_Avatars'
)
TO FILEGROUP FG_Avatars
GO
-- ERREUR obtenue a partir de mon laptop --
-- Msg 35221, Level 16, State 1, Line 27
-- Could not process the operation. Always On Availability Groups replica manager is disabled on this instance of SQL Server. 
-- Enable Always On Availability Groups, by using the SQL Server Configuration Manager. 
-- Then, restart the SQL Server service, and retry the currently operation. 
-- For information about how to enable and disable Always On Availability Groups, see SQL Server Books Online.
