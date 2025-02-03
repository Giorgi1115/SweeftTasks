IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Teacher')
CREATE TABLE Teacher (
    TeacherId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Gender NVARCHAR(10),
    Subject NVARCHAR(100)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pupil')
CREATE TABLE Pupil (
    PupilId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Gender NVARCHAR(10),
    Class NVARCHAR(20)
);

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TeacherPupil')
CREATE TABLE TeacherPupil (
    TeacherId INT NOT NULL,
    PupilId INT NOT NULL,
    PRIMARY KEY (TeacherId, PupilId),
    FOREIGN KEY (TeacherId) REFERENCES Teacher(TeacherId),
    FOREIGN KEY (PupilId) REFERENCES Pupil(PupilId)
);

INSERT INTO Teacher (FirstName, LastName, Gender, Subject)
SELECT N'ნინო', N'ყურაშვილი', N'მდედრობითი', N'მათემატიკა'
WHERE NOT EXISTS (
    SELECT 1 FROM Teacher 
    WHERE FirstName = N'ნინო' AND LastName = N'ყურაშვილი'
);

INSERT INTO Pupil (FirstName, LastName, Gender, Class)
SELECT N'გიორგი', N'მამულაშვილი', N'მამრობითი', N'10-ა'
WHERE NOT EXISTS (
    SELECT 1 FROM Pupil 
    WHERE FirstName = N'გიორგი' AND LastName = N'მამულაშვილი'
);

INSERT INTO TeacherPupil (TeacherId, PupilId)
SELECT t.TeacherId, p.PupilId
FROM Teacher t
CROSS JOIN Pupil p
WHERE t.FirstName = N'ნინო'
  AND p.FirstName = N'გიორგი'
  AND NOT EXISTS (
    SELECT 1 FROM TeacherPupil
    WHERE TeacherId = t.TeacherId AND PupilId = p.PupilId
  );

SELECT t.TeacherId,
       t.FirstName AS TeacherFirstName,
       t.LastName AS TeacherLastName,
       t.Subject
FROM Teacher t
INNER JOIN TeacherPupil tp ON t.TeacherId = tp.TeacherId
INNER JOIN Pupil p ON tp.PupilId = p.PupilId
WHERE p.FirstName = N'გიორგი';
