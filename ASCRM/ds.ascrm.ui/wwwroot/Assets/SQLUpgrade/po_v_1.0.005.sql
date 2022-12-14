/****************************************************************************************
******** Short description of this script:
********
******** drop some SPs with incorrect typos, creates new one which are used in the code
****************************************************************************************/

DECLARE @SetVerRelease int = 1,	 
		 @SetVerBuild int = 0,		 
		 @SetVerPatch int =  5,	 
		 @Success bit = 0 ;  

 

BEGIN TRY
    BEGIN TRANSACTION 
     
	/********************************************************
	*********************************************************
	****************** WRITE YOUR LOGIC SCRIPT BELOW *********
	*********************************************************
	*********************************************************
	*********************************************************/    
	BEGIN TRANSACTION
	SET QUOTED_IDENTIFIER ON
	SET ARITHABORT ON
	SET NUMERIC_ROUNDABORT OFF
	SET CONCAT_NULL_YIELDS_NULL ON
	SET ANSI_NULLS ON
	SET ANSI_PADDING ON
	SET ANSI_WARNINGS ON
	COMMIT
	BEGIN TRANSACTION
	GO
	ALTER TABLE dbo.Lead ADD CONSTRAINT
		DF_Lead_Lead_DateOfEnquiry DEFAULT (getdate()) FOR Lead_DateOfEnquiry
	GO
	ALTER TABLE dbo.Lead SET (LOCK_ESCALATION = TABLE)
	GO
	COMMIT
	
	
	
	BEGIN TRANSACTION
	SET QUOTED_IDENTIFIER ON
	SET ARITHABORT ON
	SET NUMERIC_ROUNDABORT OFF
	SET CONCAT_NULL_YIELDS_NULL ON
	SET ANSI_NULLS ON
	SET ANSI_PADDING ON
	SET ANSI_WARNINGS ON
	COMMIT
	BEGIN TRANSACTION
	GO
	ALTER TABLE dbo.Lead ADD CONSTRAINT
		DF_Lead_Lead_DateCreated DEFAULT (getdate()) FOR Lead_DateCreated
	GO
	ALTER TABLE dbo.Lead SET (LOCK_ESCALATION = TABLE)
	GO
	COMMIT


	/********************************************************
	*********************************************************
	****************** DO NOT WRITE ANYTHING AFTER THIS *****
	*********************************************************
	*********************************************************
	*********************************************************/


    COMMIT

	SET @Success = 1;


END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK
	 
		INSERT INTO [tblProductUpgradeLogs] 
		(	pul_error_number
			,pul_error_message
			,pul_version_actual
			,pul_version_toupdate
			,pul_date
		)
		SELECT  ERROR_NUMBER() AS ErrorNumber  
			,ERROR_MESSAGE() AS ErrorMessage
			, (SELECT 
					CAST([Release] as varchar(5)) +'.' + 
					CAST([Build] as varchar(5)) + '.' +
					CAST([Patch] as varchar(5))
				FROM [tblProduct] ) 		
			, CONCAT(CAST(@SetVerRelease as varchar(5)),'.',CAST(@SetVerBuild as varchar(5)),'.',CAST(@SetVerPatch as varchar(5))) 
			,GETDATE() ;

		SELECT	ERROR_NUMBER() AS ErrorNumber  
				,ERROR_MESSAGE() AS ErrorMessage 
	

END CATCH

IF @Success = 1 
BEGIN
		 
	INSERT INTO [tblProductUpgradeLogs] 
		(	pul_error_number
			,pul_error_message
			,pul_version_actual
			,pul_version_toupdate
			,pul_date
		)
	SELECT  '' AS ErrorNumber  
			,'Success' AS ErrorMessage
			, (SELECT 
					CAST([Release] as varchar(5)) +'.' + 
					CAST([Build] as varchar(5)) + '.' +
					CAST([Patch] as varchar(5))
				FROM [tblProduct] ) 		
			, CONCAT(CAST(@SetVerRelease as varchar(5)),'.',CAST(@SetVerBuild as varchar(5)),'.',CAST(@SetVerPatch as varchar(5))) 
			,GETDATE() ;
	-- update table after 
	UPDATE  [dbo].[tblProduct] 
	SET  [Release]  = @SetVerRelease
         ,[Build] = @SetVerBuild
         ,[Patch] = @SetVerPatch  
		    
END
 