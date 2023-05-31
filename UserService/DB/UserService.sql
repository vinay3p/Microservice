Create Database [UserService]
GO
Use [UserService]
/****** Object:  Table [dbo].[User]    Script Date: 5/29/2023 8:42:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[UserId] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Mobile] [varchar](20) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[User] ([Id], [Name], [UserId], [Password], [Email], [Mobile]) VALUES (N'6ac3f94f-b7cd-4334-b152-2094b5779118', N'SecondCustomer_Name', N'test', N'test@123', N'test@test.com', N'9711760888')
GO
/****** Object:  StoredProcedure [dbo].[UserInsert]    Script Date: 5/31/2023 6:16:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UserInsert]
(
	@Id uniqueidentifier,
	@UserId varchar(50),
	@Name varchar(50),
	@Password varchar(50),
	@Email varchar(50),
	@Mobile varchar(50)
)
AS 
BEGIN

	INSERT INTO [User] (Id, [Name], UserId, [Password], Email, Mobile)
	VALUES (@Id, @Name, @UserId, @Password, @Email, @Mobile)

END
GO


