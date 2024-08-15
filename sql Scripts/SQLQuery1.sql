USE [Forte_Invoice]
GO

/****** Object:  Table [dbo].[Invoice_one]    Script Date: 1/9/2015 3:19:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Invoice_two](
	[Applicant] [nvarchar](max) NULL,
	[Consignee] [nvarchar](max) NULL,
	[Invoice_date] [datetime] NULL,
	[Invoice_number] [nvarchar](50) NULL,
	[FTI_number] [nchar](25) NULL,
	[LC_number] [nchar](25) NULL,
	[LC_date] [datetime] NULL,
	[Contact_PO] [nchar](25) NULL,
	[Covering] [nvarchar](max) NULL,
	[Quilty_condition] [nchar](25) NULL,
	[Packing] [nvarchar](50) NULL,
	[Ship_date] [datetime] NULL,
	[Ship_via] [nvarchar](50) NULL,
	[Ship_terms] [nchar](25) NULL,
	[Shipping_mark] [nvarchar](max) NULL,
	[Price_extra] [nchar](10) NULL,
	[Price] [money] NULL,
	[extra1] [nchar](10) NULL,
	[extra2] [nchar](10) NULL,
	[extra3] [nchar](10) NULL,
	[extra4] [nchar](10) NULL,
	[extra5] [nchar](10) NULL,
	[extra6] [nchar](10) NULL,
	[extra7] [nchar](10) NULL,
	[extra8] [nchar](10) NULL,
	[extra9] [nchar](10) NULL,
	[extra10] [nchar](10) NULL,
	[extra11] [nchar](10) NULL,
	[extra12] [nchar](10) NULL,
	[extra13] [nchar](10) NULL,
	[extra14] [nchar](10) NULL,
	[extra15] [nchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


