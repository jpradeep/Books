USE [Book]
GO

/****** Object:  View [dbo].[BookDetails]    Script Date: 26-10-2021 01:14:29 ******/
DROP VIEW [dbo].[BookDetails]
GO

/****** Object:  View [dbo].[BookDetails]    Script Date: 26-10-2021 01:14:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create View [dbo].[BookDetails] as select b.ISBN, b.title, b.publicationDate, a.authorid, a.authorname, a.country from [Book].[dbo].[books] b 
left join [Book].[dbo].AuthorBook ab on b.ISBN = ab.Fk_ISBN
left join [Book].[dbo].authors a on a.authorid = ab.Fk_authorid
GO


