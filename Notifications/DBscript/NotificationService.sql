Create database [NotificationService]
GO
Use [NotificationService]
Go

/****** Object:  Table [dbo].[Notifications]    Script Date: 5/29/2023 8:34:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[NotificationID] [uniqueidentifier] NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[Message] [varchar](300) NOT NULL,
	[NotificationDate] [datetime] NOT NULL,
	[AccountNumber] [varchar](9) NOT NULL,
	[TransactionTypeId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[TransferToAccountNumber] [varchar](9) NULL,
 CONSTRAINT [PK__Notifica__20CF2E32002ED81B] PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[NotificationsInsert]    Script Date: 5/29/2023 8:34:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[NotificationsInsert]
(
	@CustomerId uniqueidentifier,
	@AccountNumber varchar(9),
	@Message varchar(300),
	@NotificationDate datetime,
	@TransactionTypeId int,
	@Amount decimal,
	@TransferToAccountNumber varchar(9)
)
AS 
BEGIN

	INSERT INTO Notifications(NotificationId, CustomerId, [Message], NotificationDate, AccountNumber, TransactionTypeId, Amount, TransferToAccountNumber )
	VALUES ((SELECT NEWID()), @CustomerId, @Message, @NotificationDate, @AccountNumber, @TransactionTypeId, @Amount, @TransferToAccountNumber )

END
GO