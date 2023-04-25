

	-- TABLES

	CREATE TABLE Musique.Chanteur(
		Nom nvarchar(50) NOT NULL,
		DateNaissance date NOT NULL,
		CONSTRAINT PK_Chanteur_Nom PRIMARY KEY (Nom)
	);
	GO
	
	CREATE TABLE Musique.Chanson(
		ChansonID int IDENTITY(1,1),
		Nom nvarchar(100) NOT NULL,
		NomChanteur nvarchar(50) NOT NULL,
		CONSTRAINT PK_Chanson_ChansonID PRIMARY KEY (ChansonID)
	);
	GO
	
	-- CONTRAINTES
	
	ALTER TABLE Musique.Chanson ADD CONSTRAINT FK_Chanson_NomChanteur
	FOREIGN KEY (NomChanteur) REFERENCES Musique.Chanteur(Nom)
	ON DELETE CASCADE
	ON UPDATE CASCADE;
	GO
	
	-- Insertions
	
	INSERT INTO Musique.Chanteur (Nom, DateNaissance)
	VALUES
	('The Weeknd', '1990-02-16'),
	('Dua Lipa', '1995-08-22'),
	('Post Malone', '1995-07-04'),
	('Billie Eilish', '2001-12-18'),
	('Ed Sheeran', '1991-02-17');
	GO
	
	INSERT INTO Musique.Chanson (Nom, NomChanteur)
	VALUES
	('Sacrifice', 'The Weeknd'),
	('Starboy', 'The Weeknd'),
	('Die for You', 'The Weeknd'),
	('Save Your Tears', 'The Weeknd'),
	('Love Again', 'Dua Lipa'),
	('One Kiss', 'Dua Lipa'),
	('Sunflower', 'Post Malone'),
	('Better Now', 'Post Malone'),
	('Circles', 'Post Malone'),
	('Bad Guy', 'Billie Eilish'),
	('Your Power', 'Billie Eilish'),
	('Ocean Eyes', 'Billie Eilish'),
	('Lovely', 'Billie Eilish'),
	('Happier Than Ever', 'Billie Eilish'),
	('Shape of You', 'Ed Sheeran'),
	('Perfect', 'Ed Sheeran'),
	('Photograph', 'Ed Sheeran'),
	('Shivers', 'Ed Sheeran'),
	('Bad Habits', 'Ed Sheeran'),
	('Eyes Closed', 'Ed Sheeran');
	GO