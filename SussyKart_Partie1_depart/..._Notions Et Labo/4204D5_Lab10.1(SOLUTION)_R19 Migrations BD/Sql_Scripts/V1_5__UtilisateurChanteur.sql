
	-- Nouvelle table de liaison pour les chanteurs favoris des utilisateurs
	
	CREATE TABLE Musique.ChanteurFavori(
		ChanteurFavoriID int IDENTITY(1,1),
		ChanteurID int NOT NULL,
		UtilisateurID int NOT NULL,
		CONSTRAINT PK_ChanteurFavori_ChanteurFavoriID PRIMARY KEY (ChanteurFavoriID)
	);
	GO
	
	-- Contraintes pour les deux FK
	
	ALTER TABLE Musique.ChanteurFavori ADD CONSTRAINT FK_ChanteurFavori_ChanteurID
	FOREIGN KEY (ChanteurID) REFERENCES Musique.Chanteur(ChanteurID)
	ON DELETE CASCADE
	ON UPDATE CASCADE;
	GO
	
	ALTER TABLE Musique.ChanteurFavori ADD CONSTRAINT FK_ChanteurFavori_UtilisateurID
	FOREIGN KEY (UtilisateurID) REFERENCES Utilisateurs.Utilisateur(UtilisateurID)
	ON DELETE CASCADE
	ON UPDATE CASCADE;
	GO
	
	-- Contrainte pour que chaque paire ChanteurID, UtilisateurID soit unique
	
	ALTER TABLE Musique.ChanteurFavori ADD CONSTRAINT UC_ChanteurFavori_ChanteurUtilisateur
	UNIQUE (ChanteurID, UtilisateurID);
	GO