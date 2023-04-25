
	-- Nouvelle colonne pour la PK et la FK

	ALTER TABLE Musique.Chanteur
	ADD ChanteurID int IDENTITY(1,1);
	GO
	
	ALTER TABLE Musique.Chanson
	ADD ChanteurID int NULL;
	GO
	
	-- Supprimer les anciennes contraintes
	
	ALTER TABLE Musique.Chanson DROP CONSTRAINT FK_Chanson_NomChanteur;
	GO
	
	ALTER TABLE Musique.Chanteur DROP CONSTRAINT PK_Chanteur_Nom;
	GO
	
	-- Nouvelles contraintes
	
	ALTER TABLE Musique.Chanteur ADD CONSTRAINT PK_Chanteur_ChanteurID
	PRIMARY KEY (ChanteurID);
	GO
	
	ALTER TABLE Musique.Chanson ADD CONSTRAINT FK_Chanson_ChanteurID
	FOREIGN KEY (ChanteurID) REFERENCES Musique.Chanteur(ChanteurID)
	ON DELETE CASCADE
	ON UPDATE CASCADE;
	GO
	
	-- Remplir la colonne FK
	
	UPDATE Musique.Chanson
	SET ChanteurID = (SELECT C.ChanteurID FROM Musique.Chanteur C WHERE C.Nom = NomChanteur);
	GO
	
	-- Supprimer l'ancienne colonne FK (On veut garder le nom des chanteurs, donc on ne supprime pas l'ancienne PK !)
	
	ALTER TABLE Musique.Chanson DROP COLUMN NomChanteur;
	