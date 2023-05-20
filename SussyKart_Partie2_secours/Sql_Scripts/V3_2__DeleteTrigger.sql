-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--			  Création des déclencheurs
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

-- Crée le nouveau déclencheur INSTEAD OF DELETE Utilisateurs.Utilisateur
CREATE OR ALTER TRIGGER Utilisateurs.TRd_Utilisateur_Utilisateur
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
END
GO