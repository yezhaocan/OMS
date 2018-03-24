-------------------------------
-------[User] 用户表-----------
--------creator zxw-----------
-------------------------------
create table [User]
(
	Id int primary key identity(1,1) not null, 
	UserName nvarchar(32) not null, 
	UserPwd nvarchar(50) not null ,
	[Name] nvarchar(15) null,
	Email nvarchar(50) null,
	PhoneNumber nvarchar(15) null,
	Salt nvarchar(32) not null,
	LastLoginTime datetime2 null,
	LastLoginIp nvarchar(40) null,
	[State] char(1) default('1') not null, --'0' 未激活;'1'正常;'2'禁用
	
	[Isvalid] [bit] NULL,
	[ModifiedBy] [int] NULL,	
	[ModifiedTime] [datetime] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[CreatedBy] [int] NULL
)

----------------------------------
---------[Dictionary]  数据字典表
---------creator whf-------
---------------------------------
CREATE TABLE Dictionary
(
   Id              INT             NOT NULL       PRIMARY KEY     IDENTITY(1,1),
   [Type]          INT             NOT NULL ,
   Value           NVARCHAR(200)   NOT NULL,
   
   [Isvalid] [bit] NULL,
   [ModifiedBy] [int] NULL,	
   [ModifiedTime] [datetime] NOT NULL,
   [CreatedTime] [datetime] NOT NULL,
   [CreatedBy] [int] NULL      
)
GO
----------------------------------
---------[Product]  商品表
---------creator whf-------
---------------------------------
CREATE TABLE Product
(
  Id                 INT                NOT NULL        PRIMARY KEY     IDENTITY(1,1),
  Name               NVARCHAR(200)      NOT NULL,
  NameEn             NVARCHAR(200)      NULL,
  [Type]             INT                NOT NULL DEFAULT(0),
  Cover              NVARCHAR(200)      NULL,
  Country            INT                NULL DEFAULT(0),
  Area               INT                NULL DEFAULT(0),
  Grapes             NVARCHAR(100)      NULL,
  Capacity           INT                NULL DEFAULT(0),
  [Year]             NVARCHAR(100)      NULL,
  Packing            INT                NULL DEFAULT(0),
  CategoryId         INT                NULL,
  Code               NVARCHAR(100)      NOT NULL,

   [Isvalid] [bit] NULL,
   [ModifiedBy] [int] NULL,	
   [ModifiedTime] [datetime] NOT NULL,
   [CreatedTime] [datetime] NOT NULL,
   [CreatedBy] [int] NULL      
)
GO



