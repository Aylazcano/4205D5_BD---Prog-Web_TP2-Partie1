
	-- Table d'utilisateurs
	
	CREATE TABLE Utilisateurs.Utilisateur(
		UtilisateurID int IDENTITY(1,1),
		Pseudo nvarchar(50) NOT NULL,
		MotDePasseHache nvarchar(50) NOT NULL,
		Sel varbinary(16) NOT NULL,
		CouleurPrefere nvarchar(30) NOT NULL,
		CONSTRAINT PK_Utilisateur_UtilisateurID PRIMARY KEY (UtilisateurID)
	);
	GO
	
	-- Contraintes
	
	ALTER TABLE Utilisateurs.Utilisateur ADD CONSTRAINT
	UC_Utilisateur_Pseudo UNIQUE (Pseudo);
	GO
	
	-- Créer une clé maîtresse avec un mot de passe
	-- ?
	
	-- Créer un certificat auto-signé
	-- ?
	
	-- Créer une clé symétrique
	-- ?
	
	-- Procédure inscription
	CREATE PROCEDURE Utilisateurs.USP_CreerUtilisateur
		@Pseudo nvarchar(50),
		@MotDePasse nvarchar(50),
		@Couleur nvarchar(30)
	AS
	BEGIN
	
		DECLARE @Sel varbinary(16) = CRYPT_GEN_RANDOM(16);
		
		INSERT INTO Utilisateurs.Utilisateur (Pseudo, MotDePasseHache, Sel, CouleurPrefere)
		VALUES
		(@Pseudo, @MotDePasse, @Sel, @Couleur);
	
	END
	GO
	
	-- Procédure authentification
	
	CREATE PROCEDURE Utilisateurs.USP_AuthUtilisateur
		@Pseudo nvarchar(50),
		@MotDePasse nvarchar(50)
	AS
	BEGIN
		
		DECLARE @Mdp nvarchar(50);
		SELECT @Mdp = MotDePasseHache
		FROM Utilisateurs.Utilisateur
		WHERE Pseudo = @Pseudo;
		
		IF HASHBYTES @MotDePasse = @Mdp
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
	
	
	
	
	
	