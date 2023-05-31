CREATE DATABASE [BankingSystem]
GO
USE [BankingSystem]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[AccountNumber] [varchar](9) NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[AccountTypeID] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK__Accounts__BE2ACD6EC94E82D9] PRIMARY KEY CLUSTERED 
(
	[AccountNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountType]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountType](
	[AccountTypeID] [int] NOT NULL,
	[AccountTypeName] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Address] [varchar](100) NULL,
	[Phone] [varchar](20) NULL,
	[Email] [varchar](100) NULL,
 CONSTRAINT [PK__Customer__A4AE64B8BF289422] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionID] [uniqueidentifier] NOT NULL,
	[AccountNumber] [varchar](9) NULL,
	[TransactionDate] [datetime] NULL,
	[Amount] [decimal](18, 2) NULL,
	[TransactionTypeID] [int] NULL,
	[CustomerId] [uniqueidentifier] NULL,
 CONSTRAINT [PK__Transact__55433A4B8CDEF8CF] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionType](
	[TransactionTypeID] [int] NOT NULL,
	[TransactionTypeName] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NULL,
	[Name] [varchar](50) NULL,
	[UserId] [varchar](50) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[Accounts] ([AccountNumber], [CustomerID], [AccountTypeID], [Balance]) VALUES (N'001111111', N'6ac3f94f-b7cd-4334-b152-2094b5779118', 1, CAST(0.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Accounts] ([AccountNumber], [CustomerID], [AccountTypeID], [Balance]) VALUES (N'001122334', N'99f9edc3-7f79-4e6c-8dde-2eefd177653f', 1, CAST(1000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[AccountType] ([AccountTypeID], [AccountTypeName]) VALUES (0, N'Current')
GO
INSERT [dbo].[AccountType] ([AccountTypeID], [AccountTypeName]) VALUES (1, N'Savings')
GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Address], [Phone], [Email]) VALUES (N'6ac3f94f-b7cd-4334-b152-2094b5779118', N'SecondCustomer_First_Name', N'SecondCustomer_Last_Name', N'SecondCustomer_Address', N'9888888888', N'SecondCustomer@gmail.com')
GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Address], [Phone], [Email]) VALUES (N'99f9edc3-7f79-4e6c-8dde-2eefd177653f', N'Test_First_Name', N'Test_Last_Name', N'Test_Address', N'9999999999', N'testname@gmail.com')
GO
INSERT [dbo].[Transactions] ([TransactionID], [AccountNumber], [TransactionDate], [Amount], [TransactionTypeID], [CustomerId]) VALUES (N'8b36cf71-2877-47dd-b02a-68ab04811c94', N'001122334', CAST(N'2023-05-29T20:19:12.677' AS DateTime), CAST(1000.00 AS Decimal(18, 2)), 0, N'99f9edc3-7f79-4e6c-8dde-2eefd177653f')
GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF__Accounts__Balanc__29572725]  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK__Accounts__Accoun__2A4B4B5E] FOREIGN KEY([AccountTypeID])
REFERENCES [dbo].[AccountType] ([AccountTypeID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK__Accounts__Accoun__2A4B4B5E]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK__Accounts__Custom__2B3F6F97] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK__Accounts__Custom__2B3F6F97]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK__Transacti__Accou__2E1BDC42] FOREIGN KEY([AccountNumber])
REFERENCES [dbo].[Accounts] ([AccountNumber])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK__Transacti__Accou__2E1BDC42]
GO
/****** Object:  StoredProcedure [dbo].[TransactionsDeposit]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TransactionsDeposit]
(
	@AccountNumber varchar(9),
	@TransactionDate datetime,
	@Amount decimal,
	@TransactionTypeID int,
	@CustomerId uniqueidentifier
)
AS 
BEGIN

	INSERT INTO Transactions (TransactionID, AccountNumber, TransactionDate, Amount, TransactionTypeID, CustomerId)
	VALUES ((SELECT NEWID()), @AccountNumber, @TransactionDate, @Amount, @TransactionTypeID, @CustomerId)

	UPDATE Accounts set Balance = (SELECT Balance FROM Accounts WHERE CustomerID = @CustomerId) + @Amount  
	WHERE CustomerID = @CustomerId

END



GO
/****** Object:  StoredProcedure [dbo].[TransactionsTransfer]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TransactionsTransfer]
(
	@AccountNumber varchar(9),
	@TransactionDate datetime,
	@Amount decimal,
	@TransactionTypeID int,
	@CustomerId uniqueidentifier,
	@TransferToAccountNumber varchar(9)
)
AS 
BEGIN

	INSERT INTO Transactions (TransactionID, AccountNumber, TransactionDate, Amount, TransactionTypeID, CustomerId)
	VALUES ((SELECT NEWID()), @AccountNumber, @TransactionDate, @Amount, @TransactionTypeID, @CustomerId)
	
	UPDATE Accounts 
	set Balance = (SELECT Balance 
					FROM Accounts as a 
					inner join Customers as c
						on a.CustomerID = c.CustomerId
					WHERE AccountNumber = @AccountNumber) - @Amount  
	WHERE CustomerID = @CustomerId
	
	UPDATE Accounts 
	set Balance = (SELECT Balance 
					FROM Accounts as a 
					inner join Customers as c
						on a.CustomerID = c.CustomerId
					where AccountNumber = @TransferToAccountNumber) + @Amount  
	WHERE CustomerID = (select c.CustomerID from Customers as c inner join Accounts as a on a.CustomerID = c.CustomerID where AccountNumber = @TransferToAccountNumber)

END



GO
/****** Object:  StoredProcedure [dbo].[TransactionsWithdrawl]    Script Date: 5/29/2023 8:41:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[TransactionsWithdrawl]
(
	@AccountNumber varchar(9),
	@TransactionDate datetime,
	@Amount decimal,
	@TransactionTypeID int,
	@CustomerId uniqueidentifier
)
AS 
BEGIN

	INSERT INTO Transactions (TransactionID, AccountNumber, TransactionDate, Amount, TransactionTypeID, CustomerId)
	VALUES ((SELECT NEWID()), @AccountNumber, @TransactionDate, @Amount, @TransactionTypeID, @CustomerId)

	UPDATE Accounts set Balance = (SELECT Balance FROM Accounts WHERE CustomerID = @CustomerId) - @Amount  
	WHERE CustomerID = @CustomerId

END
GO

/****** Object:  StoredProcedure [dbo].[UserInsert]    Script Date: 5/31/2023 6:13:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[UserInsert]
(
	@Id uniqueidentifier,
	@UserId varchar(50),
	@Name varchar(50)
)
AS 
BEGIN

	INSERT INTO [User] (Id, UserId, [Name])
	VALUES (@Id, @UserId, @Name)

END
GO