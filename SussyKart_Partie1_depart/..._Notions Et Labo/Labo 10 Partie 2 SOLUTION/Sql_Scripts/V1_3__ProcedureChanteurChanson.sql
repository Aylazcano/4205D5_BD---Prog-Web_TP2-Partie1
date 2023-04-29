
	-- Nouvelle procédure
	
	CREATE PROCEDURE Musique.USP_ChanteurChansons
	@ChanteurID int
	AS
	BEGIN
		SELECT * FROM Musique.Chanson WHERE ChanteurID = @ChanteurID;
	END
	GO