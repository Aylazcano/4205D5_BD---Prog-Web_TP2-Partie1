USE TP2_SussyKart;
GO

-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--		Création de procedure stockée ParticipationCourse
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

CREATE PROCEDURE USP_InsertParticipationCourse
	@Position int,
	@Chrono int,
	@NbJoueurs int,
	@NomCourse nvarchar(50),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @CourseId int = (SELECT CourseID FROM Courses.Course WHERE Nom = @NomCourse);

	INSERT INTO Courses.ParticipationCourse(Position, Chrono, NbJoueurs, DateParticipation, CourseID, UtilisateurID)
	VALUES (@Position, @Chrono, @NbJoueurs, GETDATE(), @CourseId, @UserId)
END