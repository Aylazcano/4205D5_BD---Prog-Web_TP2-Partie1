USE TP2_SussyKart;
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Création de procedure stockée ParticipationCourse
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

CREATE PROCEDURE USP_InsertParticipationCourse
	@Position int,
	@Chrono int,
	@NbJoueurs int,
	@CourseId int,
	@UserId int
AS
BEGIN
	INSERT INTO Courses.ParticipationCourse(Position, Chrono, NbJoueurs, DateParticipation, CourseID, UtilisateurID)
	VALUES (@Position, @Chrono, @NbJoueurs, GETDATE(), @CourseId, @UserId)
END