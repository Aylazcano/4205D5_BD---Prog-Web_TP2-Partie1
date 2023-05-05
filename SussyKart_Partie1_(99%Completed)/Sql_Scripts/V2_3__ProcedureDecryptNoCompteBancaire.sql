USE TP2_SussyKart;
GO

CREATE TABLE Utilisateurs.CompteBancaire (NoCompteBancaire varchar(20) NOT NULL);

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Création de procedure stockée DecryptNoCompteBancaire
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
GO
CREATE PROCEDURE Utilisateurs.USP_DecryptNoCompteBancaire
	@Pseudo nvarchar(50)
AS
BEGIN
	OPEN SYMMETRIC KEY MaCleSymetrique
	DECRYPTION BY CERTIFICATE MonCertificat;

	SELECT CONVERT(varchar(20), DecryptByKey(NoCompteBancaireChiffre)) AS NoCompteBancaire
	FROM Utilisateurs.Utilisateur
	WHERE Pseudo = @Pseudo

	CLOSE SYMMETRIC KEY MaCleSymetrique;
END