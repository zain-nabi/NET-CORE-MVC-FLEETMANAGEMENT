USE [TritonGroup]
GO

DECLARE @CreatedByUserID INT = 88392
DECLARE @LookupcodeCategoryID VARCHAR(100) = (SELECT LC.LookupcodeCategoryID  FROM [dbo].[LookupcodeCategories] LC WITH(NOLOCK) WHERE LC.Category = 'Loyality Types')
DECLARE @Item1 VARCHAR(100) ='Platinum'
DECLARE @Item2 VARCHAR(100) ='Gold'
DECLARE @Item3 VARCHAR(100) ='Silver'
DECLARE @Item4 VARCHAR(100) ='Bronze'

IF (SELECT COUNT(*) FROM [dbo].[Lookupcodes] L WHERE L.Name = @Item1 AND  [LookupcodeCategoryID] = @LookupcodeCategoryID ) = 0
BEGIN
INSERT INTO [dbo].[Lookupcodes]
           ([Name]
           ,[Detail]
           ,[LookupcodeCategoryID]
           ,[Sequence]
           ,[CreatedByUserID]
           ,[CreatedOn]
           ,[DeletedByUserID]
           ,[DeletedOn]
           ,[FAIconString]
           ,[AdditionalField1Name]
           ,[AdditionalField1Value])
     VALUES
           (@Item1
           ,@Item1
           ,@LookupcodeCategoryID
           ,1
           ,@CreatedByUserID
           ,GETDATE()
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
		   )
END

IF (SELECT COUNT(*) FROM [dbo].[Lookupcodes] L WHERE L.Name = @Item2 AND  [LookupcodeCategoryID] = @LookupcodeCategoryID ) = 0
BEGIN
INSERT INTO [dbo].[Lookupcodes]
           ([Name]
           ,[Detail]
           ,[LookupcodeCategoryID]
           ,[Sequence]
           ,[CreatedByUserID]
           ,[CreatedOn]
           ,[DeletedByUserID]
           ,[DeletedOn]
           ,[FAIconString]
           ,[AdditionalField1Name]
           ,[AdditionalField1Value])
     VALUES
           (@Item2
           ,@Item2
           ,@LookupcodeCategoryID
           ,1
           ,@CreatedByUserID
           ,GETDATE()
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
		   )
END

IF (SELECT COUNT(*) FROM [dbo].[Lookupcodes] L WHERE L.Name = @Item3 AND  [LookupcodeCategoryID] = @LookupcodeCategoryID ) = 0
BEGIN
INSERT INTO [dbo].[Lookupcodes]
           ([Name]
           ,[Detail]
           ,[LookupcodeCategoryID]
           ,[Sequence]
           ,[CreatedByUserID]
           ,[CreatedOn]
           ,[DeletedByUserID]
           ,[DeletedOn]
           ,[FAIconString]
           ,[AdditionalField1Name]
           ,[AdditionalField1Value])
     VALUES
           (@Item3
           ,@Item3
           ,@LookupcodeCategoryID
           ,1
           ,@CreatedByUserID
           ,GETDATE()
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
		   )
END

IF (SELECT COUNT(*) FROM [dbo].[Lookupcodes] L WHERE L.Name = @Item4 AND  [LookupcodeCategoryID] = @LookupcodeCategoryID ) = 0
BEGIN
INSERT INTO [dbo].[Lookupcodes]
           ([Name]
           ,[Detail]
           ,[LookupcodeCategoryID]
           ,[Sequence]
           ,[CreatedByUserID]
           ,[CreatedOn]
           ,[DeletedByUserID]
           ,[DeletedOn]
           ,[FAIconString]
           ,[AdditionalField1Name]
           ,[AdditionalField1Value])
     VALUES
           (@Item4
           ,@Item4
           ,@LookupcodeCategoryID
           ,1
           ,@CreatedByUserID
           ,GETDATE()
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
		   )
END

GO

