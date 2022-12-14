/****************************************************************************************
******** Short description of this script:
********
******** drop some SPs with incorrect typos, creates new one which are used in the code
****************************************************************************************/

DECLARE @SetVerRelease int = 1,	 
		 @SetVerBuild int = 0,		 
		 @SetVerPatch int =  7,	 
		 @Success bit = 0 ;  

 

BEGIN TRY
    BEGIN TRANSACTION 
     
	/********************************************************
	*********************************************************
	****************** WRITE YOUR LOGIC SCRIPT BELOW *********
	*********************************************************
	*********************************************************
	*********************************************************/    
	 
	
	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
		AND OBJECT_ID = OBJECT_ID('dbo.SP_Lead_GetAllHistory'))
	BEGIN
		DROP PROCEDURE SP_Lead_GetAllHistory 
		 
	END 	

	GO
	   
	-- =============================================                          
	-- Author		: Yasar                     
	-- Create date	: 05/02/2020                          
	-- Description	: Get all history of a lead                      
	-- History                              
	-- Date			Created By		Description                              
	-- 05/02/2020   Yasar			Init.                          
	-- =============================================                          
	CREATE PROCEDURE [dbo].[SP_Lead_GetAllHistory]              
	@lead_id INT                                  
	AS                          
	BEGIN                                  
	SET NOCOUNT ON;                          
		SELECT    
		  history_action_id    
		 ,module    
		 ,action_type    
		 ,action_opt_id    
		 ,lead_id    
		 ,user_id    
		 ,FORMAT(date_time,'dd/MM/yyyy HH:mm') AS date_time    
		 ,is_deleted    
		 ,u.users_username as username
		 ,o.Opt_Title as action_title
		FROM tblHistoryActions tha
		LEFT JOIN Users u ON u.users_id = tha.user_id
		LEFT JOIN DDL_Options o ON o.Opt_Id = tha.action_opt_id
		WHERE lead_id = @lead_id               

	 ORDER BY date_time                                 
	END     



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
 