
	-- Table d'utilisateurs
	
	CREATE TABLE Utilisateurs.Utilisateur(
		UtilisateurID int IDENTITY(1,1),
		Pseudo nvarchar(50) NOT NULL,
		MotDePasseHache varbinary(32) NOT NULL,
		Sel varbinary(16) NOT NULL,
		CouleurPrefere varbinary(max) NOT NULL,
		CONSTRAINT PK_Utilisateur_UtilisateurID PRIMARY KEY (UtilisateurID)
	);
	GO
	
	-- Contraintes
	
	ALTER TABLE Utilisateurs.Utilisateur ADD CONSTRAINT
	UC_Utilisateur_Pseudo UNIQUE (Pseudo);
	GO
	
	-- Créer une clé maîtresse avec un mot de passe
	CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'P4ssw0rd!etcetcetc';
	GO
	
	-- Créer un certificat auto-signé
	CREATE CERTIFICATE MonCertificat WITH SUBJECT = 'ChiffrementCouleur';
	GO
	
	-- Créer une clé symétrique
	CREATE SYMMETRIC KEY MaSuperCle WITH ALGORITHM = AES_256 ENCRYPTION BY CERTIFICATE MonCertificat;
	GO
	
	-- Procédure inscription
	CREATE PROCEDURE Utilisateurs.USP_CreerUtilisateur
		@Pseudo nvarchar(50),
		@MotDePasse nvarchar(50),
		@Couleur nvarchar(30)
	AS
	BEGIN
	
		DECLARE @Sel varbinary(16) = CRYPT_GEN_RANDOM(16);

		-- Concaténation de données et sel
		DECLARE @MdpEtSel nvarchar(116) = CONCAT(@MotDePasse, @Sel);
		
		-- Hachage du mot de passe
		DECLARE @MdpHachage varbinary(32) = HASHBYTES('SHA2_256', @MdpEtSel);

		-- Chiffrement de la couleur
		OPEN SYMMETRIC KEY MaSuperCle
		DECRYPTION BY CERTIFICATE MonCertificat;
	
		DECLARE @CouleurChiffre varbinary(max) = EncryptByKey(KEY_GUID('MaSuperCle'), @Couleur);
		
		CLOSE SYMMETRIC KEY MaSuperCle;
		
		INSERT INTO Utilisateurs.Utilisateur (Pseudo, MotDePasseHache, Sel, CouleurPrefere)
		VALUES
		(@Pseudo, @MdpHachage, @Sel, @CouleurChiffre);
	
	END
	GO
	
	-- Procédure authentification
	
	CREATE PROCEDURE Utilisateurs.USP_AuthUtilisateur
		@Pseudo nvarchar(50),
		@MotDePasse nvarchar(50)
	AS
	BEGIN
		
		DECLARE @Sel varbinary(16);
		DECLARE @MdpHache varbinary(32);
		SELECT @Sel = Sel, @MdpHache = MotDePasseHache 
		FROM Utilisateurs.Utilisateur
		WHERE Pseudo = @Pseudo;
		
		IF HASHBYTES('SHA2_256', CONCAT(@MotDePasse, @Sel)) = @MdpHache
		BEGIN
			SELECT * FROM Utilisateurs.Utilisateur WHERE Pseudo = @Pseudo;
		END
		ELSE
		BEGIN
			SELECT TOP 0 * FROM Utilisateurs.Utilisateur;
		END
	
	END
	GO
	
	-- Insertions de quelques utilisateurs (si jamais inscription ne marche pas, testez au moins la connexion / déconnexion)
	
	EXEC Utilisateurs.USP_CreerUtilisateur @Pseudo = 'max', @MotDePasse = 'Salut1!', @Couleur = 'indigo';
	GO
	
	EXEC Utilisateurs.USP_CreerUtilisateur @Pseudo = 'chantal', @MotDePasse = 'Allo2!', @Couleur = 'bourgogne';
	GO
	
	EXEC Utilisateurs.USP_CreerUtilisateur @Pseudo = 'kamalPro', @MotDePasse = 'Bonjour3!', @Couleur = 'cramoisi';
	GO
	
	
	
	
	
	