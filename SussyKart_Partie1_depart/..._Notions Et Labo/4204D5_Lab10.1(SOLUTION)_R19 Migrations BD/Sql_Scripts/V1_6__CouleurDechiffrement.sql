
	-- Table simplissime pour retourner un nvarchar(30) avec _context.Couleurs.FromSqlRaw
	-- car on peut juste utiliser les procédures stockées pour retourner des données qui
	-- correspondent à une des tables / vues dans la BD. (On ne peut pas juste retourner une
	-- valeur scalaire)
	
	-- Il existe d'autres stratégies moins "brouillon" pour faire ça, mais celle-ci est 
	-- parmi les plus simples.
	
	CREATE TABLE Utilisateurs.Couleur(
		Couleur nvarchar(30)
	);
	GO

	-- Procédure qui retourne une couleur préférée, déchiffrée.
	-- On remarque que le SELECT avec la couleur déchiffrée fait SEMBLANT
	-- de retourner un objet de la table Couleur. C'est-à-dire qu'on retourne
	-- une seule colonne, avec le bon type (nvarchar(30)) et le bon nom, et pour
	-- Entity Framework, ça passe, car ça correspond à la table Utilisateurs.Couleur

	CREATE PROCEDURE Utilisateurs.USP_Couleur
		@Pseudo nvarchar(50),
		@MotDePasse nvarchar(50)
	AS
	BEGIN
		
		DECLARE @Sel varbinary(16);
		DECLARE @MdpHache varbinary(32);
		
		SELECT @Sel = Sel, @MdpHache = MotDePasseHache 
		FROM Utilisateurs.Utilisateur
		WHERE Pseudo = @Pseudo;
		
		-- Est-ce que c'est le bon mot de passe ?
		IF HASHBYTES('SHA2_256', CONCAT(@MotDePasse, @Sel)) = @MdpHache
		BEGIN
			-- ?
			
			-- Select qui imite la table Utilisateurs.Couleur et déchiffre la couleur préférée
			SELECT 
			-- ?
			AS Couleur
			FROM Utilisateurs.Utilisateur WHERE Pseudo = @Pseudo;
			
			-- ?
		END
		ELSE
		BEGIN
			-- Select vide si le mot de passe est mauvais
			SELECT TOP 0 * FROM Utilisateurs.Couleur;
		END
	
	END
	GO
	
	