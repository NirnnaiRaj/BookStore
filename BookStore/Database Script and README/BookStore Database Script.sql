/****** Object:  UserDefinedTableType [dbo].[UDT_OrderTable]    Script Date: 12-05-2024 13:49:10 ******/
CREATE TYPE [dbo].[UDT_OrderTable] AS TABLE(
	[Order_ID] [int] NOT NULL,
	[Book_ID] [int] NOT NULL
)
GO
/****** Object:  Table [dbo].[Books]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Book_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[Author] [nvarchar](500) NULL,
	[Genre] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](18, 3) NULL,
	[Publish_Date] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[Image] [nvarchar](500) NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[Book_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Order_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[User_ID] [bigint] NULL,
	[Book_ID] [bigint] NULL,
	[Quantity] [int] NULL,
	[Order_Date] [datetime] NULL,
	[Total_Price] [decimal](18, 3) NULL,
	[IsPending] [bit] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Order_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](500) NULL,
	[Phone] [varchar](20) NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Table_User] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Books] ADD  CONSTRAINT [DF_Books_Deleted]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_IsPending]  DEFAULT ((0)) FOR [IsPending]
GO
/****** Object:  StoredProcedure [dbo].[BookAddToCart]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[BookAddToCart]
(
	 @BookID	BIGINT = NULL,
	 @UserID	BIGINT = NULL
)
AS
BEGIN
	IF NOT EXISTS(SELECT Order_ID FROM Orders WHERE Book_ID = @BookID AND [User_ID] = @UserID AND IsPending = 1)
	BEGIN
		INSERT INTO Orders
		(
		 [User_ID],[Book_ID],[Quantity],[Order_Date],[Total_Price],IsPending
		)
		 SELECT @UserID,@BookID,1,GETDATE(),Price,1
		 FROM [dbo].[Books] WHERE [Book_ID] = @BookID

	END
	ELSE 
	BEGIN
		UPDATE OD
		SET [Quantity] = OD.[Quantity]+1,
			Order_Date = GETDATE(),
			[Total_Price] = OD.[Total_Price]+B.[Price]
			FROM Orders OD
			INNER JOIN Books B ON B.[Book_ID] = OD.[Book_ID]
			WHERE OD.[Book_ID] = @BookID
			AND [User_ID] = @UserID
			AND OD.IsPending = 1

	END
END


GO
/****** Object:  StoredProcedure [dbo].[GetAllBooks]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetAllBooks]
AS
BEGIN
	SELECT [Book_ID],[Title],[Author],[Genre] ,[Price],[Publish_Date],[Status]
	FROM [dbo].[Books]
END


GO
/****** Object:  StoredProcedure [dbo].[GetBookByID]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetBookByID]
(
	@BookID	BIGINT = NULL
)
AS
BEGIN
	SELECT [Book_ID],[Title],[Author],[Genre],[Description] ,[Price],[Publish_Date],[Status],[Image]
	FROM [dbo].[Books]
	WHERE [Book_ID] = @BookID
END


GO
/****** Object:  StoredProcedure [dbo].[GetCartStatusByUser]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetCartStatusByUser]
(
	 @UserID	BIGINT = NULL
)
AS
BEGIN
	SELECT OD.Order_ID,OD.[Book_ID],OD.[User_ID],B.Title,OD.Quantity,B.Price 
	FROM Orders OD 
	INNER JOIN Books B ON B.[Book_ID] = OD.[Book_ID]
	WHERE OD.[User_ID] = @UserID AND IsPending = 1 AND OD.Quantity>0
END


GO
/****** Object:  StoredProcedure [dbo].[GetPreviousOrders]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetPreviousOrders]
(
	 @UserID	BIGINT = NULL
)
AS
BEGIN
	SELECT OD.Order_ID,OD.[Book_ID],OD.[User_ID],B.Title,OD.Quantity,B.Price 
	FROM Orders OD 
	INNER JOIN Books B ON B.[Book_ID] = OD.[Book_ID]
	WHERE OD.[User_ID] = @UserID AND IsPending = 0 --AND OD.Quantity>0
END


GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RegisterUser]
(
  @UserName NVARCHAR(50) = NULL,
  @Email NVARCHAR(50) = NULL,
  @Address NVARCHAR(500) = NULL,
  @Phone NVARCHAR(50) = NULL,
  @Password NVARCHAR(50) = NULL
)
AS
	IF NOT EXISTS(SELECT Email FROM Users WHERE Email = @Email OR Phone = @Phone)
	BEGIN
		INSERT INTO Users
		(
		UserName,Email,[Address],Phone,[Password],[Deleted]
		)
		VALUES
		(@UserName,@Email,@Address,@Phone,@Password,0)
	END
	ELSE 
	THROW 51000, 'User Already Exists. Please Login...', 1

GO
/****** Object:  StoredProcedure [dbo].[RemoveFromCart]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[RemoveFromCart]
(
	 @BookID	BIGINT = NULL,
	 @OrderID	BIGINT = NULL,
	 @Type		INT	   = NULL,
	 @UserID	BIGINT = NULL
)
AS
BEGIN
	IF @Type = 0
	BEGIN
	UPDATE OD
	SET [Quantity] = OD.[Quantity]-1,
		Order_Date = GETDATE(),
		[Total_Price] = OD.[Total_Price]-B.[Price]
		FROM Orders OD
		INNER JOIN Books B ON B.[Book_ID] = OD.[Book_ID]
		WHERE OD.[Book_ID] = @BookID
		AND [User_ID] = @UserID
	END
	ELSE
	BEGIN
	UPDATE OD
	SET [Quantity] = OD.[Quantity]+1,
		Order_Date = GETDATE(),
		[Total_Price] = OD.[Total_Price]+B.[Price]
		FROM Orders OD
		INNER JOIN Books B ON B.[Book_ID] = OD.[Book_ID]
		WHERE OD.[Book_ID] = @BookID
		AND [User_ID] = @UserID
	END
	
END


GO
/****** Object:  StoredProcedure [dbo].[UpdateCheckout]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateCheckout]
	@UserID INT = NULL,
	@UDT_OrderTable  UDT_OrderTable    READONLY
AS
 BEGIN TRY
    BEGIN TRANSACTION

			  MERGE Orders AS O USING
			  (
				SELECT [Order_ID],[Book_ID]
				FROM @UDT_OrderTable
			  )OU ON OU.[Order_ID] = O.[Order_ID] AND O.[User_ID] = @UserID
			  WHEN MATCHED THEN 
			        UPDATE SET
					O.IsPending = 0,
					Order_Date = GETDATE();
			  
	 
	 COMMIT TRANSACTION
     END TRY
   BEGIN CATCH
        ROLLBACK TRANSACTION
        RETURN -1
   END CATCH
 RETURN 1    





GO
/****** Object:  StoredProcedure [dbo].[ValidateUser]    Script Date: 12-05-2024 13:49:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[ValidateUser]
(
  @Email NVARCHAR(50) = NULL,
  @Password NVARCHAR(50) = NULL,
  @ID INT output
)
AS
	SELECT  @ID=[User_ID] from Users
	WHERE Email = @Email AND [Password] = @Password
GO
