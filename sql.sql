USE [master]
GO
/****** Object:  Database [QuanLySinhVien]    Script Date: 02/06/2024 10:39:49 CH ******/
CREATE DATABASE [QuanLySinhVien]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuanLySinhVien', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QLSV\QuanLySinhVien.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuanLySinhVien_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QLSV\QuanLySinhVien_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QuanLySinhVien] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuanLySinhVien].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuanLySinhVien] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [QuanLySinhVien] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLySinhVien] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuanLySinhVien] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QuanLySinhVien] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuanLySinhVien] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QuanLySinhVien] SET  MULTI_USER 
GO
ALTER DATABASE [QuanLySinhVien] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuanLySinhVien] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuanLySinhVien] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuanLySinhVien] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QuanLySinhVien] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QuanLySinhVien] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [QuanLySinhVien] SET QUERY_STORE = ON
GO
ALTER DATABASE [QuanLySinhVien] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [QuanLySinhVien]
GO
/****** Object:  Table [dbo].[Diem]    Script Date: 02/06/2024 10:39:49 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diem](
	[MaSV] [varchar](10) NOT NULL,
	[MaLopHocPhan] [nvarchar](10) NOT NULL,
	[Diem] [float] NULL,
	[MaHocKi] [int] NULL,
 CONSTRAINT [PK_Diem] PRIMARY KEY CLUSTERED 
(
	[MaSV] ASC,
	[MaLopHocPhan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GiaoVien]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GiaoVien](
	[MaGV] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaGV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HocKi]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HocKi](
	[MaHocKi] [int] IDENTITY(1,1) NOT NULL,
	[TenHocKi] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__HocKi__1EB551008C9F4CEB] PRIMARY KEY CLUSTERED 
(
	[MaHocKi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HocPhi]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HocPhi](
	[MaHocPhi] [int] IDENTITY(1,1) NOT NULL,
	[MaLopHocPhan] [nvarchar](10) NOT NULL,
	[MaSV] [varchar](10) NOT NULL,
	[MaHocKi] [int] NOT NULL,
	[NgayDong] [date] NULL,
 CONSTRAINT [PK__HocPhi__929232A251739ABB] PRIMARY KEY CLUSTERED 
(
	[MaHocPhi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lop]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lop](
	[MaLop] [varchar](10) NOT NULL,
	[TenLop] [nvarchar](100) NOT NULL,
	[MaNganh] [varchar](10) NULL,
 CONSTRAINT [PK_Lop] PRIMARY KEY CLUSTERED 
(
	[MaLop] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LopHocPhan]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LopHocPhan](
	[MaLopHocPhan] [nvarchar](10) NOT NULL,
	[MaMonHoc] [nvarchar](10) NULL,
	[DiaDiem] [nvarchar](100) NULL,
	[MaGV] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLopHocPhan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonHoc]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonHoc](
	[MaHP] [nvarchar](10) NOT NULL,
	[TenHP] [nvarchar](100) NOT NULL,
	[SoTinChi] [int] NOT NULL,
	[DonGia] [int] NOT NULL,
 CONSTRAINT [PK__MonHoc__2725DFD96D890F3F] PRIMARY KEY CLUSTERED 
(
	[MaHP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nganh]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nganh](
	[MaNganh] [varchar](10) NOT NULL,
	[TenNganh] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNganh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SinhVien]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SinhVien](
	[MaSV] [varchar](10) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[NgaySinh] [date] NULL,
	[GioiTinh] [nvarchar](10) NOT NULL,
	[DiaChi] [nvarchar](200) NULL,
	[Email] [nvarchar](100) NULL,
	[SDT] [nvarchar](15) NULL,
	[MaLop] [varchar](10) NOT NULL,
 CONSTRAINT [PK__SinhVien__2725081A5EC470D4] PRIMARY KEY CLUSTERED 
(
	[MaSV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SinhVien_LopHocPhan]    Script Date: 02/06/2024 10:39:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SinhVien_LopHocPhan](
	[MaSV] [varchar](10) NOT NULL,
	[MaLopHocPhan] [nvarchar](10) NOT NULL,
	[MaHocKi] [int] NULL,
 CONSTRAINT [PK__SinhVien__AF0089D7A13C37ED] PRIMARY KEY CLUSTERED 
(
	[MaSV] ASC,
	[MaLopHocPhan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010001', N'Android001', 8, 1)
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010001', N'CTDL-GT001', 8, 1)
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010001', N'HDH001', 8, 1)
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010001', N'QLDAPM001', NULL, 1)
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010001', N'TD001', 10, 1)
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010002', N'KTMT001', NULL, 1)
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010002', N'QLDAPM001', NULL, 1)
INSERT [dbo].[Diem] ([MaSV], [MaLopHocPhan], [Diem], [MaHocKi]) VALUES (N'63010002', N'TKMT001', NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[GiaoVien] ON 

INSERT [dbo].[GiaoVien] ([MaGV], [HoTen], [Email]) VALUES (1, N'Bùi Thị Hồng Minh', N'minh.bth@ntu.edu.vn')
INSERT [dbo].[GiaoVien] ([MaGV], [HoTen], [Email]) VALUES (2, N'Nguyễn Khắc Cường', N'cuong.nk@ntu.edu.vn')
INSERT [dbo].[GiaoVien] ([MaGV], [HoTen], [Email]) VALUES (3, N'Nguyễn Đức Thuần', N'thuan.nd@ntu.edu.vn')
INSERT [dbo].[GiaoVien] ([MaGV], [HoTen], [Email]) VALUES (4, N'Mai Cường Thọ', N'tho.mc@ntu.edu.vn')
INSERT [dbo].[GiaoVien] ([MaGV], [HoTen], [Email]) VALUES (5, N'Bùi Chí Thành', N'thanh.bc@ntu.edu.vn')
INSERT [dbo].[GiaoVien] ([MaGV], [HoTen], [Email]) VALUES (9, N'Huỳnh Huy', N'huy@ntu.edu.vn')
INSERT [dbo].[GiaoVien] ([MaGV], [HoTen], [Email]) VALUES (10, N'Hồ Thanh Nhã', N'nha.ht@ntu.edu.vn')
SET IDENTITY_INSERT [dbo].[GiaoVien] OFF
GO
SET IDENTITY_INSERT [dbo].[HocKi] ON 

INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (1, N'Học Kì 1')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (2, N'Học Kì 2')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (3, N'Học Kì 3')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (4, N'Học Kì 4')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (5, N'Học Kì 5')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (6, N'Học Kì 6')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (7, N'Học Kì 7')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (8, N'Học Kì 8')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (9, N'Học Kì 9')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (10, N'Học Kì 10')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (11, N'Học Kì 11')
INSERT [dbo].[HocKi] ([MaHocKi], [TenHocKi]) VALUES (12, N'Học Kì 12')
SET IDENTITY_INSERT [dbo].[HocKi] OFF
GO
SET IDENTITY_INSERT [dbo].[HocPhi] ON 

INSERT [dbo].[HocPhi] ([MaHocPhi], [MaLopHocPhan], [MaSV], [MaHocKi], [NgayDong]) VALUES (1, N'Android001', N'63010001', 1, NULL)
INSERT [dbo].[HocPhi] ([MaHocPhi], [MaLopHocPhan], [MaSV], [MaHocKi], [NgayDong]) VALUES (6, N'HDH001', N'63010001', 1, NULL)
INSERT [dbo].[HocPhi] ([MaHocPhi], [MaLopHocPhan], [MaSV], [MaHocKi], [NgayDong]) VALUES (7, N'TD001', N'63010001', 1, NULL)
SET IDENTITY_INSERT [dbo].[HocPhi] OFF
GO
INSERT [dbo].[Lop] ([MaLop], [TenLop], [MaNganh]) VALUES (N'1', N'CNTT-1', N'1')
INSERT [dbo].[Lop] ([MaLop], [TenLop], [MaNganh]) VALUES (N'2', N'CNTT-2', N'1')
INSERT [dbo].[Lop] ([MaLop], [TenLop], [MaNganh]) VALUES (N'3', N'KT-1', N'2')
INSERT [dbo].[Lop] ([MaLop], [TenLop], [MaNganh]) VALUES (N'4', N'KT-2', N'2')
INSERT [dbo].[Lop] ([MaLop], [TenLop], [MaNganh]) VALUES (N'5', N'DL-1', N'3')
INSERT [dbo].[Lop] ([MaLop], [TenLop], [MaNganh]) VALUES (N'6', N'DL-2', N'3')
GO
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'Android001', N'Android', N'G6-101', 4)
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'CTDL-GT001', N'CTDL-GT', N'G6-203', 3)
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'HDH001', N'HDH', N'G6-302', 2)
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'KTMT001', N'KTMT', N'G6-203', 4)
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'LTN001', N'LTN', N'G6-202', 9)
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'QLDAPM001', N'QLDAPM', N'G6-101', 5)
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'TD001', N'TD', N'Sân bóng', 10)
INSERT [dbo].[LopHocPhan] ([MaLopHocPhan], [MaMonHoc], [DiaDiem], [MaGV]) VALUES (N'TKMT001', N'TKMT', N'G6-201', 2)
GO
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'Android', N'Lập trình thiết bị di động', 3, 450000)
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'CTDL-GT', N'Cấu trúc dữ liệu và giải thuật', 3, 450000)
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'HDH', N'Hệ điều hành', 3, 450000)
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'KTMT', N'Kiến trúc máy tính', 3, 450000)
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'LTN', N'Lập trình nhúng', 3, 450000)
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'QLDAPM', N'Quản lí dự án phần mềm', 3, 450000)
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'TD', N'Thể dục', 2, 280000)
INSERT [dbo].[MonHoc] ([MaHP], [TenHP], [SoTinChi], [DonGia]) VALUES (N'TKMT', N'Thống kê máy tính', 2, 450000)
GO
INSERT [dbo].[Nganh] ([MaNganh], [TenNganh]) VALUES (N'1', N'Công Nghệ Thông Tin')
INSERT [dbo].[Nganh] ([MaNganh], [TenNganh]) VALUES (N'2', N'Kinh Tế')
INSERT [dbo].[Nganh] ([MaNganh], [TenNganh]) VALUES (N'3', N'Du Lịch')
GO
INSERT [dbo].[SinhVien] ([MaSV], [HoTen], [NgaySinh], [GioiTinh], [DiaChi], [Email], [SDT], [MaLop]) VALUES (N'63010001', N'Phan Châu Hải Lâm', CAST(N'2003-08-15' AS Date), N'Nam', N'Ninh Hòa', N'phanchauhailam15803@gmail.com', N'0945311467', N'2')
INSERT [dbo].[SinhVien] ([MaSV], [HoTen], [NgaySinh], [GioiTinh], [DiaChi], [Email], [SDT], [MaLop]) VALUES (N'63010002', N'Dương Thị Thanh Mỹ', CAST(N'2003-03-15' AS Date), N'Nữ', N'Ninh Hòa', N'tmyx@gmail.com', N'0764967441', N'2')
INSERT [dbo].[SinhVien] ([MaSV], [HoTen], [NgaySinh], [GioiTinh], [DiaChi], [Email], [SDT], [MaLop]) VALUES (N'64020004', N'Bành Thị B', CAST(N'2004-09-02' AS Date), N'Nữ', N'Đăk Lăk', N'B@gmail.com', N'0912837171', N'1')
INSERT [dbo].[SinhVien] ([MaSV], [HoTen], [NgaySinh], [GioiTinh], [DiaChi], [Email], [SDT], [MaLop]) VALUES (N'65020003', N'Nguyễn Văn A', CAST(N'2005-09-02' AS Date), N'Nam', N'Nha Trang', N'A@gmail.com', N'0191892872', N'3')
GO
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010001', N'Android001', 1)
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010001', N'CTDL-GT001', 1)
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010001', N'HDH001', 1)
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010001', N'QLDAPM001', 1)
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010001', N'TD001', 1)
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010002', N'KTMT001', 1)
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010002', N'QLDAPM001', 1)
INSERT [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan], [MaHocKi]) VALUES (N'63010002', N'TKMT001', 1)
GO
ALTER TABLE [dbo].[SinhVien] ADD  CONSTRAINT [DF__SinhVien__GioiTi__7D439ABD]  DEFAULT ((0)) FOR [GioiTinh]
GO
ALTER TABLE [dbo].[Diem]  WITH CHECK ADD  CONSTRAINT [FK_Diem_HocKi1] FOREIGN KEY([MaHocKi])
REFERENCES [dbo].[HocKi] ([MaHocKi])
GO
ALTER TABLE [dbo].[Diem] CHECK CONSTRAINT [FK_Diem_HocKi1]
GO
ALTER TABLE [dbo].[Diem]  WITH CHECK ADD  CONSTRAINT [FK_Diem_LopHocPhan] FOREIGN KEY([MaLopHocPhan])
REFERENCES [dbo].[LopHocPhan] ([MaLopHocPhan])
GO
ALTER TABLE [dbo].[Diem] CHECK CONSTRAINT [FK_Diem_LopHocPhan]
GO
ALTER TABLE [dbo].[Diem]  WITH CHECK ADD  CONSTRAINT [FK_Diem_SinhVien] FOREIGN KEY([MaSV])
REFERENCES [dbo].[SinhVien] ([MaSV])
GO
ALTER TABLE [dbo].[Diem] CHECK CONSTRAINT [FK_Diem_SinhVien]
GO
ALTER TABLE [dbo].[HocPhi]  WITH CHECK ADD  CONSTRAINT [FK_HocPhi_HocKi] FOREIGN KEY([MaHocKi])
REFERENCES [dbo].[HocKi] ([MaHocKi])
GO
ALTER TABLE [dbo].[HocPhi] CHECK CONSTRAINT [FK_HocPhi_HocKi]
GO
ALTER TABLE [dbo].[HocPhi]  WITH CHECK ADD  CONSTRAINT [FK_HocPhi_SinhVien_LopHocPhan] FOREIGN KEY([MaSV], [MaLopHocPhan])
REFERENCES [dbo].[SinhVien_LopHocPhan] ([MaSV], [MaLopHocPhan])
GO
ALTER TABLE [dbo].[HocPhi] CHECK CONSTRAINT [FK_HocPhi_SinhVien_LopHocPhan]
GO
ALTER TABLE [dbo].[Lop]  WITH CHECK ADD  CONSTRAINT [FK_Lop_Nganh] FOREIGN KEY([MaNganh])
REFERENCES [dbo].[Nganh] ([MaNganh])
GO
ALTER TABLE [dbo].[Lop] CHECK CONSTRAINT [FK_Lop_Nganh]
GO
ALTER TABLE [dbo].[LopHocPhan]  WITH CHECK ADD  CONSTRAINT [FK__LopHocPhan__MaMH__5BE2A6F2] FOREIGN KEY([MaMonHoc])
REFERENCES [dbo].[MonHoc] ([MaHP])
GO
ALTER TABLE [dbo].[LopHocPhan] CHECK CONSTRAINT [FK__LopHocPhan__MaMH__5BE2A6F2]
GO
ALTER TABLE [dbo].[LopHocPhan]  WITH CHECK ADD  CONSTRAINT [FK_LopHocPhan_GiaoVien] FOREIGN KEY([MaGV])
REFERENCES [dbo].[GiaoVien] ([MaGV])
GO
ALTER TABLE [dbo].[LopHocPhan] CHECK CONSTRAINT [FK_LopHocPhan_GiaoVien]
GO
ALTER TABLE [dbo].[SinhVien]  WITH CHECK ADD  CONSTRAINT [FK_SinhVien_Lop1] FOREIGN KEY([MaLop])
REFERENCES [dbo].[Lop] ([MaLop])
GO
ALTER TABLE [dbo].[SinhVien] CHECK CONSTRAINT [FK_SinhVien_Lop1]
GO
ALTER TABLE [dbo].[SinhVien_LopHocPhan]  WITH CHECK ADD  CONSTRAINT [FK_SinhVien_LopHocPhan_HocKi] FOREIGN KEY([MaHocKi])
REFERENCES [dbo].[HocKi] ([MaHocKi])
GO
ALTER TABLE [dbo].[SinhVien_LopHocPhan] CHECK CONSTRAINT [FK_SinhVien_LopHocPhan_HocKi]
GO
ALTER TABLE [dbo].[SinhVien_LopHocPhan]  WITH CHECK ADD  CONSTRAINT [FK_SinhVien_LopHocPhan_LopHocPhan] FOREIGN KEY([MaLopHocPhan])
REFERENCES [dbo].[LopHocPhan] ([MaLopHocPhan])
GO
ALTER TABLE [dbo].[SinhVien_LopHocPhan] CHECK CONSTRAINT [FK_SinhVien_LopHocPhan_LopHocPhan]
GO
ALTER TABLE [dbo].[SinhVien_LopHocPhan]  WITH CHECK ADD  CONSTRAINT [FK_SinhVien_LopHocPhan_SinhVien] FOREIGN KEY([MaSV])
REFERENCES [dbo].[SinhVien] ([MaSV])
GO
ALTER TABLE [dbo].[SinhVien_LopHocPhan] CHECK CONSTRAINT [FK_SinhVien_LopHocPhan_SinhVien]
GO
USE [master]
GO
ALTER DATABASE [QuanLySinhVien] SET  READ_WRITE 
GO
