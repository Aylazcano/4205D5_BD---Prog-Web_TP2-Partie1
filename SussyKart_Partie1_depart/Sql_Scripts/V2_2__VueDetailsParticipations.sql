USE TP2_SussyKart;
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Création de vue Participations
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

CREATE VIEW Courses.VW_DetailsParticipations 
AS
SELECT U.Pseudo AS Joueur, C.Nom AS Course, PC.NbJoueurs, PC.Position, PC.Chrono, PC.DateParticipation
FROM Courses.ParticipationCourse PC
INNER JOIN Courses.Course C ON PC.CourseID = C.CourseID
INNER JOIN Utilisateurs.Utilisateur U ON PC.UtilisateurID = U.UtilisateurID
GO