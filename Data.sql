CREATE DATABASE QuanLyQuanCafe
GO
 
USE QuanLyQuanCafe
Go

--FOOD
--TABLE
--CATEGORY_FOOD
--ACCOUNT
--BILL
--BILL_INFO

CREATE TABLE TableFood(
    TableID int IDENTITY PRIMARY KEY,
    name nvarchar(100) not null,
	status nvarchar(100) DEFAULT N'Trống' -- Trống hoặc có người
)
go

Create table Account(
	AccountID int  IDENTITY PRIMARY KEY,
	displayName nvarchar(100) NOT NULL,
	userName nvarchar(100) NOT NULL UNIQUE,
	password nvarchar(500) NOT NULL,
	Type int NOT NULL DEFAULT 0 -- 1 Admin|| 0 staff 
)go

CREATE TABLE FoodCategory(
	 FoodCategoryID int IDENTITY PRIMARY KEY,
     name nvarchar(100) NOT NULL DEFAULT N'CHƯA ĐẶT TÊN'
)go

CREATE TABLE Food(
	 FoodID int IDENTITY PRIMARY KEY,
     name nvarchar(100),
	 CategoryID int Not null,
	 price float not null DEFAULT 0,
	 Foreign Key (CategoryID) references dbo.FoodCategory(FoodCategoryID)

)go


CREATE TABLE Bill(
	 billID int IDENTITY PRIMARY KEY,
	 dateCheckin DateTime NOT NULL DEFAULT GETDATE(),
	 dateCheckOut DateTime,
	 TableID INT  NOT NULL,
	 status int  NOT NULL DEFAULT 0,
	 Foreign Key (TableID) references dbo.TableFood(TableID)
)go

CREATE TABLE Billinfo(
	 billInfoID int IDENTITY PRIMARY KEY,
	 billID int NOT NULL,
	 foodID int NOT NULL,
	 count int NOT NULL DEFAULT 0,
	 Foreign Key (billID) references dbo.Bill(billID),
	 Foreign Key (foodID) references dbo.Food(FoodID)
)go