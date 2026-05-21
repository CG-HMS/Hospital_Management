/* =========================================================
   Create Database
   ========================================================= */
CREATE DATABASE hms;
GO

USE hms;
GO

/* =========================================================
   DROP TABLES (in dependency order)
   ========================================================= */

DROP TABLE IF EXISTS Undergoes;
DROP TABLE IF EXISTS Stay;
DROP TABLE IF EXISTS On_Call;
DROP TABLE IF EXISTS Room;
DROP TABLE IF EXISTS Block;
DROP TABLE IF EXISTS Prescribes;
DROP TABLE IF EXISTS Medication;
DROP TABLE IF EXISTS Appointment;
DROP TABLE IF EXISTS Nurse;
DROP TABLE IF EXISTS Patient;
DROP TABLE IF EXISTS Trained_In;
DROP TABLE IF EXISTS Procedures;
DROP TABLE IF EXISTS Affiliated_With;
DROP TABLE IF EXISTS Department;
DROP TABLE IF EXISTS Physician;
GO

/* =========================================================
   Physician
   ========================================================= */
CREATE TABLE Physician (
    EmployeeID INT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Position VARCHAR(30) NOT NULL,
    SSN INT NOT NULL,
    CONSTRAINT pk_physician PRIMARY KEY (EmployeeID)
);
GO

/* =========================================================
   Department
   ========================================================= */
CREATE TABLE Department (
    DepartmentID INT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Head INT NOT NULL,
    CONSTRAINT pk_Department PRIMARY KEY (DepartmentID),
    CONSTRAINT fk_Department_Physician_EmployeeID
        FOREIGN KEY (Head) REFERENCES Physician(EmployeeID)
);
GO

/* =========================================================
   Affiliated_With
   ========================================================= */
CREATE TABLE Affiliated_With (
    Physician INT NOT NULL,
    Department INT NOT NULL,
    PrimaryAffiliation BIT NOT NULL,
    CONSTRAINT fk_Affiliated_With_Physician_EmployeeID
        FOREIGN KEY (Physician) REFERENCES Physician(EmployeeID),
    CONSTRAINT fk_Affiliated_With_Department_DepartmentID
        FOREIGN KEY (Department) REFERENCES Department(DepartmentID),
    CONSTRAINT pk_Affiliated_With PRIMARY KEY (Physician, Department)
);
GO

/* =========================================================
   Procedures
   ========================================================= */
CREATE TABLE Procedures (
    Code INT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Cost REAL NOT NULL,
    CONSTRAINT pk_Procedures PRIMARY KEY (Code)
);
GO

/* =========================================================
   Trained_In
   ========================================================= */
CREATE TABLE Trained_In (
    Physician INT NOT NULL,
    Treatment INT NOT NULL,
    CertificationDate DATETIME NOT NULL,
    CertificationExpires DATETIME NOT NULL,
    CONSTRAINT fk_Trained_In_Physician_EmployeeID
        FOREIGN KEY (Physician) REFERENCES Physician(EmployeeID),
    CONSTRAINT fk_Trained_In_Procedures_Code
        FOREIGN KEY (Treatment) REFERENCES Procedures(Code),
    CONSTRAINT pk_Trained_In PRIMARY KEY (Physician, Treatment)
);
GO

/* =========================================================
   Patient
   ========================================================= */
CREATE TABLE Patient (
    SSN INT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Address VARCHAR(30) NOT NULL,
    Phone VARCHAR(30) NOT NULL,
    InsuranceID INT NOT NULL,
    PCP INT NOT NULL,
    CONSTRAINT pk_Patient PRIMARY KEY (SSN),
    CONSTRAINT fk_Patient_Physician_EmployeeID
        FOREIGN KEY (PCP) REFERENCES Physician(EmployeeID)
);
GO

/* =========================================================
   Nurse
   ========================================================= */
CREATE TABLE Nurse (
    EmployeeID INT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Position VARCHAR(30) NOT NULL,
    Registered BIT NOT NULL,
    SSN INT NOT NULL,
    CONSTRAINT pk_Nurse PRIMARY KEY (EmployeeID)
);
GO

/* =========================================================
   Appointment
   ========================================================= */
CREATE TABLE Appointment (
    AppointmentID INT NOT NULL,
    Patient INT NOT NULL,
    PrepNurse INT NULL,
    Physician INT NOT NULL,
    Starto DATETIME NOT NULL,
    Endo DATETIME NOT NULL,
    ExaminationRoom VARCHAR(MAX) NOT NULL,
    CONSTRAINT pk_Appointment PRIMARY KEY (AppointmentID),
    CONSTRAINT fk_Appointment_Patient_SSN
        FOREIGN KEY (Patient) REFERENCES Patient(SSN),
    CONSTRAINT fk_Appointment_Nurse_EmployeeID
        FOREIGN KEY (PrepNurse) REFERENCES Nurse(EmployeeID),
    CONSTRAINT fk_Appointment_Physician_EmployeeID
        FOREIGN KEY (Physician) REFERENCES Physician(EmployeeID)
);
GO

/* =========================================================
   Medication
   ========================================================= */
CREATE TABLE Medication (
    Code INT NOT NULL,
    Name VARCHAR(30) NOT NULL,
    Brand VARCHAR(30) NOT NULL,
    Description VARCHAR(30) NOT NULL,
    CONSTRAINT pk_Medication PRIMARY KEY (Code)
);
GO

/* =========================================================
   Prescribes
   ========================================================= */
CREATE TABLE Prescribes (
    Physician INT NOT NULL,
    Patient INT NOT NULL,
    Medication INT NOT NULL,
    Date DATETIME NOT NULL,
    Appointment INT NULL,
    Dose VARCHAR(30) NOT NULL,
    CONSTRAINT pk_Prescribes PRIMARY KEY
        (Physician, Patient, Medication, Date),
    CONSTRAINT fk_Prescribes_Physician_EmployeeID
        FOREIGN KEY (Physician) REFERENCES Physician(EmployeeID),
    CONSTRAINT fk_Prescribes_Patient_SSN
        FOREIGN KEY (Patient) REFERENCES Patient(SSN),
    CONSTRAINT fk_Prescribes_Medication_Code
        FOREIGN KEY (Medication) REFERENCES Medication(Code),
    CONSTRAINT fk_Prescribes_Appointment_AppointmentID
        FOREIGN KEY (Appointment) REFERENCES Appointment(AppointmentID)
);
GO

/* =========================================================
   Block
   ========================================================= */
CREATE TABLE Block (
    BlockFloor INT NOT NULL,
    BlockCode INT NOT NULL,
    CONSTRAINT pk_Block PRIMARY KEY (BlockFloor, BlockCode)
);
GO

/* =========================================================
   Room
   ========================================================= */
CREATE TABLE Room (
    RoomNumber INT NOT NULL,
    RoomType VARCHAR(30) NOT NULL,
    BlockFloor INT NOT NULL,
    BlockCode INT NOT NULL,
    Unavailable BIT NOT NULL,
    CONSTRAINT pk_Room PRIMARY KEY (RoomNumber),
    CONSTRAINT fk_Room_Block_PK
        FOREIGN KEY (BlockFloor, BlockCode)
        REFERENCES Block(BlockFloor, BlockCode)
);
GO

/* =========================================================
   On_Call
   ========================================================= */
CREATE TABLE On_Call (
    Nurse INT NOT NULL,
    BlockFloor INT NOT NULL,
    BlockCode INT NOT NULL,
    OnCallStart DATETIME NOT NULL,
    OnCallEnd DATETIME NOT NULL,
    CONSTRAINT pk_On_Call PRIMARY KEY
        (Nurse, BlockFloor, BlockCode, OnCallStart, OnCallEnd),
    CONSTRAINT fk_OnCall_Nurse_EmployeeID
        FOREIGN KEY (Nurse) REFERENCES Nurse(EmployeeID),
    CONSTRAINT fk_OnCall_Block
        FOREIGN KEY (BlockFloor, BlockCode)
        REFERENCES Block(BlockFloor, BlockCode)
);
GO

/* =========================================================
   Stay
   ========================================================= */
CREATE TABLE Stay (
    StayID INT NOT NULL,
    Patient INT NOT NULL,
    Room INT NOT NULL,
    StayStart DATETIME NOT NULL,
    StayEnd DATETIME NOT NULL,
    CONSTRAINT pk_Stay PRIMARY KEY (StayID),
    CONSTRAINT fk_Stay_Patient_SSN
        FOREIGN KEY (Patient) REFERENCES Patient(SSN),
    CONSTRAINT fk_Stay_Room_Number
        FOREIGN KEY (Room) REFERENCES Room(RoomNumber)
);
GO

/* =========================================================
   Undergoes
   ========================================================= */
CREATE TABLE Undergoes (
    Patient INT NOT NULL,
    Procedures INT NOT NULL,
    Stay INT NOT NULL,
    DateUndergoes DATETIME NOT NULL,
    Physician INT NOT NULL,
    AssistingNurse INT NULL,
    CONSTRAINT pk_Undergoes PRIMARY KEY
        (Patient, Procedures, Stay, DateUndergoes),
    CONSTRAINT fk_Undergoes_Patient_SSN
        FOREIGN KEY (Patient) REFERENCES Patient(SSN),
    CONSTRAINT fk_Undergoes_Procedures_Code
        FOREIGN KEY (Procedures) REFERENCES Procedures(Code),
    CONSTRAINT fk_Undergoes_Stay_StayID
        FOREIGN KEY (Stay) REFERENCES Stay(StayID),
    CONSTRAINT fk_Undergoes_Physician_EmployeeID
        FOREIGN KEY (Physician) REFERENCES Physician(EmployeeID),
    CONSTRAINT fk_Undergoes_Nurse_EmployeeID
        FOREIGN KEY (AssistingNurse) REFERENCES Nurse(EmployeeID)
);
GO

/* ===============================
   Physician
   =============================== */
INSERT INTO Physician VALUES (1,'John Dorian','Staff Internist',111111111);
INSERT INTO Physician VALUES (2,'Elliot Reid','Attending Physician',222222222);
INSERT INTO Physician VALUES (3,'Christopher Turk','Surgical Attending Physician',333333333);
INSERT INTO Physician VALUES (4,'Percival Cox','Senior Attending Physician',444444444);
INSERT INTO Physician VALUES (5,'Bob Kelso','Head Chief of Medicine',555555555);
INSERT INTO Physician VALUES (6,'Todd Quinlan','Surgical Attending Physician',666666666);
INSERT INTO Physician VALUES (7,'John Wen','Surgical Attending Physician',777777777);
INSERT INTO Physician VALUES (8,'Keith Dudemeister','MD Resident',888888888);
INSERT INTO Physician VALUES (9,'Molly Clock','Attending Psychiatrist',999999999);

/* ===============================
   Department
   =============================== */
INSERT INTO Department VALUES (1,'General Medicine',4);
INSERT INTO Department VALUES (2,'Surgery',7);
INSERT INTO Department VALUES (3,'Psychiatry',9);

/* ===============================
   Affiliated_With
   =============================== */
INSERT INTO Affiliated_With VALUES (1,1,1);
INSERT INTO Affiliated_With VALUES (2,1,1);
INSERT INTO Affiliated_With VALUES (3,1,0);
INSERT INTO Affiliated_With VALUES (3,2,1);
INSERT INTO Affiliated_With VALUES (4,1,1);
INSERT INTO Affiliated_With VALUES (5,1,1);
INSERT INTO Affiliated_With VALUES (6,2,1);
INSERT INTO Affiliated_With VALUES (7,1,0);
INSERT INTO Affiliated_With VALUES (7,2,1);
INSERT INTO Affiliated_With VALUES (8,1,1);
INSERT INTO Affiliated_With VALUES (9,3,1);

/* ===============================
   Procedures
   =============================== */
INSERT INTO Procedures VALUES (1,'Reverse Rhinopodoplasty',1500.0);
INSERT INTO Procedures VALUES (2,'Obtuse Pyloric Recombobulation',3750.0);
INSERT INTO Procedures VALUES (3,'Folded Demiophtalmectomy',4500.0);
INSERT INTO Procedures VALUES (4,'Complete Walletectomy',10000.0);
INSERT INTO Procedures VALUES (5,'Obfuscated Dermogastrotomy',4899.0);
INSERT INTO Procedures VALUES (6,'Reversible Pancreomyoplasty',5600.0);
INSERT INTO Procedures VALUES (7,'Follicular Demiectomy',25.0);

/* ===============================
   Patient
   =============================== */
INSERT INTO Patient VALUES (100000001,'John Smith','42 Foobar Lane','555-0256',68476213,1);
INSERT INTO Patient VALUES (100000002,'Grace Ritchie','37 Snafu Drive','555-0512',36546321,2);
INSERT INTO Patient VALUES (100000003,'Random J. Patient','101 Omgbbq Street','555-1204',65465421,2);
INSERT INTO Patient VALUES (100000004,'Dennis Doe','1100 Foobaz Avenue','555-2048',68421879,3);

/* ===============================
   Nurse
   =============================== */
INSERT INTO Nurse VALUES (101,'Carla Espinosa','Head Nurse',1,111111110);
INSERT INTO Nurse VALUES (102,'Laverne Roberts','Nurse',1,222222220);
INSERT INTO Nurse VALUES (103,'Paul Flowers','Nurse',0,333333330);

/* ===============================
   Appointment
   =============================== */
INSERT INTO Appointment VALUES (13216584,100000001,101,1,'2008-04-24 10:00','2008-04-24 11:00','A');
INSERT INTO Appointment VALUES (26548913,100000002,101,2,'2008-04-24 10:00','2008-04-24 11:00','B');
INSERT INTO Appointment VALUES (36549879,100000001,102,1,'2008-04-25 10:00','2008-04-25 11:00','A');
INSERT INTO Appointment VALUES (46846589,100000004,103,4,'2008-04-25 10:00','2008-04-25 11:00','B');
INSERT INTO Appointment VALUES (59871321,100000004,NULL,4,'2008-04-26 10:00','2008-04-26 11:00','C');
INSERT INTO Appointment VALUES (69879231,100000003,103,2,'2008-04-26 11:00','2008-04-26 12:00','C');
INSERT INTO Appointment VALUES (76983231,100000001,NULL,3,'2008-04-26 12:00','2008-04-26 13:00','C');
INSERT INTO Appointment VALUES (86213939,100000004,102,9,'2008-04-27 10:00','2008-04-21 11:00','A');
INSERT INTO Appointment VALUES (93216548,100000002,101,2,'2008-04-27 10:00','2008-04-27 11:00','B');

/* ===============================
   Medication
   =============================== */
INSERT INTO Medication VALUES (1,'Procrastin-X','X','N/A');
INSERT INTO Medication VALUES (2,'Thesisin','Foo Labs','N/A');
INSERT INTO Medication VALUES (3,'Awakin','Bar Laboratories','N/A');
INSERT INTO Medication VALUES (4,'Crescavitin','Baz Industries','N/A');
INSERT INTO Medication VALUES (5,'Melioraurin','Snafu Pharmaceuticals','N/A');

/* ===============================
   Prescribes
   =============================== */
INSERT INTO Prescribes VALUES (1,100000001,1,'2008-04-24 10:47',13216584,'5');
INSERT INTO Prescribes VALUES (9,100000004,2,'2008-04-27 10:53',86213939,'10');
INSERT INTO Prescribes VALUES (9,100000004,2,'2008-04-30 16:53',NULL,'5');

/* ===============================
   Block
   =============================== */
INSERT INTO Block VALUES (1,1);
INSERT INTO Block VALUES (1,2);
INSERT INTO Block VALUES (1,3);
INSERT INTO Block VALUES (2,1);
INSERT INTO Block VALUES (2,2);
INSERT INTO Block VALUES (2,3);
INSERT INTO Block VALUES (3,1);
INSERT INTO Block VALUES (3,2);
INSERT INTO Block VALUES (3,3);
INSERT INTO Block VALUES (4,1);
INSERT INTO Block VALUES (4,2);
INSERT INTO Block VALUES (4,3);

/* ===============================
   Room
   =============================== */
INSERT INTO Room VALUES (101,'Single',1,1,0);
INSERT INTO Room VALUES (102,'Single',1,1,0);
INSERT INTO Room VALUES (103,'Single',1,1,0);
INSERT INTO Room VALUES (111,'Single',1,2,0);
INSERT INTO Room VALUES (112,'Single',1,2,1);
INSERT INTO Room VALUES (113,'Single',1,2,0);
INSERT INTO Room VALUES (121,'Single',1,3,0);
INSERT INTO Room VALUES (122,'Single',1,3,0);
INSERT INTO Room VALUES (123,'Single',1,3,0);
INSERT INTO Room VALUES (201,'Single',2,1,1);
INSERT INTO Room VALUES (202,'Single',2,1,0);
INSERT INTO Room VALUES (203,'Single',2,1,0);
INSERT INTO Room VALUES (211,'Single',2,2,0);
INSERT INTO Room VALUES (212,'Single',2,2,0);
INSERT INTO Room VALUES (213,'Single',2,2,1);
INSERT INTO Room VALUES (221,'Single',2,3,0);
INSERT INTO Room VALUES (222,'Single',2,3,0);
INSERT INTO Room VALUES (223,'Single',2,3,0);
INSERT INTO Room VALUES (301,'Single',3,1,0);
INSERT INTO Room VALUES (302,'Single',3,1,1);
INSERT INTO Room VALUES (303,'Single',3,1,0);
INSERT INTO Room VALUES (311,'Single',3,2,0);
INSERT INTO Room VALUES (312,'Single',3,2,0);
INSERT INTO Room VALUES (313,'Single',3,2,0);
INSERT INTO Room VALUES (321,'Single',3,3,1);
INSERT INTO Room VALUES (322,'Single',3,3,0);
INSERT INTO Room VALUES (323,'Single',3,3,0);
INSERT INTO Room VALUES (401,'Single',4,1,0);
INSERT INTO Room VALUES (402,'Single',4,1,1);
INSERT INTO Room VALUES (403,'Single',4,1,0);
INSERT INTO Room VALUES (411,'Single',4,2,0);
INSERT INTO Room VALUES (412,'Single',4,2,0);
INSERT INTO Room VALUES (413,'Single',4,2,0);
INSERT INTO Room VALUES (421,'Single',4,3,1);
INSERT INTO Room VALUES (422,'Single',4,3,0);
INSERT INTO Room VALUES (423,'Single',4,3,0);

/* ===============================
   On_Call
   =============================== */
INSERT INTO On_Call VALUES (101,1,1,'2008-11-04 11:00','2008-11-04 19:00');
INSERT INTO On_Call VALUES (101,1,2,'2008-11-04 11:00','2008-11-04 19:00');
INSERT INTO On_Call VALUES (102,1,3,'2008-11-04 11:00','2008-11-04 19:00');
INSERT INTO On_Call VALUES (103,1,1,'2008-11-04 19:00','2008-11-05 03:00');
INSERT INTO On_Call VALUES (103,1,2,'2008-11-04 19:00','2008-11-05 03:00');
INSERT INTO On_Call VALUES (103,1,3,'2008-11-04 19:00','2008-11-05 03:00');

/* ===============================
   Stay
   =============================== */
INSERT INTO Stay VALUES (3215,100000001,111,'2008-05-01','2008-05-04');
INSERT INTO Stay VALUES (3216,100000003,123,'2008-05-03','2008-05-14');
INSERT INTO Stay VALUES (3217,100000004,112,'2008-05-02','2008-05-03');

/* ===============================
   Undergoes
   =============================== */
INSERT INTO Undergoes VALUES (100000001,6,3215,'2008-05-02',3,101);
INSERT INTO Undergoes VALUES (100000001,2,3215,'2008-05-03',7,101);
INSERT INTO Undergoes VALUES (100000004,1,3217,'2008-05-07',3,102);
INSERT INTO Undergoes VALUES (100000004,5,3217,'2008-05-09',6,NULL);
INSERT INTO Undergoes VALUES (100000001,7,3217,'2008-05-10',7,101);
INSERT INTO Undergoes VALUES (100000004,4,3217,'2008-05-13',3,103);

/* ===============================
   Trained_In
   =============================== */
INSERT INTO Trained_In VALUES (3,1,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (3,2,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (3,5,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (3,6,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (3,7,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (6,2,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (6,5,'2007-01-01','2007-12-31');
INSERT INTO Trained_In VALUES (6,6,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (7,1,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (7,2,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (7,3,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (7,4,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (7,5,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (7,6,'2008-01-01','2008-12-31');
INSERT INTO Trained_In VALUES (7,7,'2008-01-01','2008-12-31');