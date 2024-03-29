USE [master]
GO

/****** Object:  Database [MonitorService]    Script Date: 29-05-2023 21:07:41 ******/
CREATE DATABASE [MonitorService]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MonitorService', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\MonitorService.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MonitorService_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\MonitorService_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MonitorService].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [MonitorService] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [MonitorService] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [MonitorService] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [MonitorService] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [MonitorService] SET ARITHABORT OFF 
GO

ALTER DATABASE [MonitorService] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [MonitorService] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [MonitorService] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [MonitorService] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [MonitorService] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [MonitorService] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [MonitorService] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [MonitorService] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [MonitorService] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [MonitorService] SET  DISABLE_BROKER 
GO

ALTER DATABASE [MonitorService] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [MonitorService] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [MonitorService] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [MonitorService] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [MonitorService] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [MonitorService] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [MonitorService] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [MonitorService] SET RECOVERY FULL 
GO

ALTER DATABASE [MonitorService] SET  MULTI_USER 
GO

ALTER DATABASE [MonitorService] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [MonitorService] SET DB_CHAINING OFF 
GO

ALTER DATABASE [MonitorService] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [MonitorService] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [MonitorService] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [MonitorService] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [MonitorService] SET QUERY_STORE = ON
GO

ALTER DATABASE [MonitorService] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [MonitorService] SET  READ_WRITE 
GO

USE [MonitorService]
GO

/****** Object:  Table [dbo].[Frauds]    Script Date: 29-05-2023 23:02:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Frauds](
	[AccountNumber] [varchar](50) NOT NULL,
	[TransactionTypeId] [int] NULL,
	[TransferToAccountNumber] [varchar](50) NULL,
	[Amount] [decimal](18, 0) NULL,
	[Message] [varchar](max) NULL,
	[CreatedOn] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [MonitorService]
GO

/****** Object:  StoredProcedure [dbo].[FraudInsert]    Script Date: 29-05-2023 21:08:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FraudInsert]
(
	@AccountNumber varchar(9),
	@TransactionTypeId int,
	@TransferToAccountNumber varchar(9),
	@Amount decimal,
	@Message varchar(300),
	@CreatedDate datetime
)
AS 
BEGIN

	INSERT INTO Frauds(AccountNumber, TransactionTypeId, TransferToAccountNumber, Amount, [Message], CreatedOn )
	VALUES (@AccountNumber,@TransactionTypeId, @TransferToAccountNumber, @Amount, @Message, @CreatedDate)

END
GO
