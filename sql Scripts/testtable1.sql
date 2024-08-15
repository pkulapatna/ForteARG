USE [Forte_Test]
GO

/****** Object:  Table [dbo].[table_one]Script Date: 7/26/2017 12:35:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[table_one](
	[Index] [int] NULL,
	[StockName] [nvarchar](255) NULL,
	[CalibrationName] [nvarchar](255) NULL,
	[GradeName] [nvarchar](255) NULL,
	[Weight] [real] NULL,
	[Forte] [int] NULL,
	[Moisture] [real] NULL,
	[SerialNumber] [int] NULL,
	[LotNumber] [int] NULL
) ON [PRIMARY]

GO


