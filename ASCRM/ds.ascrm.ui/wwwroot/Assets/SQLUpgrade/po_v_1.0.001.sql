/****************************************************************************************
******** This is an update script with versions, do not amend the core script, 
******** TO DO:
******** 1- update the SetVer... values for your script, check which is latest file in code
******** 2- write your logic where it is indicated below
******** 3- commit to source control this file with same SetVer.... ie po_v_1.0.001.sql
********		where: 1 = Release,  0 = Build  , 001 = Patch
********
******** Short description of this script:
********
******** Creates tables tblProductUpgradeLogs, tblProduct and insert default values
****************************************************************************************/

 /* update below for every script! **************** */
 DECLARE @SetVerRelease int = 1,	-- <<<--- Update this if required
		 @SetVerBuild int = 0,		-- <<<--- Update this if required
		 @SetVerPatch int = 001,	-- <<<--- Update this Always
		 @Success bit = 0 ; -- do not touch @Success
 /* update ABOVE for every script! **************** */
 

---****first time script
	IF ( NOT EXISTS (SELECT * 
				FROM INFORMATION_SCHEMA.TABLES 
				WHERE TABLE_SCHEMA = 'dbo' 
				AND  TABLE_NAME = 'tblProductUpgradeLogs'))
	BEGIN
		-- --------------------------------------
	 	CREATE TABLE [dbo].[tblProductUpgradeLogs](
			[pul_id] [int] IDENTITY(1,1) NOT NULL,
			[pul_error_number] [varchar](15) NULL,
			[pul_error_message] text NULL,
			[pul_version_actual] [varchar](15) NULL,
			[pul_version_toupdate] [varchar](15) NULL,
			[pul_date] datetime2 default getdate(),
		 CONSTRAINT [PK_tblProductUpgradeLogs] PRIMARY KEY CLUSTERED 
		(
			[pul_id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		 
	END
	 


BEGIN TRY
    BEGIN TRANSACTION 
    
	/********************************************************
	*********************************************************
	****************** WRITE YOUR LOGIC SCRIPT BELOW *********
	*********************************************************
	*********************************************************
	*********************************************************/ 
	--select 1/0 throws error!
	   
	--action 1
	IF ( NOT EXISTS (SELECT * 
				FROM INFORMATION_SCHEMA.TABLES 
				WHERE TABLE_SCHEMA = 'dbo' 
				AND  TABLE_NAME = 'tblProduct'))
	BEGIN
		-- --------------------------------------
	 	--create new Product version table
		CREATE TABLE [dbo].[tblProduct](
			[ProductName] [varchar](50) NULL,
			[Edition] [varchar](50) NULL,
			[Release] [int] NULL,
			[Build] [int] NULL,
			[Patch] [int] NULL
			) ON [PRIMARY]
		 
		--insert default
		INSERT INTO [dbo].[tblProduct]
           ([ProductName]
           ,[Edition]
           ,[Release]
           ,[Build]
           ,[Patch])
		VALUES
           ('CRM Portal'
           ,'beta'
           ,1
           ,0
           ,0)
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
