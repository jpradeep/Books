USE [Book]
GO
/****** Object:  Table [dbo].[authors]    Script Date: 26-10-2021 01:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authors](
	[authorid] [int] IDENTITY(1,1) NOT NULL,
	[authorname] [varchar](100) NOT NULL,
	[country] [varchar](50) NULL,
	[createdAt] [datetime] NULL,
	[modifiedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[authorid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[books]    Script Date: 26-10-2021 01:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[books](
	[ISBN] [bigint] NOT NULL,
	[title] [varchar](max) NOT NULL,
	[publicationDate] [date] NOT NULL,
	[createdAt] [datetime] NULL,
	[modifiedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ISBN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthorBook]    Script Date: 26-10-2021 01:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthorBook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Fk_ISBN] [bigint] NULL,
	[Fk_authorid] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[BookDetails]    Script Date: 26-10-2021 01:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[BookDetails] as select b.ISBN, b.title, b.publicationDate, a.authorid, a.authorname, a.country from [Book].[dbo].[books] b 
left join [Book].[dbo].AuthorBook ab on b.ISBN = ab.Fk_ISBN
left join [Book].[dbo].authors a on a.authorid = ab.Fk_authorid
GO
ALTER TABLE [dbo].[authors] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[authors] ADD  DEFAULT (getdate()) FOR [modifiedAt]
GO
ALTER TABLE [dbo].[books] ADD  DEFAULT (getdate()) FOR [createdAt]
GO
ALTER TABLE [dbo].[books] ADD  DEFAULT (getdate()) FOR [modifiedAt]
GO
