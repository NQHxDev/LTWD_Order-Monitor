/*
 Navicat Premium Data Transfer

 Source Server         : SQL Server
 Source Server Type    : SQL Server
 Source Server Version : 16001000
 Source Host           : localhost:1433
 Source Catalog        : ltwd_order_monitor
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 16001000
 File Encoding         : 65001

 Date: 03/11/2025 21:01:39
*/


-- ----------------------------
-- Table structure for account
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[account]') AND type IN ('U'))
	DROP TABLE [dbo].[account]
GO

CREATE TABLE [dbo].[account] (
  [ac_id] int  IDENTITY(1,1) NOT NULL,
  [username] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [password] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [role] int DEFAULT 0 NOT NULL,
  [created_at] datetime DEFAULT getdate() NULL,
  [name] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [acc_status] int DEFAULT 0 NOT NULL,
  [reason] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[account] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of account
-- ----------------------------
SET IDENTITY_INSERT [dbo].[account] ON
GO

INSERT INTO [dbo].[account] ([ac_id], [username], [password], [role], [created_at], [name], [acc_status], [reason]) VALUES (N'1', N'admin', N'582005', N'1', N'2025-10-07 14:08:51.580', N'NQH', N'0', NULL)
GO

INSERT INTO [dbo].[account] ([ac_id], [username], [password], [role], [created_at], [name], [acc_status], [reason]) VALUES (N'2', N'staff01', N'1', N'0', N'2025-10-07 14:08:51.580', N'Chief', N'0', NULL)
GO

INSERT INTO [dbo].[account] ([ac_id], [username], [password], [role], [created_at], [name], [acc_status], [reason]) VALUES (N'3', N'1', N'1', N'0', N'2025-10-14 21:30:22.710', N'Tester', N'0', N'Ngu')
GO

INSERT INTO [dbo].[account] ([ac_id], [username], [password], [role], [created_at], [name], [acc_status], [reason]) VALUES (N'4', N'2', N'2', N'1', N'2025-10-21 11:50:26.783', N'Leader', N'0', NULL)
GO

INSERT INTO [dbo].[account] ([ac_id], [username], [password], [role], [created_at], [name], [acc_status], [reason]) VALUES (N'5', N'3', N'4', N'0', N'2025-11-03 20:09:32.177', N'NQH', N'-1', N'Nghi kh ly do')
GO

SET IDENTITY_INSERT [dbo].[account] OFF
GO


-- ----------------------------
-- Table structure for depot
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[depot]') AND type IN ('U'))
	DROP TABLE [dbo].[depot]
GO

CREATE TABLE [dbo].[depot] (
  [stt] int  IDENTITY(1,1) NOT NULL,
  [item_id] int  NOT NULL,
  [quantity] decimal(10,3) DEFAULT 0.000 NOT NULL,
  [last_updated] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[depot] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of depot
-- ----------------------------
SET IDENTITY_INSERT [dbo].[depot] ON
GO

INSERT INTO [dbo].[depot] ([stt], [item_id], [quantity], [last_updated]) VALUES (N'1', N'1', N'5.000', N'2025-10-07 14:09:32.887')
GO

INSERT INTO [dbo].[depot] ([stt], [item_id], [quantity], [last_updated]) VALUES (N'2', N'2', N'10.000', N'2025-10-07 14:09:32.887')
GO

INSERT INTO [dbo].[depot] ([stt], [item_id], [quantity], [last_updated]) VALUES (N'3', N'3', N'5.000', N'2025-10-07 14:09:32.887')
GO

INSERT INTO [dbo].[depot] ([stt], [item_id], [quantity], [last_updated]) VALUES (N'4', N'4', N'3.000', N'2025-10-07 14:09:32.887')
GO

INSERT INTO [dbo].[depot] ([stt], [item_id], [quantity], [last_updated]) VALUES (N'5', N'5', N'1.000', N'2025-10-07 14:09:32.887')
GO

INSERT INTO [dbo].[depot] ([stt], [item_id], [quantity], [last_updated]) VALUES (N'6', N'6', N'2.000', N'2025-10-07 14:09:32.887')
GO

SET IDENTITY_INSERT [dbo].[depot] OFF
GO


-- ----------------------------
-- Table structure for export
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[export]') AND type IN ('U'))
	DROP TABLE [dbo].[export]
GO

CREATE TABLE [dbo].[export] (
  [export_id] int  IDENTITY(1,1) NOT NULL,
  [export_date] datetime DEFAULT getdate() NULL,
  [purpose] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [export_by] int  NULL,
  [export_status] int DEFAULT 0 NOT NULL
)
GO

ALTER TABLE [dbo].[export] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of export
-- ----------------------------
SET IDENTITY_INSERT [dbo].[export] ON
GO

SET IDENTITY_INSERT [dbo].[export] OFF
GO


-- ----------------------------
-- Table structure for export_detail
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[export_detail]') AND type IN ('U'))
	DROP TABLE [dbo].[export_detail]
GO

CREATE TABLE [dbo].[export_detail] (
  [ex_detl_id] int  IDENTITY(1,1) NOT NULL,
  [export_id] int  NOT NULL,
  [item_id] int  NOT NULL,
  [quantity] decimal(10,3)  NOT NULL,
  [note] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[export_detail] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of export_detail
-- ----------------------------
SET IDENTITY_INSERT [dbo].[export_detail] ON
GO

SET IDENTITY_INSERT [dbo].[export_detail] OFF
GO


-- ----------------------------
-- Table structure for food
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[food]') AND type IN ('U'))
	DROP TABLE [dbo].[food]
GO

CREATE TABLE [dbo].[food] (
  [food_id] int  IDENTITY(1,1) NOT NULL,
  [name] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [description] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [price] int DEFAULT 0 NOT NULL,
  [status] smallint DEFAULT 0 NOT NULL,
  [created_at] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[food] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of food
-- ----------------------------
SET IDENTITY_INSERT [dbo].[food] ON
GO

INSERT INTO [dbo].[food] ([food_id], [name], [description], [price], [status], [created_at]) VALUES (N'1', N'Trà sữa trân châu', N'Trà đen pha với sữa và topping trân châu đen', N'35', N'0', N'2025-10-07 14:08:10.783')
GO

INSERT INTO [dbo].[food] ([food_id], [name], [description], [price], [status], [created_at]) VALUES (N'2', N'Trà sữa bạc hà', N'Hương vị the mát của bạc hà kết hợp cùng sữa tươi', N'38', N'0', N'2025-10-07 14:08:10.783')
GO

INSERT INTO [dbo].[food] ([food_id], [name], [description], [price], [status], [created_at]) VALUES (N'3', N'Trà đen sữa kem béo', N'Trà đen nguyên chất, lớp kem béo mềm mịn', N'40', N'0', N'2025-10-07 14:08:10.783')
GO

INSERT INTO [dbo].[food] ([food_id], [name], [description], [price], [status], [created_at]) VALUES (N'4', N'Trà sữa đặc biệt', N'Trà sữa mix nhiều loại topping và kem béo', N'48', N'0', N'2025-10-07 14:08:10.783')
GO

INSERT INTO [dbo].[food] ([food_id], [name], [description], [price], [status], [created_at]) VALUES (N'5', N'Trà Đào', N'Thơm ngon chuẩn vị', N'35', N'1', N'2025-10-09 16:00:02.690')
GO

INSERT INTO [dbo].[food] ([food_id], [name], [description], [price], [status], [created_at]) VALUES (N'6', N'Trà sữa truyền thống', N'Thơm ngon', N'45', N'0', N'2025-10-09 16:09:27.097')
GO

INSERT INTO [dbo].[food] ([food_id], [name], [description], [price], [status], [created_at]) VALUES (N'7', N'Trà Vải', N'Ngon', N'35', N'0', N'2025-10-09 16:14:45.963')
GO

SET IDENTITY_INSERT [dbo].[food] OFF
GO


-- ----------------------------
-- Table structure for food_ingredient
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[food_ingredient]') AND type IN ('U'))
	DROP TABLE [dbo].[food_ingredient]
GO

CREATE TABLE [dbo].[food_ingredient] (
  [fo_ingr_id] int  IDENTITY(1,1) NOT NULL,
  [food_id] int  NOT NULL,
  [item_id] int  NOT NULL,
  [quantity] decimal(10,3)  NOT NULL
)
GO

ALTER TABLE [dbo].[food_ingredient] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of food_ingredient
-- ----------------------------
SET IDENTITY_INSERT [dbo].[food_ingredient] ON
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'1', N'1', N'1', N'.020')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'2', N'1', N'2', N'.050')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'3', N'1', N'4', N'.030')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'4', N'2', N'1', N'.020')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'5', N'2', N'2', N'.050')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'6', N'2', N'5', N'.010')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'7', N'3', N'1', N'.020')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'8', N'3', N'2', N'.040')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'9', N'3', N'6', N'.020')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'17', N'6', N'5', N'1.000')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'18', N'6', N'2', N'1.000')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'19', N'6', N'1', N'50.000')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'32', N'5', N'1', N'5.000')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'33', N'5', N'3', N'1.000')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'34', N'4', N'1', N'.020')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'35', N'4', N'2', N'.060')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'36', N'4', N'4', N'.030')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'37', N'4', N'6', N'.020')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'38', N'7', N'1', N'200.000')
GO

INSERT INTO [dbo].[food_ingredient] ([fo_ingr_id], [food_id], [item_id], [quantity]) VALUES (N'39', N'7', N'3', N'2.000')
GO

SET IDENTITY_INSERT [dbo].[food_ingredient] OFF
GO


-- ----------------------------
-- Table structure for import
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[import]') AND type IN ('U'))
	DROP TABLE [dbo].[import]
GO

CREATE TABLE [dbo].[import] (
  [import_id] int  IDENTITY(1,1) NOT NULL,
  [create_at] datetime DEFAULT getdate() NULL,
  [total_price] decimal(12,2) DEFAULT 0.00 NOT NULL,
  [created_by] int  NULL,
  [import_status] int DEFAULT 0 NOT NULL,
  [update_by] int DEFAULT NULL NULL,
  [total_item] int DEFAULT 0 NOT NULL,
  [reason] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [update_at] datetime  NULL
)
GO

ALTER TABLE [dbo].[import] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of import
-- ----------------------------
SET IDENTITY_INSERT [dbo].[import] ON
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'7', N'2025-10-28 18:00:46.670', N'2250000.00', N'3', N'2', N'3', N'3', NULL, NULL)
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'8', N'2025-10-31 14:41:27.430', N'7740000.00', N'3', N'-1', N'4', N'2', N'Khong du tieu chuan', N'2025-11-03 20:23:16.020')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'9', N'2025-10-31 17:04:32.793', N'2820000.00', N'3', N'1', N'4', N'4', NULL, N'2025-11-02 10:38:46.027')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'10', N'2025-11-03 20:06:55.357', N'1644000.00', N'3', N'-1', N'4', N'2', N'Nhap nhieu qua', N'2025-11-03 20:07:31.290')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'11', N'2025-11-03 20:27:26.990', N'3180000.00', N'3', N'1', N'4', N'3', NULL, N'2025-11-03 20:27:53.413')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'12', N'2025-11-03 20:28:18.167', N'3600000.00', N'3', N'-1', N'4', N'3', N'aaa', N'2025-11-03 20:49:00.470')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'13', N'2025-11-03 20:32:54.660', N'2100000.00', N'3', N'1', N'4', N'2', NULL, N'2025-11-03 20:33:09.860')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'14', N'2025-11-03 20:42:38.577', N'1800000.00', N'3', N'-1', N'4', N'1', N'aa', N'2025-11-03 20:42:49.987')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'15', N'2025-11-03 20:49:18.553', N'1800000.00', N'3', N'-1', N'4', N'1', N'aaaaaa', N'2025-11-03 20:49:33.443')
GO

INSERT INTO [dbo].[import] ([import_id], [create_at], [total_price], [created_by], [import_status], [update_by], [total_item], [reason], [update_at]) VALUES (N'16', N'2025-11-03 20:49:21.700', N'420000.00', N'3', N'1', N'4', N'1', NULL, N'2025-11-03 20:49:26.420')
GO

SET IDENTITY_INSERT [dbo].[import] OFF
GO


-- ----------------------------
-- Table structure for import_detail
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[import_detail]') AND type IN ('U'))
	DROP TABLE [dbo].[import_detail]
GO

CREATE TABLE [dbo].[import_detail] (
  [im_detl_id] int  IDENTITY(1,1) NOT NULL,
  [import_id] int  NOT NULL,
  [item_id] int  NOT NULL,
  [quantity] decimal(10,3)  NOT NULL,
  [unit_price] decimal(10,2)  NOT NULL,
  [import_date] datetime  NULL,
  [import_by] int  NULL
)
GO

ALTER TABLE [dbo].[import_detail] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of import_detail
-- ----------------------------
SET IDENTITY_INSERT [dbo].[import_detail] ON
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'12', N'7', N'1', N'10.000', N'150000.00', N'2025-10-28 20:27:15.367', N'3')
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'13', N'7', N'4', N'10.000', N'35000.00', N'2025-10-28 20:27:15.470', N'3')
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'14', N'7', N'6', N'10.000', N'40000.00', N'2025-10-28 20:27:15.493', N'3')
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'15', N'8', N'1', N'50.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'16', N'8', N'3', N'20.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'17', N'9', N'1', N'10.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'18', N'9', N'5', N'10.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'19', N'9', N'6', N'10.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'20', N'9', N'3', N'10.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'21', N'10', N'1', N'10.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'22', N'10', N'3', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'23', N'11', N'1', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'24', N'11', N'4', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'25', N'11', N'5', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'26', N'12', N'1', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'27', N'12', N'4', N'24.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'28', N'12', N'5', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'29', N'13', N'1', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'30', N'13', N'2', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'31', N'14', N'1', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'32', N'15', N'1', N'12.000', N'.00', NULL, NULL)
GO

INSERT INTO [dbo].[import_detail] ([im_detl_id], [import_id], [item_id], [quantity], [unit_price], [import_date], [import_by]) VALUES (N'33', N'16', N'4', N'12.000', N'.00', NULL, NULL)
GO

SET IDENTITY_INSERT [dbo].[import_detail] OFF
GO


-- ----------------------------
-- Table structure for item
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[item]') AND type IN ('U'))
	DROP TABLE [dbo].[item]
GO

CREATE TABLE [dbo].[item] (
  [item_id] int  IDENTITY(1,1) NOT NULL,
  [name] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [unit_id] int  NOT NULL,
  [import_price] decimal(10,2) DEFAULT 0.00 NOT NULL,
  [is_active] bit DEFAULT 1 NOT NULL,
  [quantity] int DEFAULT 0 NOT NULL
)
GO

ALTER TABLE [dbo].[item] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of item
-- ----------------------------
SET IDENTITY_INSERT [dbo].[item] ON
GO

INSERT INTO [dbo].[item] ([item_id], [name], [unit_id], [import_price], [is_active], [quantity]) VALUES (N'1', N'Trà đen', N'1', N'150000.00', N'1', N'10')
GO

INSERT INTO [dbo].[item] ([item_id], [name], [unit_id], [import_price], [is_active], [quantity]) VALUES (N'2', N'Sữa tươi', N'3', N'25000.00', N'1', N'0')
GO

INSERT INTO [dbo].[item] ([item_id], [name], [unit_id], [import_price], [is_active], [quantity]) VALUES (N'3', N'Đường trắng', N'2', N'12000.00', N'1', N'0')
GO

INSERT INTO [dbo].[item] ([item_id], [name], [unit_id], [import_price], [is_active], [quantity]) VALUES (N'4', N'Trân châu đen', N'2', N'35000.00', N'1', N'10')
GO

INSERT INTO [dbo].[item] ([item_id], [name], [unit_id], [import_price], [is_active], [quantity]) VALUES (N'5', N'Lá bạc hà', N'1', N'80000.00', N'1', N'0')
GO

INSERT INTO [dbo].[item] ([item_id], [name], [unit_id], [import_price], [is_active], [quantity]) VALUES (N'6', N'Kem béo', N'3', N'40000.00', N'1', N'10')
GO

SET IDENTITY_INSERT [dbo].[item] OFF
GO


-- ----------------------------
-- Table structure for list_order
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[list_order]') AND type IN ('U'))
	DROP TABLE [dbo].[list_order]
GO

CREATE TABLE [dbo].[list_order] (
  [oder_id] int  IDENTITY(1,1) NOT NULL,
  [customer_name] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [note] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [total_price] decimal(12,2) DEFAULT 0.00 NOT NULL,
  [status] smallint DEFAULT 0 NOT NULL,
  [created_at] datetime DEFAULT getdate() NULL,
  [updated_at] datetime DEFAULT getdate() NULL,
  [customer_phone] nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [staff_feedback] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [count_food] int DEFAULT 0 NOT NULL
)
GO

ALTER TABLE [dbo].[list_order] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of list_order
-- ----------------------------
SET IDENTITY_INSERT [dbo].[list_order] ON
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'118', N'Nguyễn Quang Hưng', N'', N'73000.00', N'2', N'2025-10-21 10:10:42.790', N'2025-10-21 10:50:19.220', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'119', N'Nguyễn Quang Hưng', N'', N'113000.00', N'2', N'2025-10-21 10:49:50.843', N'2025-10-22 22:01:19.070', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'120', N'PTMN', N'', N'123000.00', N'2', N'2025-10-21 11:42:55.040', N'2025-10-22 22:01:18.390', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'121', N'Nguyễn Quang Hưng', N'', N'121000.00', N'2', N'2025-10-22 21:20:35.573', N'2025-10-22 22:01:16.193', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'122', N'PTMN', N'', N'80000.00', N'2', N'2025-10-22 21:34:07.233', N'2025-10-22 21:34:18.493', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'123', N'NQH', N'', N'116000.00', N'2', N'2025-10-22 21:37:52.720', N'2025-10-22 22:01:17.727', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'124', N'Nguyễn Quang Hưng', N'', N'170000.00', N'2', N'2025-10-22 21:58:47.690', N'2025-10-22 22:01:16.990', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'125', N'Nguyễn Quang Hưng', N'', N'70000.00', N'2', N'2025-10-22 22:01:04.353', N'2025-10-22 22:10:42.510', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'126', N'PTMN', N'', N'105000.00', N'2', N'2025-10-22 22:07:44.180', N'2025-10-22 22:10:41.357', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'127', N'Nguyễn Quang Hưng', N'', N'48000.00', N'2', N'2025-10-22 22:11:09.977', N'2025-10-22 22:12:47.410', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'128', N'NQH', N'', N'70000.00', N'2', N'2025-10-22 22:13:13.700', N'2025-10-22 22:15:26.133', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'129', N'Nguyễn Quang Hưng', N'', N'96000.00', N'2', N'2025-10-22 22:15:48.847', N'2025-10-22 22:18:14.987', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'130', N'Nguyễn Quang Hưng', N'', N'78000.00', N'2', N'2025-10-22 22:19:10.400', N'2025-10-22 22:21:37.847', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'131', N'Nguyễn Quang Hưng', N'', N'105000.00', N'2', N'2025-10-22 22:21:55.760', N'2025-10-22 22:25:37.223', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'132', N'Nguyễn Quang Hưng', N'', N'38000.00', N'2', N'2025-10-22 22:25:53.067', N'2025-10-22 22:30:33.810', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'133', N'Nguyễn Quang Hưng', N'', N'96000.00', N'2', N'2025-10-22 22:30:50.540', N'2025-10-22 22:34:48.027', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'134', N'Nguyễn Quang Hưng', N'', N'73000.00', N'2', N'2025-10-22 22:35:05.220', N'2025-10-22 22:38:22.730', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'135', N'Nguyễn Quang Hưng', N'', N'96000.00', N'2', N'2025-10-22 22:38:46.830', N'2025-10-22 22:40:01.433', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'136', N'Nguyễn Quang Hưng', N'', N'96000.00', N'2', N'2025-10-22 22:40:09.180', N'2025-10-22 22:43:01.473', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'137', N'NQH', N'', N'190000.00', N'2', N'2025-10-22 22:43:19.430', N'2025-10-22 22:52:19.393', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'138', N'Nguyễn Quang Hưng', N'', N'240000.00', N'2', N'2025-10-22 22:52:33.283', N'2025-10-22 23:03:08.773', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'139', N'Nguyễn Quang Hưng', N'', N'240000.00', N'2', N'2025-10-22 23:03:44.857', N'2025-10-22 23:14:28.130', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'140', N'Nguyễn Quang Hưng', N'', N'240000.00', N'2', N'2025-10-22 23:14:17.007', N'2025-10-22 23:14:27.567', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'141', N'Nguyễn Quang Hưng', N'', N'240000.00', N'2', N'2025-10-22 23:14:51.400', N'2025-10-22 23:14:59.187', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'142', N'Nguyễn Quang Hưng', N'', N'38000.00', N'2', N'2025-10-23 12:52:35.947', N'2025-10-23 12:52:41.417', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'143', N'NQH', N'', N'114000.00', N'2', N'2025-10-23 13:00:43.817', N'2025-10-23 13:01:00.863', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'144', N'Nguyễn Quang Hưng', N'', N'380000.00', N'2', N'2025-10-25 13:06:38.930', N'2025-10-25 13:07:13.457', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'145', N'Nguyễn Quang Hưng', N'', N'73000.00', N'2', N'2025-10-27 12:51:35.310', N'2025-10-27 12:52:48.880', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'146', N'A', N'', N'73000.00', N'2', N'2025-10-27 13:59:11.923', N'2025-10-27 13:59:42.970', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'147', N'Trung Ne', N'', N'163000.00', N'-1', N'2025-10-27 15:20:16.240', N'2025-10-27 15:20:57.290', N'0966376155', N'Hết nguyên liệu', N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'148', N'Nguyễn Quang Hưng', N'', N'113000.00', N'2', N'2025-10-27 15:25:00.643', N'2025-10-27 15:25:45.843', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'149', N'Nguyễn Quang Hưng', N'', N'35000.00', N'2', N'2025-10-31 15:48:36.823', N'2025-11-03 20:11:25.097', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'150', N'Nguyễn Quang Hưng', N'', N'123000.00', N'2', N'2025-10-31 20:50:55.747', N'2025-11-03 20:11:24.713', N'0966376155', NULL, N'0')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'151', N'NQH', N'', N'128000.00', N'2', N'2025-10-31 20:57:19.917', N'2025-11-03 20:11:25.453', N'0966376155', NULL, N'3')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'152', N'Nguyễn Quang Hưng', N'', N'113000.00', N'2', N'2025-11-03 20:11:15.953', N'2025-11-03 20:11:24.117', N'0966376155', NULL, N'3')
GO

INSERT INTO [dbo].[list_order] ([oder_id], [customer_name], [note], [total_price], [status], [created_at], [updated_at], [customer_phone], [staff_feedback], [count_food]) VALUES (N'153', N'Nguyễn Quang Hưng', N'', N'73000.00', N'1', N'2025-11-03 20:12:44.240', N'2025-11-03 20:12:47.483', N'0966376155', NULL, N'2')
GO

SET IDENTITY_INSERT [dbo].[list_order] OFF
GO


-- ----------------------------
-- Table structure for order_detail
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[order_detail]') AND type IN ('U'))
	DROP TABLE [dbo].[order_detail]
GO

CREATE TABLE [dbo].[order_detail] (
  [ord_delt_id] int  IDENTITY(1,1) NOT NULL,
  [order_id] int  NOT NULL,
  [food_id] int  NOT NULL,
  [quantity] int DEFAULT 1 NOT NULL,
  [price] decimal(10,2)  NOT NULL
)
GO

ALTER TABLE [dbo].[order_detail] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of order_detail
-- ----------------------------
SET IDENTITY_INSERT [dbo].[order_detail] ON
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'261', N'118', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'262', N'118', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'263', N'119', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'264', N'119', N'3', N'1', N'40.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'265', N'119', N'5', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'266', N'120', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'267', N'120', N'3', N'1', N'40.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'268', N'120', N'6', N'1', N'45.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'269', N'121', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'270', N'121', N'5', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'271', N'121', N'4', N'1', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'272', N'122', N'6', N'1', N'45.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'273', N'122', N'7', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'274', N'123', N'2', N'2', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'275', N'123', N'3', N'1', N'40.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'276', N'124', N'6', N'3', N'45.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'277', N'124', N'5', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'278', N'125', N'5', N'2', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'279', N'126', N'1', N'3', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'280', N'127', N'4', N'1', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'281', N'128', N'7', N'2', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'282', N'129', N'4', N'2', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'283', N'130', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'284', N'130', N'3', N'1', N'40.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'285', N'131', N'7', N'3', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'286', N'132', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'287', N'133', N'4', N'2', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'288', N'134', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'289', N'134', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'290', N'135', N'4', N'2', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'291', N'136', N'4', N'2', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'292', N'137', N'2', N'5', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'293', N'138', N'4', N'5', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'294', N'139', N'4', N'5', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'295', N'140', N'4', N'5', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'296', N'141', N'4', N'5', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'297', N'142', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'298', N'143', N'2', N'3', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'299', N'144', N'2', N'10', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'300', N'145', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'301', N'145', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'302', N'146', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'303', N'146', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'304', N'147', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'305', N'147', N'5', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'306', N'147', N'6', N'1', N'45.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'307', N'147', N'4', N'1', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'308', N'148', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'309', N'148', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'310', N'148', N'3', N'1', N'40.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'311', N'149', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'312', N'150', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'313', N'150', N'3', N'1', N'40.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'314', N'150', N'6', N'1', N'45.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'315', N'151', N'5', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'316', N'151', N'6', N'1', N'45.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'317', N'151', N'4', N'1', N'48.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'318', N'152', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'319', N'152', N'2', N'1', N'38.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'320', N'152', N'3', N'1', N'40.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'321', N'153', N'1', N'1', N'35.00')
GO

INSERT INTO [dbo].[order_detail] ([ord_delt_id], [order_id], [food_id], [quantity], [price]) VALUES (N'322', N'153', N'2', N'1', N'38.00')
GO

SET IDENTITY_INSERT [dbo].[order_detail] OFF
GO


-- ----------------------------
-- Table structure for unit
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[unit]') AND type IN ('U'))
	DROP TABLE [dbo].[unit]
GO

CREATE TABLE [dbo].[unit] (
  [unit_id] int  IDENTITY(1,1) NOT NULL,
  [name] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [abbreviation] nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[unit] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of unit
-- ----------------------------
SET IDENTITY_INSERT [dbo].[unit] ON
GO

INSERT INTO [dbo].[unit] ([unit_id], [name], [abbreviation]) VALUES (N'1', N'Kilogram', N'kg')
GO

INSERT INTO [dbo].[unit] ([unit_id], [name], [abbreviation]) VALUES (N'2', N'Gram', N'g')
GO

INSERT INTO [dbo].[unit] ([unit_id], [name], [abbreviation]) VALUES (N'3', N'Mililít', N'ml')
GO

INSERT INTO [dbo].[unit] ([unit_id], [name], [abbreviation]) VALUES (N'4', N'Ly', N'ly')
GO

INSERT INTO [dbo].[unit] ([unit_id], [name], [abbreviation]) VALUES (N'5', N'Cái', N'cái')
GO

SET IDENTITY_INSERT [dbo].[unit] OFF
GO


-- ----------------------------
-- Auto increment value for account
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[account]', RESEED, 5)
GO


-- ----------------------------
-- Uniques structure for table account
-- ----------------------------
ALTER TABLE [dbo].[account] ADD CONSTRAINT [UQ__account__F3DBC572BD95D09B] UNIQUE NONCLUSTERED ([username] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table account
-- ----------------------------
ALTER TABLE [dbo].[account] ADD CONSTRAINT [PK__account__A36ABBF4227C8DA9] PRIMARY KEY CLUSTERED ([ac_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for depot
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[depot]', RESEED, 6)
GO


-- ----------------------------
-- Uniques structure for table depot
-- ----------------------------
ALTER TABLE [dbo].[depot] ADD CONSTRAINT [UQ__depot__52020FDCA78146D7] UNIQUE NONCLUSTERED ([item_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table depot
-- ----------------------------
ALTER TABLE [dbo].[depot] ADD CONSTRAINT [PK__depot__DDDF328E7EBB0A8A] PRIMARY KEY CLUSTERED ([stt])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for export
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[export]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table export
-- ----------------------------
ALTER TABLE [dbo].[export] ADD CONSTRAINT [PK__export__323057CFE521A03D] PRIMARY KEY CLUSTERED ([export_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for export_detail
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[export_detail]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table export_detail
-- ----------------------------
ALTER TABLE [dbo].[export_detail] ADD CONSTRAINT [PK__export_d__308B5FA622AA5E94] PRIMARY KEY CLUSTERED ([ex_detl_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for food
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[food]', RESEED, 7)
GO


-- ----------------------------
-- Uniques structure for table food
-- ----------------------------
ALTER TABLE [dbo].[food] ADD CONSTRAINT [UQ__food__72E12F1B90151FAF] UNIQUE NONCLUSTERED ([name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table food
-- ----------------------------
ALTER TABLE [dbo].[food] ADD CONSTRAINT [PK__food__2F4C4DD87B28B664] PRIMARY KEY CLUSTERED ([food_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for food_ingredient
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[food_ingredient]', RESEED, 39)
GO


-- ----------------------------
-- Primary Key structure for table food_ingredient
-- ----------------------------
ALTER TABLE [dbo].[food_ingredient] ADD CONSTRAINT [PK__food_ing__53B0726F4BA28C7F] PRIMARY KEY CLUSTERED ([fo_ingr_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for import
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[import]', RESEED, 16)
GO


-- ----------------------------
-- Primary Key structure for table import
-- ----------------------------
ALTER TABLE [dbo].[import] ADD CONSTRAINT [PK__import__F3E6B05FCD41BC33] PRIMARY KEY CLUSTERED ([import_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for import_detail
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[import_detail]', RESEED, 33)
GO


-- ----------------------------
-- Primary Key structure for table import_detail
-- ----------------------------
ALTER TABLE [dbo].[import_detail] ADD CONSTRAINT [PK__import_d__7907B8CCD57E6733] PRIMARY KEY CLUSTERED ([im_detl_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for item
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[item]', RESEED, 6)
GO


-- ----------------------------
-- Uniques structure for table item
-- ----------------------------
ALTER TABLE [dbo].[item] ADD CONSTRAINT [UQ__item__72E12F1B23F5FBE3] UNIQUE NONCLUSTERED ([name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table item
-- ----------------------------
ALTER TABLE [dbo].[item] ADD CONSTRAINT [PK__item__52020FDDC2B7983B] PRIMARY KEY CLUSTERED ([item_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for list_order
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[list_order]', RESEED, 153)
GO


-- ----------------------------
-- Primary Key structure for table list_order
-- ----------------------------
ALTER TABLE [dbo].[list_order] ADD CONSTRAINT [PK__list_ord__DD2BC0DF8807C945] PRIMARY KEY CLUSTERED ([oder_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for order_detail
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[order_detail]', RESEED, 322)
GO


-- ----------------------------
-- Primary Key structure for table order_detail
-- ----------------------------
ALTER TABLE [dbo].[order_detail] ADD CONSTRAINT [PK__order_de__27ED651D801D279F] PRIMARY KEY CLUSTERED ([ord_delt_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for unit
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[unit]', RESEED, 5)
GO


-- ----------------------------
-- Uniques structure for table unit
-- ----------------------------
ALTER TABLE [dbo].[unit] ADD CONSTRAINT [UQ__unit__72E12F1BA4A6796C] UNIQUE NONCLUSTERED ([name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table unit
-- ----------------------------
ALTER TABLE [dbo].[unit] ADD CONSTRAINT [PK__unit__D3AF5BD7231388F6] PRIMARY KEY CLUSTERED ([unit_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table depot
-- ----------------------------
ALTER TABLE [dbo].[depot] ADD CONSTRAINT [FK_depot_item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[item] ([item_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table export
-- ----------------------------
ALTER TABLE [dbo].[export] ADD CONSTRAINT [FK_export_account] FOREIGN KEY ([export_by]) REFERENCES [dbo].[account] ([ac_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table export_detail
-- ----------------------------
ALTER TABLE [dbo].[export_detail] ADD CONSTRAINT [FK_export_detail_export] FOREIGN KEY ([export_id]) REFERENCES [dbo].[export] ([export_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[export_detail] ADD CONSTRAINT [FK_export_detail_item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[item] ([item_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table food_ingredient
-- ----------------------------
ALTER TABLE [dbo].[food_ingredient] ADD CONSTRAINT [FK_food_ingredient_food] FOREIGN KEY ([food_id]) REFERENCES [dbo].[food] ([food_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[food_ingredient] ADD CONSTRAINT [FK_food_ingredient_item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[item] ([item_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table import
-- ----------------------------
ALTER TABLE [dbo].[import] ADD CONSTRAINT [FK_import_account_update] FOREIGN KEY ([update_by]) REFERENCES [dbo].[account] ([ac_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[import] ADD CONSTRAINT [FK_import_account] FOREIGN KEY ([created_by]) REFERENCES [dbo].[account] ([ac_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table import_detail
-- ----------------------------
ALTER TABLE [dbo].[import_detail] ADD CONSTRAINT [FK_import_detail_account] FOREIGN KEY ([import_by]) REFERENCES [dbo].[account] ([ac_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[import_detail] ADD CONSTRAINT [FK_import_detail_import] FOREIGN KEY ([import_id]) REFERENCES [dbo].[import] ([import_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[import_detail] ADD CONSTRAINT [FK_import_detail_item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[item] ([item_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table item
-- ----------------------------
ALTER TABLE [dbo].[item] ADD CONSTRAINT [FK_item_unit] FOREIGN KEY ([unit_id]) REFERENCES [dbo].[unit] ([unit_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table order_detail
-- ----------------------------
ALTER TABLE [dbo].[order_detail] ADD CONSTRAINT [FK_order_detail_order] FOREIGN KEY ([order_id]) REFERENCES [dbo].[list_order] ([oder_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[order_detail] ADD CONSTRAINT [FK_order_detail_food] FOREIGN KEY ([food_id]) REFERENCES [dbo].[food] ([food_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

