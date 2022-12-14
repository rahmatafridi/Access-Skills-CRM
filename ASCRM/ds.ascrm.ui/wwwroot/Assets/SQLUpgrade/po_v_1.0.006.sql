/****************************************************************************************
******** Short description of this script:
********
******** drop some SPs with incorrect typos, creates new one which are used in the code
****************************************************************************************/

DECLARE @SetVerRelease int = 1,	 
		 @SetVerBuild int = 0,		 
		 @SetVerPatch int =  6,	 
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
		AND OBJECT_ID = OBJECT_ID('dbo.SP_mdlead_Dashboard_GetMyLeads'))
	BEGIN
		DROP PROCEDURE SP_mdlead_Dashboard_GetMyLeads 
		 
	END ;	
	GO
	   
	-- =============================================
	-- Author:		Ubaid
	-- Create date: 30/09/2019
	-- Description:	Get leads by user id
	-- =============================================
	CREATE PROCEDURE [dbo].[SP_Dashboard_GetMyLeads]
		@Users_Id		int
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		SELECT 	
			Lead_Id AS Lead_Id, 
			Lead_ContactName AS ContactName, 
			CL.CL_Title AS CourseTitle, 
			Lead_DateOfEnquiry AS DateOfEnquiry,
			Lead_Id_Status AS Status_Id,
			(SELECT ddlo.Opt_Title 
				FROM DDL_Options ddlo 
				INNER JOIN DDL_OptionsHeaders ddloh ON ddlo.Opt_Id_OptHeader = ddloh.OptHeader_Id 
				WHERE ddlo.Opt_Value = [Lead_Id_Status] 
				AND ddloh.OptHeader_Title = 'LeadStatus') AS 'Status'	 
		FROM Lead 
			LEFT OUTER JOIN tblCourseLevel CL on [Lead_Id_CourseLevel] = CL.CL_Id
		WHERE Lead_Id_SalesAdvisor = @Users_Id

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
 