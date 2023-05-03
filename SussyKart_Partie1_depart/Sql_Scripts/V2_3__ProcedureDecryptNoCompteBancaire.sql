USE TP2_SussyKart;
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Création de procedure stockée DecryptNoCompteBancaire
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

CREATE PROCEDURE Utilisateurs.USP_DecryptNoCompteBancaire
	@Pseudo nvarchar(50),
	@NoCompteBancaireChiffre nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	OPEN SYMMETRIC KEY MaCleSymetrique
	DECRYPTION BY CERTIFICATE MonCertificat;

	SELECT CONVERT(nvarchar(max), DECRYPTBYKEY(@NoCompteBancaireChiffre)) AS NoCompteBancaire
	FROM Utilisateurs.Utilisateur
	WHERE Pseudo = @Pseudo

	CLOSE SYMMETRIC KEY MaCleSymetrique;
END