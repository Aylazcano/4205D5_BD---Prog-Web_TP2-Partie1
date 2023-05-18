-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--				Création des tables
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE TABLE Utilisateurs.Amitie(
	AmitieID int IDENTITY(1,1),
	UtilisateurID int NOT NULL,
	UtilisateurID_Ami int NOT NULL,
	CONSTRAINT PK_Amitie_AmitieID PRIMARY KEY (AmitieID)
);


-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--			  Création des contraintes
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

ALTER TABLE Utilisateurs.Amitie ADD CONSTRAINT FK_Amitie_UtilisateurID
FOREIGN KEY (UtilisateurID) REFERENCES Utilisateurs.Utilisateur(UtilisateurID)
ON DELETE CASCADE
ON UPDATE CASCADE;
GO

ALTER TABLE Utilisateurs.Amitie ADD CONSTRAINT FK_Amitie_UtilisateurID_Ami
FOREIGN KEY (UtilisateurID_Ami) REFERENCES Utilisateurs.Utilisateur(UtilisateurID);
GO


CONSTRAINT CK_Amitie_AmisUniques CHECK (UtilisateurID  UtilisateurID_Ami) -- Assure l'unicité et empêche l'auto-amitié




-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--			  Création des déclencheurs
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE TRIGGER Utilisateurs.TRd_Utilisateur_Utilisateur
ON Utilisateurs.Utilisateur INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE Utilisateurs.Utilisateur
	SET EstSuppr = 1
	WHERE UtilisateurID IN (SELECT UtilisateurID FROM deleted);
END
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--			  Création des procédures
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE PROCEDURE Utilisateurs.USP_CreerUtilisateur
	@Pseudo nvarchar(30),
	@Courriel nvarchar(320)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO Utilisateurs.Utilisateur (Pseudo, DateInscription, Courriel, EstSuppr)
	VALUES (@Pseudo, GETDATE(), @Courriel, 0);
END
GO






