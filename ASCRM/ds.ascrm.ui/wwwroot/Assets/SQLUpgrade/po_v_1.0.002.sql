/****************************************************************************************
******** Short description of this script:
********
******** drop some SPs with incorrect typos, creates new one which are used in the code
****************************************************************************************/

DECLARE @SetVerRelease int = 1,	 
		 @SetVerBuild int = 0,		 
		 @SetVerPatch int =  2,	 
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
			AND OBJECT_ID = OBJECT_ID('dbo.SP_mdCore_Configration_CheckKeyAndValueExists'))
		BEGIN
			DROP PROCEDURE SP_mdCore_Configration_CheckKeyAndValueExists   			 
		END ;	
 
	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_mdCore_Configration_DeleteById'))
		BEGIN
			DROP PROCEDURE SP_mdCore_Configration_DeleteById 
		END; 	

	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_mdCore_Configration_GetAll'))
		BEGIN
			DROP PROCEDURE SP_mdCore_Configration_GetAll;
		END; 	

	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_mdCore_Configration_GetById'))
		BEGIN
			DROP PROCEDURE SP_mdCore_Configration_GetById;
		END;	 	

	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_mdCore_Configration_GetSingleValueByKey'))
		BEGIN
			DROP PROCEDURE SP_mdCore_Configration_GetSingleValueByKey;
		END;
	 	
	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_mdCore_Configration_Insert'))
		BEGIN
			DROP PROCEDURE SP_mdCore_Configration_Insert;
		END;
	 	

	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_mdCore_Configration_Update'))
		BEGIN
			DROP PROCEDURE SP_mdCore_Configration_Update;
		END; 	

	IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_ChangePassword'))
		BEGIN
			DROP PROCEDURE SP_ChangePassword 
		END; 
	
	 
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.SP_Core_Configuration_CheckKeyAndValueExists'))
		EXEC(N'CREATE PROCEDURE [dbo].[SP_Core_Configuration_CheckKeyAndValueExists] AS BEGIN SET NOCOUNT ON; END')
	GO 
		-- =============================================
		-- Author:		Ubaid Anwar
		-- Create date: 18-10-2019
		-- Description:	To check if value and key already exists 
		-- =============================================
		ALTER PROCEDURE [dbo].[SP_Core_Configuration_CheckKeyAndValueExists] 
		(
			@config_id		int = null,
			@config_key		nvarchar(50) = '',
			@config_value	nvarchar(MAX) = ''
		)
		AS
		BEGIN
			-- SET NOCOUNT ON added to prevent extra result sets from
			-- interfering with SELECT statements.
			SET NOCOUNT ON;
			BEGIN TRY
				DECLARE @ValueExists BIT
				DECLARE @KeyExists BIT
				SELECT @ValueExists = COUNT(*) FROM [dbo].[tblConfig] WHERE [config_id] != @config_id AND [config_value] = @config_value
				SELECT @KeyExists = COUNT(*) FROM [dbo].[tblConfig] WHERE [config_id] != @config_id AND [config_key] = @config_key
				SELECT @ValueExists AS 'ValueExists', @KeyExists AS 'KeyExists'
			END TRY
			BEGIN CATCH
				SELECT ERROR_MESSAGE();
			END CATCH
		END ;
		 
	 
		 	
	
	
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_Core_Configuration_DeleteById'))
		 
			exec('CREATE PROCEDURE [dbo].[SP_Core_Configuration_DeleteById] AS BEGIN SET NOCOUNT ON; END')
		 
	GO	
	-- =============================================
	-- Author:		<Ubaid Anwar>
	-- Create date: <18-10-2019>
	-- Description:	<To delete configration by id>
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_Core_Configuration_DeleteById] 
	(
		@config_id int
	)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		BEGIN TRY
			DELETE FROM [dbo].[tblConfig]
				WHERE [config_id] = @config_id
		END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE();
		END CATCH
	END
	GO
		 
	
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_Core_Configuration_GetAll'))
	     
			exec('CREATE PROCEDURE [dbo].[SP_Core_Configuration_GetAll] AS BEGIN SET NOCOUNT ON; END')
		 
	GO	
	-- =============================================
	-- Author:		<Ubaid Anwar>
	-- Create date: <18-10-2019>
	-- Description:	<To get configration>
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_Core_Configuration_GetAll] 
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		SELECT [config_id]
			,[config_key]
			,[config_value]
			,[config_description]
		FROM [dbo].[tblConfig]
	 
	END
	GO
	
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_Core_Configuration_GetById'))
	     
			exec('CREATE PROCEDURE [dbo].[SP_Core_Configuration_GetById] AS BEGIN SET NOCOUNT ON; END')
		 
	GO	 	
		
	-- =============================================
	-- Author:		<Ubaid Anwar>
	-- Create date: <18-10-2019>
	-- Description:	<To get configration by id>
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_Core_Configuration_GetById] 
	(
		@config_id				int
	)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		SELECT [config_id]
			,[config_key]
			,[config_value]
			,[config_description]
		FROM [dbo].[tblConfig]
		WHERE [config_id] = @config_id
	END
	GO
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_Core_Configuration_GetSingleValueByKey'))
	     
			exec('CREATE PROCEDURE [dbo].[SP_Core_Configuration_GetSingleValueByKey] AS BEGIN SET NOCOUNT ON; END')
		 
	GO	

	-- =============================================
	-- Author:		<Ubaid Anwar>
	-- Create date: <18-10-2019>
	-- Description:	<To get configration value by key>
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_Core_Configuration_GetSingleValueByKey] 
	(
		@config_key				nvarchar(50)
	)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		SELECT TOP 1 [config_value]
		FROM [dbo].[tblConfig]
		WHERE [config_key] = @config_key
	 END
	 GO
	 
	
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_Core_Configuration_Insert'))
	     
			exec('CREATE PROCEDURE [dbo].[SP_Core_Configuration_Insert] AS BEGIN SET NOCOUNT ON; END')
		 
	GO	

	-- =============================================
	-- Author:		<Ubaid Anwar>
	-- Create date: <18-10-2019>
	-- Description:	<To get configration>
	-- =============================================
	ALTER PROCEDURE [dbo].[SP_Core_Configuration_Insert] 
	(
		@config_id				int = null,
		@config_key				nvarchar(50) = '',
		@config_value			nvarchar(MAX) = '',
		@config_description		nvarchar(500) = null	
	)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		BEGIN TRY	
			INSERT INTO [dbo].[tblConfig]
				(
					 [config_key]
					,[config_value]
					,[config_description]
				)
			VALUES
				(
					 @config_key
					,@config_value
					,@config_description
				)
		END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE();
		END CATCH	
	END
	GO	
	
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_Core_Configuration_Update'))
		 
			exec('CREATE PROCEDURE [dbo].[SP_Core_Configuration_Update] AS BEGIN SET NOCOUNT ON; END')
		 
	GO	
	
	-- =============================================
	-- Author:		Ubaid Anwar
	-- Create date: 18/10/2019
	-- Description:	to update configration
	-- =============================================
	ALTER PROCEDURE [dbo].SP_Core_Configuration_Update 
	(
		@config_id				int = null,
		@config_key				nvarchar(50) = '',
		@config_value			nvarchar(MAX) = '',
		@config_description		nvarchar(500) = null	
	)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;
		BEGIN TRY
			UPDATE [dbo].[tblConfig]
			SET 
				   [config_key] = @config_key
				  ,[config_value] = @config_value
				  ,[config_description] = @config_description
			 WHERE [config_id] = @config_id
		END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE();
		END CATCH
	END
	GO	
	
	--- ***********************************************
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' 
			AND OBJECT_ID = OBJECT_ID('dbo.SP_User_ChangePassword'))
	     
			exec('CREATE PROCEDURE [dbo].[SP_User_ChangePassword] AS BEGIN SET NOCOUNT ON; END')
		 
	GO	
	
	-- Author:		Ubaid Anwar
	-- Create date: 29/11/2019
	-- Description:	Change user password
	-- =============================================
	ALTER PROCEDURE [dbo].SP_User_ChangePassword
		@GUID uniqueidentifier,
		@Password nvarchar(100)
	AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;

		DECLARE @UserId INT
	 
		SELECT @UserId = UserId 
		FROM tblResetPasswordRequests
		WHERE Id= @GUID
	 
		if(@UserId is null)
			Begin
				-- If UserId does not exist
				Select 0 as IsPasswordChanged
			End
		Else
			Begin
				-- If UserId exists, Update with new password
				Update Users set
				Users_Password = @Password, Users_PasswordChangedDate = GETDATE()
				where Users_Id = @UserId
	  
				-- Delete the password reset request row 
				Delete from tblResetPasswordRequests
				where Id = @GUID
	  
				Select 1 as IsPasswordChanged
			End
	END
	GO	
	

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
 