USE [master]
GO
/****** Object:  Database [DropboxAnalog]    Script Date: 01.05.2017 20:29:21 ******/
CREATE DATABASE [DropboxAnalog]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DropboxAnalog', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS2016\MSSQL\DATA\DropboxAnalog.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DropboxAnalog_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS2016\MSSQL\DATA\DropboxAnalog_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [DropboxAnalog] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DropboxAnalog].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DropboxAnalog] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DropboxAnalog] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DropboxAnalog] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DropboxAnalog] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DropboxAnalog] SET ARITHABORT OFF 
GO
ALTER DATABASE [DropboxAnalog] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DropboxAnalog] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DropboxAnalog] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DropboxAnalog] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DropboxAnalog] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DropboxAnalog] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DropboxAnalog] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DropboxAnalog] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DropboxAnalog] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DropboxAnalog] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DropboxAnalog] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DropboxAnalog] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DropboxAnalog] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DropboxAnalog] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DropboxAnalog] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DropboxAnalog] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DropboxAnalog] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DropboxAnalog] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DropboxAnalog] SET  MULTI_USER 
GO
ALTER DATABASE [DropboxAnalog] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DropboxAnalog] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DropboxAnalog] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DropboxAnalog] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DropboxAnalog] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DropboxAnalog] SET QUERY_STORE = OFF
GO
USE [DropboxAnalog]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [DropboxAnalog]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 01.05.2017 20:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [uniqueidentifier] NOT NULL,
	[Text] [nvarchar](4000) NOT NULL,
	[PostTime] [datetimeoffset](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Files]    Script Date: 01.05.2017 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NULL,
	[Name] [nvarchar](255) NOT NULL,
	[FileType] [nvarchar](55) NULL,
	[Size] [bigint] NULL,
	[CreationTime] [datetimeoffset](7) NOT NULL,
	[LastWriteTime] [datetimeoffset](7) NOT NULL,
	[LastAccessTime] [datetimeoffset](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Shared]    Script Date: 01.05.2017 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shared](
	[UserId] [int] NOT NULL,
	[FileId] [uniqueidentifier] NOT NULL,
	[IsUserOwner] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsReadonly] [bit] NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 01.05.2017 20:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[SecondName] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[AddDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_Shared1]    Script Date: 01.05.2017 20:29:22 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Shared1] ON [dbo].[Shared]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT (sysdatetime()) FOR [PostTime]
GO
ALTER TABLE [dbo].[Shared] ADD  DEFAULT ((1)) FOR [IsUserOwner]
GO
ALTER TABLE [dbo].[Shared] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Shared] ADD  DEFAULT ((0)) FOR [IsReadonly]
GO
ALTER TABLE [dbo].[Shared] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (sysdatetime()) FOR [AddDate]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD FOREIGN KEY([Id])
REFERENCES [dbo].[Shared] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shared]  WITH CHECK ADD  CONSTRAINT [FK__Shared__FileId__2D27B809] FOREIGN KEY([FileId])
REFERENCES [dbo].[Files] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shared] CHECK CONSTRAINT [FK__Shared__FileId__2D27B809]
GO
ALTER TABLE [dbo].[Shared]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
USE [master]
GO
ALTER DATABASE [DropboxAnalog] SET  READ_WRITE 
GO
