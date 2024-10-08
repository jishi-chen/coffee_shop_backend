USE [CoffeeShop]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IdentityString] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Phone] [varchar](50) NULL,
	[Email] [varchar](100) NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddressArea]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressArea](
	[Id] [uniqueidentifier] NOT NULL,
	[CityId] [uniqueidentifier] NOT NULL,
	[AreaName] [nvarchar](50) NOT NULL,
	[ZipCode] [varchar](5) NOT NULL,
	[SortIndex] [int] NOT NULL,
 CONSTRAINT [PK_AddressArea] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddressCity]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressCity](
	[Id] [uniqueidentifier] NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[SortIndex] [int] NOT NULL,
 CONSTRAINT [PK_AddressCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentFieldOptions]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentFieldOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentFieldId] [int] NOT NULL,
	[OptionName] [nvarchar](50) NOT NULL,
	[MemoType] [tinyint] NOT NULL,
	[Sort] [int] NOT NULL,
 CONSTRAINT [PK_DocumentFieldOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentFields]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentFields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[DocumentId] [int] NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
	[Note] [nvarchar](50) NULL,
	[FieldType] [tinyint] NOT NULL,
	[WordLimit] [int] NOT NULL,
	[RowLimit] [int] NOT NULL,
	[FileSizeLimit] [int] NOT NULL,
	[FileExtension] [varchar](5) NULL,
	[IsRequired] [bit] NOT NULL,
	[IsIncludedExport] [bit] NOT NULL,
	[IsEditable] [bit] NOT NULL,
	[Sort] [int] NOT NULL,
	[Creator] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Updator] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentRecordDetails]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentRecordDetails](
	[SeqNo] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentRecordId] [int] NOT NULL,
	[DocumentFieldId] [int] NOT NULL,
	[FilledText] [nvarchar](50) NULL,
	[MemoText] [nvarchar](50) NULL,
	[Remark] [nvarchar](50) NULL,
 CONSTRAINT [PK_DocumentRecord] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentRecords]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentRecords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Regs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Caption] [nvarchar](50) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[Sort] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[HeadText] [nvarchar](max) NULL,
	[FooterText] [nvarchar](max) NULL,
	[Hits] [int] NOT NULL,
	[Creator] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Updator] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberInfos]    Script Date: 2024/9/30 上午 12:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberInfos](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](1000) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_MemberInfos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 

INSERT [dbo].[Accounts] ([Id], [Name], [IdentityString], [Password], [Phone], [Email], [CreateDate], [UpdateDate]) VALUES (3, N'陳', N'123', N'12345', N'0935835275', N'willyab423@gmail.com', CAST(N'2024-04-18T12:27:47.237' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO
SET IDENTITY_INSERT [dbo].[DocumentFieldOptions] ON 

INSERT [dbo].[DocumentFieldOptions] ([Id], [DocumentFieldId], [OptionName], [MemoType], [Sort]) VALUES (5, 4, N'O', 0, 0)
INSERT [dbo].[DocumentFieldOptions] ([Id], [DocumentFieldId], [OptionName], [MemoType], [Sort]) VALUES (6, 4, N'A', 0, 1)
INSERT [dbo].[DocumentFieldOptions] ([Id], [DocumentFieldId], [OptionName], [MemoType], [Sort]) VALUES (7, 4, N'B', 0, 2)
INSERT [dbo].[DocumentFieldOptions] ([Id], [DocumentFieldId], [OptionName], [MemoType], [Sort]) VALUES (8, 4, N'AB', 0, 3)
INSERT [dbo].[DocumentFieldOptions] ([Id], [DocumentFieldId], [OptionName], [MemoType], [Sort]) VALUES (11, 6, N'男', 0, 0)
INSERT [dbo].[DocumentFieldOptions] ([Id], [DocumentFieldId], [OptionName], [MemoType], [Sort]) VALUES (12, 6, N'女', 0, 1)
SET IDENTITY_INSERT [dbo].[DocumentFieldOptions] OFF
GO
SET IDENTITY_INSERT [dbo].[DocumentFields] ON 

INSERT [dbo].[DocumentFields] ([Id], [ParentId], [DocumentId], [FieldName], [Note], [FieldType], [WordLimit], [RowLimit], [FileSizeLimit], [FileExtension], [IsRequired], [IsIncludedExport], [IsEditable], [Sort], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (2, 3, 1, N'姓名', NULL, 0, 0, 0, 0, NULL, 1, 1, 1, 0, N'Admin', CAST(N'2024-04-16T18:55:35.570' AS DateTime), N'Admin', CAST(N'2024-04-16T18:56:43.433' AS DateTime))
INSERT [dbo].[DocumentFields] ([Id], [ParentId], [DocumentId], [FieldName], [Note], [FieldType], [WordLimit], [RowLimit], [FileSizeLimit], [FileExtension], [IsRequired], [IsIncludedExport], [IsEditable], [Sort], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (3, NULL, 1, N'個人資料', NULL, 10, 0, 0, 0, NULL, 1, 1, 1, 0, N'Admin', CAST(N'2024-04-16T18:56:37.090' AS DateTime), NULL, NULL)
INSERT [dbo].[DocumentFields] ([Id], [ParentId], [DocumentId], [FieldName], [Note], [FieldType], [WordLimit], [RowLimit], [FileSizeLimit], [FileExtension], [IsRequired], [IsIncludedExport], [IsEditable], [Sort], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (4, 3, 1, N'血型', NULL, 2, 0, 0, 0, NULL, 1, 1, 1, 1, N'Admin', CAST(N'2024-04-16T18:57:06.223' AS DateTime), N'Admin', CAST(N'2024-04-16T18:57:12.117' AS DateTime))
INSERT [dbo].[DocumentFields] ([Id], [ParentId], [DocumentId], [FieldName], [Note], [FieldType], [WordLimit], [RowLimit], [FileSizeLimit], [FileExtension], [IsRequired], [IsIncludedExport], [IsEditable], [Sort], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (5, 3, 1, N'星座', NULL, 0, 0, 0, 0, NULL, 1, 1, 1, 2, N'Admin', CAST(N'2024-04-16T18:57:35.067' AS DateTime), NULL, NULL)
INSERT [dbo].[DocumentFields] ([Id], [ParentId], [DocumentId], [FieldName], [Note], [FieldType], [WordLimit], [RowLimit], [FileSizeLimit], [FileExtension], [IsRequired], [IsIncludedExport], [IsEditable], [Sort], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (6, 3, 1, N'性別', NULL, 2, 0, 0, 0, NULL, 1, 1, 1, 3, N'Admin', CAST(N'2024-04-16T18:57:52.653' AS DateTime), N'Admin', CAST(N'2024-04-16T18:57:56.713' AS DateTime))
SET IDENTITY_INSERT [dbo].[DocumentFields] OFF
GO
SET IDENTITY_INSERT [dbo].[DocumentRecordDetails] ON 

INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (1, 1, 2, N'我', N'', N'')
INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (2, 1, 4, N'8', NULL, N'')
INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (3, 1, 5, N'觸女', N'', N'')
INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (4, 1, 6, N'11', NULL, N'')
INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (9, 7, 2, N'你', N'', N'')
INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (10, 7, 4, N'6', NULL, N'')
INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (11, 7, 5, N'射手', N'', N'')
INSERT [dbo].[DocumentRecordDetails] ([SeqNo], [DocumentRecordId], [DocumentFieldId], [FilledText], [MemoText], [Remark]) VALUES (12, 7, 6, N'12', NULL, N'')
SET IDENTITY_INSERT [dbo].[DocumentRecordDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[DocumentRecords] ON 

INSERT [dbo].[DocumentRecords] ([Id], [DocumentId], [Name]) VALUES (1, 1, N'willy')
INSERT [dbo].[DocumentRecords] ([Id], [DocumentId], [Name]) VALUES (7, 1, N'Admin')
SET IDENTITY_INSERT [dbo].[DocumentRecords] OFF
GO
SET IDENTITY_INSERT [dbo].[Documents] ON 

INSERT [dbo].[Documents] ([Id], [Caption], [IsEnabled], [Sort], [StartDate], [EndDate], [HeadText], [FooterText], [Hits], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (1, N'問卷測試', 1, 0, CAST(N'2024-04-16T00:00:00.000' AS DateTime), CAST(N'2024-08-24T00:00:00.000' AS DateTime), N'<p>我是表頭</p>', N'<p>我是表尾</p>', 0, N'Admin', CAST(N'2024-04-16T18:22:25.940' AS DateTime), N'Admin', CAST(N'2024-04-16T18:58:20.500' AS DateTime))
SET IDENTITY_INSERT [dbo].[Documents] OFF
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Document_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Document_Hits]  DEFAULT ((0)) FOR [Hits]
GO
