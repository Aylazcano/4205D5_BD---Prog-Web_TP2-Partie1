USE TP2_SussyKart;
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--	Ajout de tables, master key, certificat et clé symétrique
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

-- Ajout de tables
ALTER TABLE Utilisateurs.Utilisateur
ADD 
MotDePasseHache varbinary(32) NULL,
MdpSel varbinary(16) NULL,
NoCompteBancaireHache nvarchar(max) NULL;
GO

-- Création d'une Master Key
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'M0tD3P@sse!';
GO

-- Création de certificat
CREATE CERTIFICATE MonCertificat WITH SUBJECT = 'ChiffrementNoCompteBancaire';
GO

-- Création de clé symetrique
CREATE SYMMETRIC KEY MaCleSymetrique WITH ALGORITHM = AES_256 ENCRYPTION BY CERTIFICATE MonCertificat;
GO


-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--	Remplir les tables des utilisateurs existants
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

UPDATE Utilisateurs.Utilisateur
SET MotDePasseHache = N'patate', NoCompteBancaire = N'123456789'
WHERE UtilisateurID <= 8000;
GO


-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Création et Modification de procédures
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

-- Modification de la procedure d'inscription
ALTER PROCEDURE Utilisateurs.USP_CreerUtilisateur
	@Pseudo nvarchar(30),
	@Courriel nvarchar(230),
	@NoCompteBancaire nvarchar(100),
	@MotDePasse nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	-- Sels aléatoire
	DECLARE @MdpSel varbinary(16) = CRYPT_GEN_RANDOM(16);

	-- Concaténation de données et sel
	DECLARE @MdpEtSel nvarchar(116) = CONCAT(@MotDePasse, @MdpSel);

	-- Hachage du mot de passe
	DECLARE @MotDePasseHache varbinary(32) = HASHBYTES('SHA2_256', @MdpEtSel);

	-- Chiffrement du numero de compte bancaire
	OPEN SYMMETRIC KEY MaCleSymetrique
	DECRYPTION BY CERTIFICATE MonCertificat;

	DECLARE @NoCompteBancaireChiffre varbinary(max) = EncryptByKey(KEY_GUID('MaCleSymetrique'), @NoCompteBancaire)
	CLOSE SYMMETRIC KEY MaCleSymetrique;
	
	-- Insertion
	INSERT INTO Utilisateurs.Utilisateur (Pseudo, DateInscription, Courriel, MotDePasseHache, MdpSel, NoCompteBancaire, EstSuppr)
	VALUES (@Pseudo, GETDATE(), @Courriel, @MotDePasseHache, @MdpSel, @NoCompteBancaireChiffre, 0);
END
GO

-- Procédure d'authentification	
CREATE PROCEDURE Utilisateurs.USP_AuthUtilisateur
	@Pseudo nvarchar(50),
	@MotDePasse nvarchar(100)
AS
BEGIN	
	DECLARE @Sel varbinary(16);
	DECLARE @MdpHache varbinary(32);
	SELECT @Sel = MdpSel, @MdpHache = MotDePasseHache
	FROM Utilisateurs.Utilisateur
	WHERE Pseudo = @Pseudo; -- Si pseudos uniques
		
	IF HASHBYTES ('SHA256', CONCAT(@MotDePasse, @Sel)) = @MdpHache
	BEGIN
		SELECT 1 AS 'MdpEstValide'; -- true (valide)
	END
	ELSE
	BEGIN
		SELECT 0 AS 'MdpEstInvalide'; -- false (invalide)
	END
END
GO