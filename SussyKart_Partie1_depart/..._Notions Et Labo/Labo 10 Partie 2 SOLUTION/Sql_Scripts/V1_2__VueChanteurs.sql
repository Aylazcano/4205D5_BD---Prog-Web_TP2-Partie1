	-- Nouvelle vue
	
	CREATE VIEW Musique.VW_ChanteurNbChansons AS
	
	SELECT C1.ChanteurID, C1.Nom, C1.DateNaissance AS 'Date de naissance', COUNT(C2.ChansonID) AS 'Nombre de chansons'
	FROM Musique.Chanteur C1
	INNER JOIN Musique.Chanson C2
	ON C1.ChanteurID = C2.ChanteurID
	GROUP BY C1.ChanteurID, C1.Nom, C1.DateNaissance;
	
	GO
	
	-- Résultat souhaité : id du chanteur, nom du chanteur, date de naissance et son nombre de chansons
	
	-- ChanteurID | Nom | Date de naissance | Nombre de chansons
	-- -----------|-----|-------------------|-------------------