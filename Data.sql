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

-- Thêm dữ liệu 

INSERT INTO dbo.Account(userName,displayName,password,Type)
VALUES(
	N'Admin',
	N'Admin',
	N'Admin123',
	1
)
INSERT INTO dbo.Account(userName,displayName,password,Type)
VALUES(
	N'Staff',
	N'Staff',
	N'Staff123',
	0
)
--Thêm category

INSERT dbo.FoodCategory(name)
values (N'Cafe')

INSERT dbo.FoodCategory(name)
values (N'Nước Ngọt')

INSERT dbo.FoodCategory(name)
values (N'Bánh ngọt')

--Thêm Food


INSERT dbo.Food(name,CategoryID,price)
values (N'Capuchino',1, 25000)

INSERT dbo.Food(name,CategoryID,price)
values (N'Coca',2, 10000)

INSERT dbo.Food(name,CategoryID,price)
values (N'Pepsi',2, 10000)

INSERT dbo.Food(name,CategoryID,price)
values (N'Bánh flan',3, 10000)


--Thêm bill

insert dbo.Bill(dateCheckin,dateCheckOut,TableID,statusBill)
values(GETDATE(),
	 null,
	 12,
	 0
)
-- thêm billInfo
Insert dbo.Billinfo(billID,foodID,count)
values(2,1,2)



-- tạo Procedure
-- Login

go 
create proc PR_Login
@username nvarchar(100), @password nvarchar(100)
as
begin 
	select * from dbo.Account 
	where userName = @username and password = @password COLLATE Latin1_General_CS_AS 
end


-- tạo bàn
DECLARE  @i int = 1
while @i <= 10
Begin
	Insert dbo.TableFood (nameTable) values (N'Bàn ' + CAST( @i as nvarchar(100)))
	set @i = @i+1
end

-- tạo Procedure load table
go 
create proc PR_LoadTable
as select *from TableFood

exec PR_LoadTable


select 
from Food f, Bill B, Billinfo Bi
where f.FoodID = Bi.billID and bi.billID = b.billID
