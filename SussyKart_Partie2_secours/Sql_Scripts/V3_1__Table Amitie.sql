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

ALTER TABLE Utilisateurs.Amitie 
ADD CONSTRAINT FK_Amitie_UtilisateurID
FOREIGN KEY (UtilisateurID) REFERENCES Utilisateurs.Utilisateur(UtilisateurID)
/* -- Inutile??? avec le nouveau déclencheur INSTEAD OF DELETE Utilisateurs.Utilisateur
ON DELETE CASCADE
ON UPDATE CASCADE;*/
GO

ALTER TABLE Utilisateurs.Amitie 
ADD CONSTRAINT FK_Amitie_UtilisateurID_Ami
FOREIGN KEY (UtilisateurID_Ami) REFERENCES Utilisateurs.Utilisateur(UtilisateurID);
GO

-- Utilisateur avec amis uniques
ALTER TABLE Utilisateurs.Amitie
ADD CONSTRAINT UC_Amitie_AmiUnique 
UNIQUE (UtilisateurID,  UtilisateurID_Ami);
GO

-- Empeche de s'ajouter en tant qu'ami
ALTER TABLE Utilisateurs.Amitie
ADD CONSTRAINT CK_Amitie_SajouterSoiMeme
CHECK (UtilisateurID <> UtilisateurID_Ami);
GO


-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--			  Création des déclencheurs
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

-- Supprime le déclencheur INSTEAD OF DELETE Utilisateurs.Utilisateur précèdent
DROP TRIGGER Utilisateurs.TRd_Utilisateur_Utilisateur
GO

-- Crée le nouveau déclencheur INSTEAD OF DELETE Utilisateurs.Utilisateur
CREATE TRIGGER Utilisateurs.Utilisateur_dtrg_SuprimeUtilisateur
ON Utilisateurs.Utilisateur
INSTEAD OF DELETE
AS
BEGIN
    -- Soft delete des utilisateurs
    UPDATE Utilisateurs.Utilisateur
    SET EstSuppr = 1
    WHERE UtilisateurID IN (SELECT UtilisateurID FROM DELETED);

    -- Suppression des amitiés liées aux utilisateurs supprimés
    DELETE FROM Utilisateurs.Amitie
    WHERE UtilisateurID IN (SELECT UtilisateurID FROM DELETED)
	   OR UtilisateurID_Ami IN (SELECT UtilisateurID FROM DELETED);

    -- Suppression des avatars des utilisateurs supprimés
    DELETE FROM Utilisateurs.Avatar
    WHERE UtilisateurID IN (SELECT UtilisateurID FROM DELETED);

    -- @@@ A VOIR, doit creer un contrainte? @@@ Empêcher d'ajouter un utilisateur supprimé dans la liste d'amis
    DELETE FROM Utilisateurs.Amitie
    WHERE UtilisateurID IN (SELECT UtilisateurID FROM DELETED)
       OR UtilisateurID_Ami IN (SELECT UtilisateurID FROM DELETED);
END
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--			  Création des procédures
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

--USE TP2_SussyKart;
--GO

--CREATE PROCEDURE Utilisateurs.USP_CreerUtilisateur
--	@Pseudo nvarchar(30),
--	@Courriel nvarchar(320)
--AS
--BEGIN
--	SET NOCOUNT ON;
	
--	INSERT INTO Utilisateurs.Utilisateur (Pseudo, DateInscription, Courriel, EstSuppr)
--	VALUES (@Pseudo, GETDATE(), @Courriel, 0);
--END
--GO






