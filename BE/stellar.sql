DROP TABLE [dbo].[course];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[course] (
    [id] int,
    [code] varchar(255),
    [name] varchar(255),
    [description] varchar(255),
    [units] int,
    [hours] int,
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[course_outline];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[course_outline] (
    [id] int,
    [program_course_id] int,
    [academic_year] varchar(255),
    [pre_requisites] text,
    [co_requisites] text,
    [student_assessment] text,
    [passing_grade] varchar(255),
    [plar_method] text,
    [instructor_id] int,
    [prepared_by_user_id] int,
    [prepared_date] date,
    [approved_by_program_head_user_id] int,
    [approved_by_program_head_date] date,
    [approved_by_academic_chair_user_id] int,
    [approved_by_academic_chair_date] date,
    [program_head_approval] varchar(255),
    [academic_chair_approval] varchar(255),
    CONSTRAINT [FK__course_ou__progr__49C3F6B7] FOREIGN KEY ([program_course_id]) REFERENCES [dbo].[program_course]([id]),
    CONSTRAINT [FK__course_ou__instr__4AB81AF0] FOREIGN KEY ([instructor_id]) REFERENCES [dbo].[user]([id]),
    CONSTRAINT [FK__course_ou__prepa__4BAC3F29] FOREIGN KEY ([prepared_by_user_id]) REFERENCES [dbo].[user]([id]),
    CONSTRAINT [FK__course_ou__appro__4CA06362] FOREIGN KEY ([approved_by_program_head_user_id]) REFERENCES [dbo].[user]([id]),
    CONSTRAINT [FK__course_ou__appro__4D94879B] FOREIGN KEY ([approved_by_academic_chair_user_id]) REFERENCES [dbo].[user]([id]),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[degree];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[degree] (
    [id] int,
    [name] varchar(255),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[learning_outcome];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[learning_outcome] (
    [id] int,
    [outcome_text] varchar(255),
    [course_id] int,
    [learning_activities] text,
    CONSTRAINT [FK__learning___cours__440B1D61] FOREIGN KEY ([course_id]) REFERENCES [dbo].[course]([id]),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[learning_step];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[learning_step] (
    [id] int,
    [learning_text] varchar(255),
    [learning_outcome_id] int,
    CONSTRAINT [FK__learning___learn__46E78A0C] FOREIGN KEY ([learning_outcome_id]) REFERENCES [dbo].[learning_outcome]([id]),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[program];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[program] (
    [id] int,
    [name] varchar(255),
    [description] varchar(255),
    [school_id] int,
    [degree_id] int,
    CONSTRAINT [FK_ProgramSchool] FOREIGN KEY ([school_id]) REFERENCES [dbo].[school]([id]),
    CONSTRAINT [FK_ProgramDegree] FOREIGN KEY ([degree_id]) REFERENCES [dbo].[degree]([id]),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[program_course];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[program_course] (
    [id] int,
    [program_id] int,
    [course_id] int,
    CONSTRAINT [FK__program_c__progr__403A8C7D] FOREIGN KEY ([program_id]) REFERENCES [dbo].[program]([id]),
    CONSTRAINT [FK__program_c__cours__412EB0B6] FOREIGN KEY ([course_id]) REFERENCES [dbo].[course]([id]),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[role];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[role] (
    [id] int,
    [name] varchar(255),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[school];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[school] (
    [id] int,
    [name] varchar(255),
    PRIMARY KEY ([id])
);

DROP TABLE [dbo].[user];
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: sequences, indices, triggers. Do not use it as a backup.

CREATE TABLE [dbo].[user] (
    [id] int,
    [name] varchar(255),
    [role_id] int,
    [password] varchar(255),
    CONSTRAINT [FK__user__role_id__398D8EEE] FOREIGN KEY ([role_id]) REFERENCES [dbo].[role]([id]),
    PRIMARY KEY ([id])
);

INSERT INTO [dbo].[course] ([id],[code],[name],[description],[units],[hours]) VALUES (1,'CS101','Introduction to Programming','Introduction to basic programming concepts',3,45),(2,'BA501','Financial Accounting','Principles of financial accounting',4,60),(3,'EE201','Circuit Analysis','Analysis of electrical circuits',3,45),(4,'CS201','Data Structures','Study of common data structures',4,60),(5,'BA601','Marketing Management','Principles of marketing management',4,60);
INSERT INTO [dbo].[course_outline] ([id],[program_course_id],[academic_year],[pre_requisites],[co_requisites],[student_assessment],[passing_grade],[plar_method],[instructor_id],[prepared_by_user_id],[prepared_date],[approved_by_program_head_user_id],[approved_by_program_head_date],[approved_by_academic_chair_user_id],[approved_by_academic_chair_date],[program_head_approval],[academic_chair_approval]) VALUES (1,1,'2023-2024',NULL,NULL,'Assignments, exams','60%',NULL,1,1,'2023-08-15',2,NULL,3,NULL,'Approved','Pending'),(2,2,'2023-2024',NULL,NULL,'Case studies, presentations','70%',NULL,5,5,'2023-08-16',2,NULL,3,NULL,'Approved','Rejected'),(3,3,'2023-2024',NULL,NULL,'Lab reports, exams','65%',NULL,1,1,'2023-08-17',2,NULL,3,NULL,'Approved','Approved'),(4,4,'2023-2024','CS101',NULL,'Coding projects, assignments','60%',NULL,1,1,'2023-08-18',2,NULL,3,NULL,'Pending','Pending'),(5,5,'2023-2024','BA501',NULL,'Group projects, presentations','70%',NULL,5,5,'2023-08-19',2,NULL,3,NULL,'Rejected','Pending');
INSERT INTO [dbo].[degree] ([id],[name]) VALUES (1,'Diploma'),(2,'Post-Graduate Certificate'),(3,'Certificate'),(4,'Bachelor');
INSERT INTO [dbo].[learning_outcome] ([id],[outcome_text],[course_id],[learning_activities]) VALUES (1,'Understand basic programming syntax',1,'Lectures, coding exercises'),(2,'Apply financial accounting principles',2,'Case studies, problem-solving'),(3,'Analyze electrical circuits',3,'Lab experiments, simulations'),(4,'Implement common data structures',4,'Coding projects, assignments'),(5,'Develop marketing strategies',5,'Group projects, presentations');
INSERT INTO [dbo].[learning_step] ([id],[learning_text],[learning_outcome_id]) VALUES (1,'Learn variables and data types',1),(2,'Understand control flow statements',1),(3,'Prepare financial statements',2),(4,'Calculate circuit parameters',3),(5,'Implement linked lists',4),(6,'Design a marketing campaign',5);
INSERT INTO [dbo].[program] ([id],[name],[description],[school_id],[degree_id]) VALUES (1,'Computer Science','Undergraduate program in computer science',1,1),(2,'Business Administration','Graduate program in business administration',2,3),(3,'Electrical Engineering','Undergraduate program in electrical engineering',1,2);
INSERT INTO [dbo].[program_course] ([id],[program_id],[course_id]) VALUES (1,1,1),(2,2,2),(3,3,3),(4,1,4),(5,2,5);
INSERT INTO [dbo].[role] ([id],[name]) VALUES (1,'Instructor'),(2,'Program Head'),(3,'Academic Chair'),(4,'Student');
INSERT INTO [dbo].[school] ([id],[name]) VALUES (1,'Computing and Digital Innovation'),(2,'Creative Media, Arts and Science');
INSERT INTO [dbo].[user] ([id],[name],[role_id],[password]) VALUES (1,'John Smith',1,'1234'),(2,'Jane Doe',2,'1234'),(3,'David Lee',3,'1234'),(4,'Emily White',4,'1234'),(5,'Michael Brown',1,'1234'),(6,'Sarah Jones',4,'1234');
