/****************************************************************************************
******** Short description of this script:
********
******** drop some SPs with incorrect typos, creates new one which are used in the code
****************************************************************************************/

DECLARE @SetVerRelease int = 1,	 
		 @SetVerBuild int = 0,		 
		 @SetVerPatch int =  4,	 
		 @Success bit = 0 ;  

 

BEGIN TRY
    BEGIN TRANSACTION 
     
	/********************************************************
	*********************************************************
	****************** WRITE YOUR LOGIC SCRIPT BELOW *********
	*********************************************************
	*********************************************************
	*********************************************************/    
	update tblproduct set edition = 'Beta'
	
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
 