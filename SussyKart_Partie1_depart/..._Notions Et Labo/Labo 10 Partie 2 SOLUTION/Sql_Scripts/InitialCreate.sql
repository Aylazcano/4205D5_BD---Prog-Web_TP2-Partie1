
-- Création de la BD Lab10

IF EXISTS(SELECT * FROM sys.databases WHERE name='Lab10')
BEGIN
    DROP DATABASE Lab10
END
CREATE DATABASE Lab10

