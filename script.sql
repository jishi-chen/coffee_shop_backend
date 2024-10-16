USE [CoffeeShop]
GO
/****** Object:  Table [dbo].[AddressArea]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressArea](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CityId] [int] NOT NULL,
	[AreaName] [nvarchar](50) NOT NULL,
	[ZipCode] [varchar](5) NOT NULL,
	[SortIndex] [int] NOT NULL,
 CONSTRAINT [PK_AddressArea_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddressCity]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressCity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[SortIndex] [int] NOT NULL,
 CONSTRAINT [PK_AddressCity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Sort] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[UpdateDate] [datetime] NOT NULL,
	[Updator] [int] NOT NULL,
 CONSTRAINT [PK__Categori__19093A0B5A7E0A9C] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentFieldOptions]    Script Date: 2024/10/18 上午 11:59:52 ******/
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
/****** Object:  Table [dbo].[DocumentFields]    Script Date: 2024/10/18 上午 11:59:52 ******/
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
/****** Object:  Table [dbo].[DocumentRecordDetails]    Script Date: 2024/10/18 上午 11:59:52 ******/
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
/****** Object:  Table [dbo].[DocumentRecords]    Script Date: 2024/10/18 上午 11:59:52 ******/
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
/****** Object:  Table [dbo].[Documents]    Script Date: 2024/10/18 上午 11:59:52 ******/
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
/****** Object:  Table [dbo].[FileStorages]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileStorages](
	[FileStorageId] [int] IDENTITY(1,1) NOT NULL,
	[OriginalFileName] [nvarchar](255) NOT NULL,
	[NewFileName] [nvarchar](255) NOT NULL,
	[FilePath] [nvarchar](500) NOT NULL,
	[FileSize] [bigint] NULL,
	[ContentType] [nvarchar](100) NULL,
	[UploadDate] [datetime] NOT NULL,
	[ModuleType] [varchar](50) NULL,
	[CategoryType] [nvarchar](50) NULL,
	[UploadedBy] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FileStorages] PRIMARY KEY CLUSTERED 
(
	[FileStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
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
/****** Object:  Table [dbo].[Modules]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modules](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](100) NOT NULL,
	[Sort] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[UpdateDate] [datetime] NOT NULL,
	[Updator] [int] NOT NULL,
 CONSTRAINT [PK__Modules__2B7477A7B3A3BDD5] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tenants]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenants](
	[TenantId] [int] IDENTITY(1,1) NOT NULL,
	[TenantName] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](255) NULL,
	[ContactEmail] [nvarchar](100) NOT NULL,
	[ContactName] [nvarchar](100) NOT NULL,
	[ContactPhone] [varchar](50) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Updator] [int] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED 
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024/10/18 上午 11:59:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[AddressId] [int] NULL,
	[Address] [nvarchar](100) NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Role] [tinyint] NOT NULL,
	[Gender] [tinyint] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsEnabled] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Creator] [int] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[Updator] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AddressArea] ON 

INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (1, 2, N'中正區', N'100', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (2, 2, N'大同區', N'103', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (3, 2, N'中山區', N'104', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (4, 2, N'松山區', N'105', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (5, 2, N'大安區', N'106', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (6, 2, N'萬華區', N'108', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (7, 2, N'信義區', N'110', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (8, 2, N'士林區', N'111', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (9, 2, N'北投區', N'112', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (10, 2, N'內湖區', N'114', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (11, 2, N'南港區', N'115', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (12, 2, N'文山區', N'116', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (13, 3, N'仁愛區', N'200', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (14, 3, N'信義區', N'201', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (15, 3, N'中正區', N'202', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (16, 3, N'中山區', N'203', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (17, 3, N'安樂區', N'204', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (18, 3, N'暖暖區', N'205', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (19, 3, N'七堵區', N'206', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (20, 4, N'萬里區', N'207', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (21, 4, N'金山區', N'208', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (22, 4, N'板橋區', N'220', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (23, 4, N'汐止區', N'221', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (24, 4, N'深坑區', N'222', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (25, 4, N'石碇區', N'223', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (26, 4, N'瑞芳區', N'224', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (27, 4, N'平溪區', N'226', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (28, 4, N'雙溪區', N'227', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (29, 4, N'貢寮區', N'228', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (30, 4, N'新店區', N'231', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (31, 4, N'坪林區', N'232', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (32, 4, N'烏來區', N'233', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (33, 4, N'永和區', N'234', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (34, 4, N'中和區', N'235', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (35, 4, N'土城區', N'236', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (36, 4, N'三峽區', N'237', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (37, 4, N'樹林區', N'238', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (38, 4, N'鶯歌區', N'239', 18)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (39, 4, N'三重區', N'241', 19)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (40, 4, N'新莊區', N'242', 20)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (41, 4, N'泰山區', N'243', 21)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (42, 4, N'林口區', N'244', 22)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (43, 4, N'蘆洲區', N'247', 23)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (44, 4, N'五股區', N'248', 24)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (45, 4, N'八里區', N'249', 25)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (46, 4, N'淡水區', N'251', 26)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (47, 4, N'三芝區', N'252', 27)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (48, 4, N'石門區', N'253', 28)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (49, 5, N'南竿鄉', N'209', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (50, 5, N'北竿鄉', N'210', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (51, 5, N'莒光鄉', N'211', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (52, 5, N'東引鄉', N'212', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (53, 6, N'宜蘭市', N'260', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (54, 6, N'壯圍鄉', N'263', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (55, 6, N'頭城鎮', N'261', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (56, 6, N'礁溪鄉', N'262', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (57, 6, N'員山鄉', N'264', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (58, 6, N'羅東鎮', N'265', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (59, 6, N'三星鄉', N'266', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (60, 6, N'大同鄉', N'267', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (61, 6, N'五結鄉', N'268', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (62, 6, N'冬山鄉', N'269', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (63, 6, N'蘇澳鎮', N'270', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (64, 6, N'南澳鄉', N'272', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (65, 6, N'釣魚臺', N'290', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (66, 7, N'釣魚臺', N'290', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (67, 8, N'東區', N'300', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (68, 8, N'北區', N'300', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (69, 8, N'香山區', N'300', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (70, 9, N'寶山鄉', N'308', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (71, 9, N'竹北市', N'302', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (72, 9, N'湖口鄉', N'303', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (73, 9, N'新豐鄉', N'304', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (74, 9, N'新埔鎮', N'305', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (75, 9, N'關西鎮', N'306', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (76, 9, N'芎林鄉', N'307', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (77, 9, N'竹東鎮', N'310', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (78, 9, N'五峰鄉', N'311', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (79, 9, N'橫山鄉', N'312', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (80, 9, N'尖石鄉', N'313', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (81, 9, N'北埔鄉', N'314', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (82, 9, N'峨眉鄉', N'315', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (83, 10, N'中壢區', N'320', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (84, 10, N'平鎮區', N'324', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (85, 10, N'龍潭區', N'325', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (86, 10, N'楊梅區', N'326', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (87, 10, N'新屋區', N'327', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (88, 10, N'觀音區', N'328', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (89, 10, N'桃園區', N'330', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (90, 10, N'龜山區', N'333', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (91, 10, N'八德區', N'334', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (92, 10, N'大溪區', N'335', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (93, 10, N'復興區', N'336', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (94, 10, N'大園區', N'337', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (95, 10, N'蘆竹區', N'338', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (96, 11, N'竹南鎮', N'350', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (97, 11, N'頭份市', N'351', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (98, 11, N'三灣鄉', N'352', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (99, 11, N'南庄鄉', N'353', 3)
GO
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (100, 11, N'獅潭鄉', N'354', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (101, 11, N'後龍鎮', N'356', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (102, 11, N'通霄鎮', N'357', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (103, 11, N'苑裡鎮', N'358', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (104, 11, N'苗栗市', N'360', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (105, 11, N'造橋鄉', N'361', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (106, 11, N'頭屋鄉', N'362', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (107, 11, N'公館鄉', N'363', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (108, 11, N'大湖鄉', N'364', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (109, 11, N'泰安鄉', N'365', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (110, 11, N'銅鑼鄉', N'366', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (111, 11, N'三義鄉', N'367', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (112, 11, N'西湖鄉', N'368', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (113, 11, N'卓蘭鎮', N'369', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (114, 12, N'中區', N'400', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (115, 12, N'東區', N'401', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (116, 12, N'南區', N'402', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (117, 12, N'西區', N'403', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (118, 12, N'北區', N'404', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (119, 12, N'北屯區', N'406', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (120, 12, N'西屯區', N'407', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (121, 12, N'南屯區', N'408', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (122, 12, N'太平區', N'411', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (123, 12, N'大里區', N'412', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (124, 12, N'霧峰區', N'413', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (125, 12, N'烏日區', N'414', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (126, 12, N'豐原區', N'420', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (127, 12, N'后里區', N'421', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (128, 12, N'石岡區', N'422', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (129, 12, N'東勢區', N'423', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (130, 12, N'和平區', N'424', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (131, 12, N'新社區', N'426', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (132, 12, N'潭子區', N'427', 18)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (133, 12, N'大雅區', N'428', 19)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (134, 12, N'神岡區', N'429', 20)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (135, 12, N'大肚區', N'432', 21)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (136, 12, N'沙鹿區', N'433', 22)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (137, 12, N'龍井區', N'434', 23)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (138, 12, N'梧棲區', N'435', 24)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (139, 12, N'清水區', N'436', 25)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (140, 12, N'大甲區', N'437', 26)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (141, 12, N'外埔區', N'438', 27)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (142, 12, N'大安區', N'439', 28)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (143, 13, N'彰化市', N'500', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (144, 13, N'芬園鄉', N'502', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (145, 13, N'花壇鄉', N'503', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (146, 13, N'秀水鄉', N'504', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (147, 13, N'鹿港鎮', N'505', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (148, 13, N'福興鄉', N'506', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (149, 13, N'線西鄉', N'507', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (150, 13, N'和美鎮', N'508', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (151, 13, N'伸港鄉', N'509', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (152, 13, N'員林市', N'510', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (153, 13, N'社頭鄉', N'511', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (154, 13, N'永靖鄉', N'512', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (155, 13, N'埔心鄉', N'513', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (156, 13, N'溪湖鎮', N'514', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (157, 13, N'大村鄉', N'515', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (158, 13, N'埔鹽鄉', N'516', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (159, 13, N'田中鎮', N'520', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (160, 13, N'北斗鎮', N'521', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (161, 13, N'田尾鄉', N'522', 18)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (162, 13, N'埤頭鄉', N'523', 19)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (163, 13, N'溪州鄉', N'524', 20)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (164, 13, N'竹塘鄉', N'525', 21)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (165, 13, N'二林鎮', N'526', 22)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (166, 13, N'大城鄉', N'527', 23)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (167, 13, N'芳苑鄉', N'528', 24)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (168, 13, N'二水鄉', N'530', 25)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (169, 14, N'南投市', N'540', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (170, 14, N'中寮鄉', N'541', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (171, 14, N'草屯鎮', N'542', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (172, 14, N'國姓鄉', N'544', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (173, 14, N'埔里鎮', N'545', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (174, 14, N'仁愛鄉', N'546', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (175, 14, N'名間鄉', N'551', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (176, 14, N'集集鎮', N'552', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (177, 14, N'水里鄉', N'553', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (178, 14, N'魚池鄉', N'555', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (179, 14, N'信義鄉', N'556', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (180, 14, N'竹山鎮', N'557', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (181, 14, N'鹿谷鄉', N'558', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (182, 15, N'西區', N'600', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (183, 15, N'東區', N'600', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (184, 16, N'番路鄉', N'602', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (185, 16, N'梅山鄉', N'603', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (186, 16, N'竹崎鄉', N'604', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (187, 16, N'阿里山鄉', N'605', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (188, 16, N'中埔鄉', N'606', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (189, 16, N'大埔鄉', N'607', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (190, 16, N'水上鄉', N'608', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (191, 16, N'鹿草鄉', N'611', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (192, 16, N'太保市', N'612', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (193, 16, N'朴子市', N'613', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (194, 16, N'東石鄉', N'614', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (195, 16, N'六腳鄉', N'615', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (196, 16, N'新港鄉', N'616', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (197, 16, N'民雄鄉', N'621', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (198, 16, N'大林鎮', N'622', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (199, 16, N'溪口鄉', N'623', 15)
GO
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (200, 16, N'義竹鄉', N'624', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (201, 16, N'布袋鎮', N'625', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (202, 17, N'斗南鎮', N'630', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (203, 17, N'大埤鄉', N'631', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (204, 17, N'虎尾鎮', N'632', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (205, 17, N'土庫鎮', N'633', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (206, 17, N'褒忠鄉', N'634', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (207, 17, N'東勢鄉', N'635', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (208, 17, N'臺西鄉', N'636', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (209, 17, N'崙背鄉', N'637', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (210, 17, N'麥寮鄉', N'638', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (211, 17, N'斗六市', N'640', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (212, 17, N'林內鄉', N'643', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (213, 17, N'古坑鄉', N'646', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (214, 17, N'莿桐鄉', N'647', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (215, 17, N'西螺鎮', N'648', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (216, 17, N'二崙鄉', N'649', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (217, 17, N'北港鎮', N'651', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (218, 17, N'水林鄉', N'652', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (219, 17, N'口湖鄉', N'653', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (220, 17, N'四湖鄉', N'654', 18)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (221, 17, N'元長鄉', N'655', 19)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (222, 18, N'中西區', N'700', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (223, 18, N'東區', N'701', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (224, 18, N'南區', N'702', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (225, 18, N'北區', N'704', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (226, 18, N'安平區', N'708', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (227, 18, N'安南區', N'709', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (228, 18, N'永康區', N'710', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (229, 18, N'歸仁區', N'711', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (230, 18, N'新化區', N'712', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (231, 18, N'左鎮區', N'713', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (232, 18, N'玉井區', N'714', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (233, 18, N'楠西區', N'715', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (234, 18, N'南化區', N'716', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (235, 18, N'仁德區', N'717', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (236, 18, N'關廟區', N'718', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (237, 18, N'龍崎區', N'719', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (238, 18, N'官田區', N'720', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (239, 18, N'麻豆區', N'721', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (240, 18, N'佳里區', N'722', 18)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (241, 18, N'西港區', N'723', 19)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (242, 18, N'七股區', N'724', 20)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (243, 18, N'將軍區', N'725', 21)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (244, 18, N'學甲區', N'726', 22)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (245, 18, N'北門區', N'727', 23)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (246, 18, N'新營區', N'730', 24)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (247, 18, N'後壁區', N'731', 25)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (248, 18, N'白河區', N'732', 26)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (249, 18, N'東山區', N'733', 27)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (250, 18, N'六甲區', N'734', 28)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (251, 18, N'下營區', N'735', 29)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (252, 18, N'柳營區', N'736', 30)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (253, 18, N'鹽水區', N'737', 31)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (254, 18, N'善化區', N'741', 32)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (255, 18, N'新市區', N'744', 33)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (256, 18, N'大內區', N'742', 34)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (257, 18, N'山上區', N'743', 35)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (258, 18, N'安定區', N'745', 36)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (259, 19, N'新興區', N'800', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (260, 19, N'前金區', N'801', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (261, 19, N'苓雅區', N'802', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (262, 19, N'鹽埕區', N'803', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (263, 19, N'鼓山區', N'804', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (264, 19, N'旗津區', N'805', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (265, 19, N'前鎮區', N'806', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (266, 19, N'三民區', N'807', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (267, 19, N'楠梓區', N'811', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (268, 19, N'小港區', N'812', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (269, 19, N'左營區', N'813', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (270, 19, N'仁武區', N'814', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (271, 19, N'大社區', N'815', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (272, 19, N'東沙群島', N'817', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (273, 19, N'南沙群島', N'819', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (274, 19, N'岡山區', N'820', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (275, 19, N'路竹區', N'821', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (276, 19, N'阿蓮區', N'822', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (277, 19, N'田寮區', N'823', 18)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (278, 19, N'燕巢區', N'824', 19)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (279, 19, N'橋頭區', N'825', 20)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (280, 19, N'梓官區', N'826', 21)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (281, 19, N'彌陀區', N'827', 22)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (282, 19, N'永安區', N'828', 23)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (283, 19, N'湖內區', N'829', 24)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (284, 19, N'鳳山區', N'830', 25)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (285, 19, N'大寮區', N'831', 26)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (286, 19, N'林園區', N'832', 27)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (287, 19, N'鳥松區', N'833', 28)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (288, 19, N'大樹區', N'840', 29)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (289, 19, N'旗山區', N'842', 30)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (290, 19, N'美濃區', N'843', 31)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (291, 19, N'六龜區', N'844', 32)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (292, 19, N'內門區', N'845', 33)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (293, 19, N'杉林區', N'846', 34)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (294, 19, N'甲仙區', N'847', 35)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (295, 19, N'桃源區', N'848', 36)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (296, 19, N'那瑪夏區', N'849', 37)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (297, 19, N'茂林區', N'851', 38)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (298, 19, N'茄萣區', N'852', 39)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (299, 20, N'東沙群島', N'817', 0)
GO
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (300, 20, N'南沙群島', N'819', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (301, 21, N'馬公市', N'880', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (302, 21, N'西嶼鄉', N'881', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (303, 21, N'望安鄉', N'882', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (304, 21, N'七美鄉', N'883', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (305, 21, N'白沙鄉', N'884', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (306, 21, N'湖西鄉', N'885', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (307, 22, N'金沙鎮', N'890', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (308, 22, N'金湖鎮', N'891', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (309, 22, N'金寧鄉', N'892', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (310, 22, N'金城鎮', N'893', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (311, 22, N'烈嶼鄉', N'894', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (312, 22, N'烏坵鄉', N'896', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (313, 23, N'屏東市', N'900', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (314, 23, N'三地門鄉', N'901', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (315, 23, N'霧臺鄉', N'902', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (316, 23, N'瑪家鄉', N'903', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (317, 23, N'九如鄉', N'904', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (318, 23, N'里港鄉', N'905', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (319, 23, N'高樹鄉', N'906', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (320, 23, N'鹽埔鄉', N'907', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (321, 23, N'長治鄉', N'908', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (322, 23, N'麟洛鄉', N'909', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (323, 23, N'竹田鄉', N'911', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (324, 23, N'內埔鄉', N'912', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (325, 23, N'萬丹鄉', N'913', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (326, 23, N'潮州鎮', N'920', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (327, 23, N'泰武鄉', N'921', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (328, 23, N'來義鄉', N'922', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (329, 23, N'萬巒鄉', N'923', 16)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (330, 23, N'崁頂鄉', N'924', 17)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (331, 23, N'新埤鄉', N'925', 18)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (332, 23, N'南州鄉', N'926', 19)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (333, 23, N'林邊鄉', N'927', 20)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (334, 23, N'東港鎮', N'928', 21)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (335, 23, N'琉球鄉', N'929', 22)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (336, 23, N'佳冬鄉', N'931', 23)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (337, 23, N'新園鄉', N'932', 24)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (338, 23, N'枋寮鄉', N'940', 25)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (339, 23, N'枋山鄉', N'941', 26)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (340, 23, N'春日鄉', N'942', 27)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (341, 23, N'獅子鄉', N'943', 28)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (342, 23, N'車城鄉', N'944', 29)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (343, 23, N'牡丹鄉', N'945', 30)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (344, 23, N'恆春鎮', N'946', 31)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (345, 23, N'滿州鄉', N'947', 32)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (346, 24, N'臺東市', N'950', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (347, 24, N'綠島鄉', N'951', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (348, 24, N'蘭嶼鄉', N'952', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (349, 24, N'延平鄉', N'953', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (350, 24, N'卑南鄉', N'954', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (351, 24, N'鹿野鄉', N'955', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (352, 24, N'關山鎮', N'956', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (353, 24, N'海端鄉', N'957', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (354, 24, N'池上鄉', N'958', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (355, 24, N'東河鄉', N'959', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (356, 24, N'成功鎮', N'961', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (357, 24, N'長濱鄉', N'962', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (358, 24, N'太麻里鄉', N'963', 12)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (359, 24, N'金峰鄉', N'964', 13)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (360, 24, N'大武鄉', N'965', 14)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (361, 24, N'達仁鄉', N'966', 15)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (362, 25, N'花蓮市', N'970', 0)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (363, 25, N'新城鄉', N'971', 1)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (364, 25, N'秀林鄉', N'972', 2)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (365, 25, N'吉安鄉', N'973', 3)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (366, 25, N'壽豐鄉', N'974', 4)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (367, 25, N'鳳林鎮', N'975', 5)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (368, 25, N'光復鄉', N'976', 6)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (369, 25, N'豐濱鄉', N'977', 7)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (370, 25, N'瑞穗鄉', N'978', 8)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (371, 25, N'萬榮鄉', N'979', 9)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (372, 25, N'玉里鎮', N'981', 10)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (373, 25, N'卓溪鄉', N'982', 11)
INSERT [dbo].[AddressArea] ([Id], [CityId], [AreaName], [ZipCode], [SortIndex]) VALUES (374, 25, N'富里鄉', N'983', 12)
SET IDENTITY_INSERT [dbo].[AddressArea] OFF
GO
SET IDENTITY_INSERT [dbo].[AddressCity] ON 

INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (2, N'臺北市', 0)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (3, N'基隆市', 1)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (4, N'新北市', 2)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (5, N'連江縣', 3)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (6, N'宜蘭縣', 4)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (7, N'釣魚臺', 5)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (8, N'新竹市', 6)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (9, N'新竹縣', 7)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (10, N'桃園市', 8)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (11, N'苗栗縣', 9)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (12, N'臺中市', 10)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (13, N'彰化縣', 11)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (14, N'南投縣', 12)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (15, N'嘉義市', 13)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (16, N'嘉義縣', 14)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (17, N'雲林縣', 15)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (18, N'臺南市', 16)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (19, N'高雄市', 17)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (20, N'南海島', 18)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (21, N'澎湖縣', 19)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (22, N'金門縣', 20)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (23, N'屏東縣', 21)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (24, N'臺東縣', 22)
INSERT [dbo].[AddressCity] ([Id], [CityName], [SortIndex]) VALUES (25, N'花蓮縣', 23)
SET IDENTITY_INSERT [dbo].[AddressCity] OFF
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
SET IDENTITY_INSERT [dbo].[FileStorages] ON 

INSERT [dbo].[FileStorages] ([FileStorageId], [OriginalFileName], [NewFileName], [FilePath], [FileSize], [ContentType], [UploadDate], [ModuleType], [CategoryType], [UploadedBy], [Description], [IsDeleted]) VALUES (9, N'4_0.png', N'20241017114117018.png', N'20241017114117018.png', 1284927, N'image/png', CAST(N'2024-10-17T11:41:17.247' AS DateTime), N'File', N'File', 0, NULL, 0)
SET IDENTITY_INSERT [dbo].[FileStorages] OFF
GO
SET IDENTITY_INSERT [dbo].[Tenants] ON 

INSERT [dbo].[Tenants] ([TenantId], [TenantName], [Address], [ContactEmail], [ContactName], [ContactPhone], [IsEnabled], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (3, N'我的公司', NULL, N'willyab423@gmail.com', N'Willy', N'0935835275', 1, 0, CAST(N'2024-09-30T15:07:40.233' AS DateTime), 0, CAST(N'2024-10-04T17:31:22.043' AS DateTime))
INSERT [dbo].[Tenants] ([TenantId], [TenantName], [Address], [ContactEmail], [ContactName], [ContactPhone], [IsEnabled], [Creator], [CreateDate], [Updator], [UpdateDate]) VALUES (4, N'另一間', NULL, N'karin@gmail.com', N'Karin', N'0935835275', 1, 0, CAST(N'2024-09-30T17:06:42.627' AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Tenants] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [TenantId], [UserName], [Email], [AddressId], [Address], [PasswordHash], [Role], [Gender], [Description], [IsEnabled], [CreateDate], [Creator], [UpdateDate], [Updator]) VALUES (0, 4, N'willy', N'willyab423@gmail.com', 60, N'8888', N'AQAAAAIAAYagAAAAEMXx8X/Dl7wJO7FW+cC/fJHuYP5Xx0DNvJvaEIGGhlI5j8davWkrQgFCHisRDUWwVg==', 0, 2, N'<p>test</p>', 1, CAST(N'2024-09-30T14:12:42.953' AS DateTime), 0, CAST(N'2024-10-04T17:45:41.777' AS DateTime), 0)
INSERT [dbo].[Users] ([UserId], [TenantId], [UserName], [Email], [AddressId], [Address], [PasswordHash], [Role], [Gender], [Description], [IsEnabled], [CreateDate], [Creator], [UpdateDate], [Updator]) VALUES (1, 3, N'karin', N'karin@gmail.com', 239, N'8888', N'AQAAAAIAAYagAAAAEG+mldn04yuwhKx3uDKd8lwqb2h5R0Lzf/F+NJ76fUNqMmJMlVKgwopBoLUq+Ekqpw==', 0, 1, N'<p>aaaa</p>', 1, CAST(N'2024-09-30T17:27:00.657' AS DateTime), 0, NULL, NULL)
INSERT [dbo].[Users] ([UserId], [TenantId], [UserName], [Email], [AddressId], [Address], [PasswordHash], [Role], [Gender], [Description], [IsEnabled], [CreateDate], [Creator], [UpdateDate], [Updator]) VALUES (3, 3, N'test', N'karin@gmail.com', 2, N'123', N'AQAAAAIAAYagAAAAEDmKB+XiJICO/TUCjkeszC93sxHE21Um4XqMtw/xuB412sPi9gelHQzBfrHLIWy+Ng==', 0, 0, N'<p>aaaa</p>', 0, CAST(N'2024-10-17T17:43:58.043' AS DateTime), 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_Sort]  DEFAULT ((0)) FOR [Sort]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Document_IsEnabled]  DEFAULT ((1)) FOR [IsEnabled]
GO
ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF_Document_Hits]  DEFAULT ((0)) FOR [Hits]
GO
ALTER TABLE [dbo].[FileStorages] ADD  CONSTRAINT [DF__FileStora__Uploa__6FE99F9F]  DEFAULT (getdate()) FOR [UploadDate]
GO
ALTER TABLE [dbo].[FileStorages] ADD  CONSTRAINT [DF__FileStora__IsDel__70DDC3D8]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Modules] ADD  CONSTRAINT [DF_Modules_Sort]  DEFAULT ((0)) FOR [Sort]
GO
