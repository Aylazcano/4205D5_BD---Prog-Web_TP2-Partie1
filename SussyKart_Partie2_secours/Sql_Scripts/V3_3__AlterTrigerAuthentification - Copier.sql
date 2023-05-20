-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--	Modification de la procédure d'autentification
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

-- Procédure d'authentification
ALTER PROCEDURE Utilisateurs.USP_AuthUtilisateur
	@Pseudo nvarchar(50),
	@MotDePasse nvarchar(100)
AS
BEGIN
	DECLARE @Sel varbinary(16);
	DECLARE @MdpHache varbinary(32);
	DECLARE @EstSuppr bit;

	SELECT @Sel = MdpSel, @MdpHache = MotDePasseHache, @EstSuppr = EstSuppr
	FROM Utilisateurs.Utilisateur
	WHERE Pseudo = @Pseudo; -- Si les pseudos sont uniques !

	IF @EstSuppr = 1
	BEGIN
		SELECT TOP 0 * FROM Utilisateurs.Utilisateur; -- On retourne rien si l'utilisateur est supprimé
		RETURN; -- Optimisation
	END

	IF HASHBYTES('SHA2_256', CONCAT(@MotDePasse, @Sel)) = @MdpHache
	BEGIN
		-- On retourne l'utilisateur si le mot de passe est valide
		SELECT * FROM Utilisateurs.Utilisateur WHERE Pseudo = @Pseudo;
		RETURN; -- Optimisation
	END

	SELECT TOP 0 * FROM Utilisateurs.Utilisateur; -- On retourne rien si mot de passe invalide
END
GO






