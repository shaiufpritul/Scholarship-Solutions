Create Table [dbo].[Student](
     StudentID int IDENTITY(1,1) PRIMARY KEY,
	 FirstName varchar (50) NOT NULL,
	 LastName varchar (50) NOT NULL,
	 StudentPicture varbinary(max),
	 Department varchar (25) NOT NULL,
	 Session varchar(10),
	 Year int NOT NULL CHECK(Year<5),
	 Semester int NULL CHECK(Semester<3),
	 Email varchar (50) NOT NULL UNIQUE,
	 AverageGrade float NOT NULL,
	 CompletedCredit float NOT NULL,
	 ExtracurricularActivities varchar(50),
	 Blog varchar(50),
	 Reference varchar (50),
	 Picture varchar(500),
	 Password varchar(20)
)
--alter table Student 
--alter column StudentPicture varchar(500)

Create Table [dbo].[Interest](
    InterestID int IDENTITY(1,1) PRIMARY KEY,
    TopicName varchar(50) Not null
)

Create Table [dbo].[Expertise](
     ExpertiseID int IDENTITY(1,1) PRIMARY KEY,
	 TopicName varchar(50)
)

Create Table [dbo].[StudentInterest](
    StudentInterestID int IDENTITY(1,1) PRIMARY KEY,
    StudentId int NOT NULL FOREIGN KEY REFERENCES Student(StudentID),
	InterestID int NOT NULL FOREIGN KEY REFERENCES Interest(InterestID),
    Reason nvarchar (300)
)

Create Table [dbo].[StudentExpertise](
    StudentExpertiseID int IDENTITY(1,1) PRIMARY KEY,
    StudentId int NOT NULL FOREIGN KEY REFERENCES Student(StudentID),
	ExpertiseID int NOT NULL FOREIGN KEY REFERENCES Expertise(ExpertiseID),
    Certificate varbinary(max),
	Institution varchar(70) NOT NULL
)

--Alter Table StudentExpertise DROP COLUMN Certificate

Create Table [dbo].[Course](
     CourseID int IDENTITY(1,1) PRIMARY KEY,
	 CourseName varchar (50) NOT NULL,
	 Year int NOT NULL CHECK(Year<5),
	 Semester int NULL CHECK(Semester<3)
)

Create Table [dbo].[StudentCourseResult](
     StudentCourseResultID int IDENTITY(1,1) PRIMARY KEY,
     CourseID int FOREIGN KEY REFERENCES Course(CourseID),
     StudentId int FOREIGN KEY REFERENCES Student(StudentID),
     Grade float NOT NULL,
)

Create Table [dbo].[University]
(
     UniID int IDENTITY(1,1) PRIMARY KEY,
     UniName varchar(50),
     UniRating float,
	 UniPicture varchar(500),
     City varchar(50),
	 Country varchar(50)
)

--select StudentCourseResult.* ,  Course.*, Student.*  from StudentCourseResult FULL JOIN Student ON StudentCourseResult.StudentId = Student.StudentID FULL JOIN Course ON Course.CourseID = StudentCourseResult.CourseID WHERE StudentCourseResult.StudentId = 1
--select StudentExpertise.* ,  Expertise.* from StudentExpertise FULL JOIN Expertise ON StudentExpertise.ExpertiseID = Expertise.ExpertiseID WHERE StudentExpertise.StudentId = 3
--select StudentInterest.* ,  Interest.* from StudentInterest FULL JOIN Interest ON StudentInterest.InterestID = Interest.InterestID WHERE StudentInterest.StudentId = 3

--alter table University 
--alter column UniPicture varchar(500)

Create table [dbo].[Professor](
     ProfID int IDENTITY(1,1) PRIMARY KEY,
	 Name varchar (50) NOT NULL,
	 Email varchar(50),
	 EducationalBackground varchar(200) NOT NULL,
	 NoOfStudents int,
	 Funding varchar(20),
	 Password varchar(20),
	 Picture varchar(500),
	 Department varchar(50),
	 UniID int foreign key references University(UniId),
	 InterestID int NOT NULL FOREIGN KEY REFERENCES Interest(InterestID),
	 ExpertiseID int NOT NULL FOREIGN KEY REFERENCES Expertise(ExpertiseID)
)

--alter table Professor 
--alter column Picture varchar(500)

Create table [dbo].[Scholarship](
     ScholarshipID int IDENTITY(1, 2) PRIMARY KEY,
	 DegreeName varchar(100) NOT NULL,
	 UniID int foreign key references University(UniID),
	 ProfID int foreign key references Professor(ProfID),
	 Subject varchar(100) NOT NULL,
	 PercentageOfScholarship float NOT NULL,
	 Session varchar(100) NOT NULL,
	 Seats int NOT NULL,
	 MinimumGPA varchar(50) NOT NULL,
	 OtherRequirements varchar(300),
	 LastDate date
) 


Create table [dbo].[StudentMessage](
     MessageId int Identity(1,1) Primary Key,
     StudentId int Foreign Key References Student(StudentID) NOT NULL,
	 TextMessage varchar(200) NOT NULL,
	 ProfId int Foreign Key References Professor(ProfID) NOT NULL
)

Create table [dbo].[StudentResearchPaper](
     DOI int Primary Key,
	 Title varchar(50) NOT NULL,
	 StudentID int FOREIGN KEY REFERENCES Student(StudentID),
	 PageNO int NOT NULL,
	 Volume varchar(10),
	 PublicationDate date NOT NULL,
	 Publisher varchar(50),  
	 Citation int,
	 Link varchar(max)
)

Create table [dbo].[ProfessorResearchPaper](
     DOI int Primary Key,
	 Title varchar(200) NOT NULL,
	 ProfID int NOT NULL foreign key references Professor(ProfID),
	 PageNO int NOT NULL,
	 Volume varchar(10),
	 PublicationDate date NOT NULL,
	 Publisher varchar(50),
	 Citation int,
	 Link varchar(max)
)
--Select ProfessorResearchPaper.*, Professor.Name from ProfessorResearchPaper JOIN Professor ON ProfessorResearchPaper.ProfID = Professor.ProfID WHERE Professor.ProfID = 5
--select Scholarship.* ,  Professor.*, University.*  from Scholarship FULL JOIN Professor ON Scholarship.ProfId = Professor.ProfId FULL JOIN University ON University.UniID = Scholarship.UniID WHERE Scholarship.ProfID =  5


Create table [dbo].[StudentScholarship](
	StudentScholarshipID int Identity(1,1) Primary Key,
	ScholarshipID int foreign key references Scholarship(ScholarshipID),
	StudentID int foreign key references Student(StudentID),
	Status varchar(50) 
)
--Insert INTO StudentScholarship VALUES
--( 1, 1, 'Waiting')
--Insert INTO StudentScholarship VALUES
--( 3, 2, 'Waiting')

--Select * from Student
--Select * from Professor
--Select * from Scholarship
--Alter Table StudentExpertise 
--Alter Column Certificate varchar(500)

--Select FirstName from Student INNER JOIN StudentScholarship ON Student.StudentID = StudentScholarship.StudentID INNER JOIN Scholarship ON StudentScholarship.ScholarshipID = Scholarship.ScholarshipID WHERE Scholarship.ProfID = 3
