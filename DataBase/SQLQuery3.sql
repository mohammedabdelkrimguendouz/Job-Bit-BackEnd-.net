

exec SP_GetAllJobSeekers
select * from JobSeekerSkills

backup database JobBit_DB
to disk = 'C:\JobBit_DB.bak'

DECLARE @i INT = 1, @JobID INT




WHILE @i <= 30  
BEGIN
    INSERT INTO Jobs (CompanyID, JobType, PostedDate, Experience, Available, Title, Description)
    VALUES 
    (1, 
     (SELECT TOP 1 number FROM master..spt_values WHERE type='P' AND number BETWEEN 0 AND 2 ORDER BY NEWID()), 
     GETDATE(), 
     (SELECT TOP 1 number FROM master..spt_values WHERE type='P' AND number BETWEEN 0 AND 3 ORDER BY NEWID()), 
     1, 
     'Job Title ' + CAST(@i AS NVARCHAR), 
     'This is a sample job description.')

    SET @JobID = SCOPE_IDENTITY()  

   
    INSERT INTO JobSkills (JobID, SkillID)
    SELECT TOP 4 @JobID, SkillID 
    FROM Skills 
    WHERE SkillID BETWEEN 70 AND 250
    ORDER BY NEWID()

    SET @i = @i + 1
END
