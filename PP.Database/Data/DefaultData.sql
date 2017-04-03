IF NOT EXISTS (SELECT * FROM [ProjectCommentType] WHERE Name = 'Programdiskussion')
	INSERT INTO [ProjectCommentType] (Name) VALUES ('Programdiskussion')

IF NOT EXISTS (SELECT * FROM [ProjectCommentType] WHERE Name = 'Projektdiskussion')
	INSERT INTO [ProjectCommentType] (Name) VALUES ('Projektdiskussion')

IF NOT EXISTS (SELECT * FROM [ProjectCommentType] WHERE Name = 'Styrgruppsdiskussion')
	INSERT INTO [ProjectCommentType] (Name) VALUES ('Styrgruppsdiskussion')


--projectrolegroup
	IF NOT EXISTS (SELECT * FROM [ProjectRoleGroup] WHERE Name = 'Programadministration')
	INSERT INTO [ProjectRoleGroup] (Name, [ProjectCommentTypeId]) VALUES ('Programadministration',1)

	IF NOT EXISTS (SELECT * FROM [ProjectRoleGroup] WHERE Name = 'Projektadministration')
	INSERT INTO [ProjectRoleGroup] (Name, [ProjectCommentTypeId]) VALUES ('Projektadministration',1)

	IF NOT EXISTS (SELECT * FROM [ProjectRoleGroup] WHERE Name = 'Projektdeltagare')
	INSERT INTO [ProjectRoleGroup] (Name, [ProjectCommentTypeId]) VALUES ('Projektdeltagare',1)

	IF NOT EXISTS (SELECT * FROM [ProjectRoleGroup] WHERE Name = 'Projektstyrgrupp')
	INSERT INTO [ProjectRoleGroup] (Name, [ProjectCommentTypeId]) VALUES ('Projektstyrgrupp',1)


IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Programägare')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Programägare',2,1,1)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Programadministratör')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Programadministratör',2,2,1)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Projektägare')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Projektägare',2,1,2)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Projektledare')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Projektledare',2,2,2)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Projektsekreterare')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Projektsekreterare',2,3,2)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Projektsamordnare')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Projektsamordnare',1,4,2)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Projektmedlem')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Projektmedlem',1,1,3)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Projektkommunikatör')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Projektkommunikatör',1,2,3)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Styrgruppsordförande')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Styrgruppsordförande',1,1,4)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Styrgruppsmedlem')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Styrgruppsmedlem',1,2,4)

IF NOT EXISTS (SELECT * FROM [ProjectRole] WHERE Name = 'Adjungerad')
	INSERT INTO [ProjectRole] ([Name], [PermissionId], [SortOrder], [ProjectRoleGroupId]) VALUES ('Adjungerad',1,3,4)


INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Programägare'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Programadministratör'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektägare'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektägare'
WHERE prg.Name = 'Projektadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektledare'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektledare'
WHERE prg.Name = 'Projektadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektsekreterare'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektsekreterare'
WHERE prg.Name = 'Projektadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektsamordnare'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektmedlem'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektmedlem'
WHERE prg.Name = 'Projektadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektkommunikatör'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Projektkommunikatör'
WHERE prg.Name = 'Projektadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Styrgruppsordförande'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Styrgruppsordförande'
WHERE prg.Name = 'Projektadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Styrgruppsmedlem'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Styrgruppsmedlem'
WHERE prg.Name = 'Projektadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Adjungerad'
WHERE prg.Name = 'Programadministration'

INSERT INTO [ProjectRoleAdministration] ([RoleID], [RoleGroupId]) SELECT  pr.[id], prg.[Id] FROM [ProjectRoleGroup] prg 
INNER JOIN [ProjectRole] pr ON pr.Name = 'Adjungerad'
WHERE prg.Name = 'Projektadministration'

/*PROGRAM START*/
SET IDENTITY_INSERT dbo.[Program] ON
	
IF NOT EXISTS (SELECT * FROM [Program] WHERE Name = 'Nationella eHälsoprojekt')
	INSERT INTO [Program]
           (Id, [Name] ,[TypeId] ,[RequireProgramOwner] ,[RequireProjectCoordinator] ,[Active] ,[Description])
     VALUES(
           (1, 'Nationella eHälsoprojekt',1,0,0,1,'Nationella projekt är synliga för alla. Landsadministratör har alltid redigeringsbehörighet')
           
IF NOT EXISTS (SELECT * FROM [Program] WHERE Name = 'eHälsoprojekt 2014')
	INSERT INTO [Program]
           (Id, [Name] ,[TypeId] ,[RequireProgramOwner] ,[RequireProjectCoordinator] ,[Active] ,[Description])
     VALUES(
           (2, 'eHälsoprojekt 2014',1,1,1,1,'Nationella projekt är synliga för alla. Landsadministratör har alltid redigeringsbehörighet')
           
           
IF NOT EXISTS (SELECT * FROM [Program] WHERE Name = 'eHälsoprojekt 2013')
	INSERT INTO [Program]
           (Id, [Name] ,[TypeId] ,[RequireProgramOwner] ,[RequireProjectCoordinator] ,[Active] ,[Description])
     VALUES(
           (3, 'eHälsoprojekt 2013',1,1,1,0,'Nationella projekt är synliga för alla. Landsadministratör har alltid redigeringsbehörighet')
           

IF NOT EXISTS (SELECT * FROM [Program] WHERE Name = 'Länsprojekt')
	INSERT INTO [Program]
           (Id, [Name] ,[TypeId] ,[RequireProgramOwner] ,[RequireProjectCoordinator] ,[Active] ,[Description])
     VALUES(
           (4, 'Länsprojekt',2,0,0,1,'Länsprojekt är synliga för alla inom länet. Länsadministratör har alltid redigeringsbehörighet. Kommunadministratör har alltid redigeringsbehörighet')

IF NOT EXISTS (SELECT * FROM [Program] WHERE Name = 'Kommunprojekt')           
    INSERT INTO [Program]
           (Id, [Name] ,[TypeId] ,[RequireProgramOwner] ,[RequireProjectCoordinator] ,[Active] ,[Description])
     VALUES(5, 'Kommunprojekt',3,0,0,1,'Kommunprojekt är synliga för alla inom kommunen. Kommunadministratör har har alltid redigeringsbehörighet')

IF NOT EXISTS (SELECT * FROM [Program] WHERE Name = 'Verksamhets-projekt')    
	INSERT INTO [Program]
           (Id, [Name] ,[TypeId] ,[RequireProgramOwner] ,[RequireProjectCoordinator] ,[Active] ,[Description])
     VALUES(6, 'Verksamhets-projekt' ,4, 0, 0 ,1 ,'Verksamhetsprojekt är bara synliga för listade deltagare')

SET IDENTITY_INSERT dbo.[Program] OFF
/*PROGRAM END*/

/*PROJECT AREA START*/
SET IDENTITY_INSERT dbo.[ProjectArea] OFF

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'eTjänster för invånarna')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (1, 'eTjänster för invånarna', 2)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'säker roll- och behörighetsidentifikation')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (2, 'säker roll- och behörighetsidentifikation', 2)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'mobil dokumentation')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (3, 'mobil dokumentation', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'digitala trygghetslarm')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (4, 'digitala trygghetslarm', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'införande NPÖ-konsument')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (5, 'införande NPÖ-konsument', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = '> administration')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (6, '> administration', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'Länsprojekt')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (7, 'Länsprojekt', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'Kommunprojekt')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (8, 'Kommunprojekt', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'Verksamhetsprojekt')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (9, 'Verksamhetsprojekt', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'Mobilt arbete')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (10, 'Mobilt arbete', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'Digitala trygghetslarm')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (11, 'Digitala trygghetslarm', 3)

IF NOT EXISTS (SELECT * FROM [ProjectArea] WHERE Name = 'NPÖ')
	INSERT INTO [ProjectArea] (Id, Name, ProgramId) VALUES (12, 'NPÖ', 3)

SET IDENTITY_INSERT dbo.[ProjectArea] OFF
/*PROJECT AREA END*/

--INSERT INTO [User] (Name, Email, RoleId)
--	VALUES ('Claes-Philip Staiger', 'claes-philip.staiger@stretch.se', 1) --default role

--INSERT INTO [User] (Name, Email, RoleId)
--	VALUES ('Mikael Möregårdh', 'mikael.moregardh@stretch.se', 1) --default role

--INSERT INTO [User] (Name, Email, RoleId)
--	VALUES ('Philip Seneby', 'philip.seneby@stretch.se', 1) --default role


--Project
--1,1,'2012-01-01 00:00:00.000',	1,'2012-01-01 00:00:00.000','2012-01-01 00:00:00.000',1,1,10,	10, 1,	'kjh',	10,	10,	10,	10,	'N/A'	'N/A'	'N/A'	'N/A'	'N/A'	'N/A'	'N/A'	'N/A'	'N/A'	'N/A,'	'N/A'




--PROJECTCOMMENT

INSERT INTO ProjectCommentArea (Name) VALUES ('Projectidea')

INSERT INTO ProjectCommentArea (Name) VALUES ('Finance')

INSERT INTO ProjectCommentArea (Name) VALUES ('Members')

INSERT INTO ProjectCommentArea (Name) VALUES ('Projectplan')

INSERT INTO ProjectCommentArea (Name) VALUES ('Goal')

INSERT INTO ProjectCommentArea (Name) VALUES ('Activity')

INSERT INTO ProjectCommentArea (Name) VALUES ('Followup')

INSERT INTO ProjectCommentArea (Name) VALUES ('Debriefing')


--
INSERT INTO [ProjectCommentType] (Name) VALUES ('Administration')

INSERT INTO [ProjectCommentType] (Name) VALUES ('Project')

