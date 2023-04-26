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
--alter food
Alter table Food
Add sellStop Int Default 0
UPDATE Food SET sellStop = 0

CREATE TABLE Bill(
	 billID int IDENTITY PRIMARY KEY,
	 dateCheckin DateTime NOT NULL DEFAULT GETDATE(),
	 dateCheckOut DateTime,
	 TableID INT  NOT NULL,
	 status int  NOT NULL DEFAULT 0,
	 Foreign Key (TableID) references dbo.TableFood(TableID)
)go

--ALTER Bill
Alter table Bill
Add Discount Int Default 0

CREATE TABLE Billinfo(
	 billInfoID int IDENTITY PRIMARY KEY,
	 billID int NOT NULL,
	 foodID int NOT NULL,
	 count int NOT NULL DEFAULT 0,
	 Foreign Key (billID) references dbo.Bill(billID),
	 Foreign Key (foodID) references dbo.Food(FoodID)
)go

-- add dữ liệu 

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
--add category

INSERT dbo.FoodCategory(name)
values (N'Cafe')

INSERT dbo.FoodCategory(name)
values (N'Nước Ngọt')

INSERT dbo.FoodCategory(name)
values (N'Bánh ngọt')

--add Food


INSERT dbo.Food(name,CategoryID,price)
values (N'Capuchino',1, 25000)

INSERT dbo.Food(name,CategoryID,price)
values (N'Coca',2, 10000)

INSERT dbo.Food(name,CategoryID,price)
values (N'Pepsi',2, 10000)

INSERT dbo.Food(name,CategoryID,price)
values (N'Bánh flan',3, 10000)


--add bill

insert dbo.Bill(dateCheckin,dateCheckOut,TableID,statusBill)
values(GETDATE(),
	 null,
	 12,
	 0
)
-- add billInfo
Insert dbo.Billinfo(billID,foodID,count)
values(2,1,2)



-- create Procedure
-- Login

go 
alter proc PR_Login
@username nvarchar(100), @password nvarchar(100)
as
begin 
	select * from dbo.Account 
	where userName = @username COLLATE Latin1_General_CS_AS  and password = @password COLLATE Latin1_General_CS_AS 
end


-- create table
DECLARE  @i int = 1
while @i <= 10
Begin
	Insert dbo.TableFood (nameTable) values (N'Bàn ' + CAST( @i as nvarchar(100)))
	set @i = @i+1
end

-- create Procedure load table
go 
create proc PR_LoadTable
as select *from TableFood
exec PR_LoadTable


-- Create Procedure insertBill
go
alter proc PR_InsertBill
@TableId int
as
begin
	insert dbo.Bill(dateCheckin,dateCheckOut,TableID,statusBill, Discount)
	values(GETDATE(),
			 null,
			 @TableId,
			 0,
			 0
	)

end

--Create procedure InsertBillInfo
go 
alter proc PR_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	Declare @isExitsBillinfo int = -1
	Declare @foodCount int = 1
	Declare @countBillInfobyIdBill int = 0

	select @isExitsBillinfo	 = billInfoID, @foodCount = b.count
	from dbo.Billinfo b
	where billID = @idBill and foodID = @idFood

	if (@isExitsBillinfo > 0)
		begin 
			Declare @newcount int = @foodCount + @count
			if(@newcount > 0)
				update dbo.Billinfo set count = @foodCount + @count where foodID = @idFood and billID = @idBill
			else
				begin
					Delete dbo.Billinfo where billID = @idBill and foodID = @idFood

					select @countBillInfobyIdBill = COUNT(*)
					from dbo.Billinfo
					where billID = @idBill
					if(@countBillInfobyIdBill = 0)
						delete dbo.Bill where billID = @idBill
				end
		end
	else
		begin
			if( @count <0 )
				return
			Insert dbo.Billinfo(billID,foodID,count)
			values(@idBill,@idFood,@count)
		end
end

--Create trigger catch event insert, update on bill
go
alter trigger  TR_TriggerBill
on dbo.Bill for insert, update
as
begin
	DECLARE  @idBill int
	select @idBill = billID  from inserted 

	DECLARE @TableId int
	select @TableId = TableID from dbo.Bill where billID = @idBill	

	DECLARE @count int = 0
	select @count = COUNT(*) from dbo.Bill where TableID = @TableId and statusBill = 0

	if(@count = 0 )
		update dbo.TableFood set statusTable = N'Trống' where TableID = @TableId
	else 
		update dbo.TableFood set statusTable = N'Có người' where TableID = @TableId
end

-- xử lý bắt sự kiện delete bill







--Create trigger catch event delete on bill
go
alter TRIGGER tr_delete_bill
ON Bill
AFTER DELETE
AS
BEGIN
	Create TABLE #temp_deleted_bill (
	  id INT,
	  table_id INT,
	  statusbill int,
	);
	INSERT INTO #temp_deleted_bill (id, table_id, statusbill)
	SELECT billID, TableID, statusBill
	FROM deleted;

	
	DECLARE @TableId int
	select @TableId = table_id from #temp_deleted_bill 

	DECLARE @count int = 0
	select @count = COUNT(*) from dbo.Bill where TableID = @TableId and statusBill = 0

	if(@count = 0 )
		update dbo.TableFood set statusTable = N'Trống' where TableID = @TableId
END;



---- create procedure Switch Table
GO
alter proc	PR_SwitchTable
@IdTableOld int,
@IdTableNew int,
@IdBillOld int,
@IdBillNew int
as
begin 
	Update dbo.TableFood set statusTable = N'Trống' where TableID = @IdTableOld

	UPDATE Billinfo
	SET [count] = [count] + COALESCE(
	  (SELECT SUM([count]) FROM (
		  SELECT foodID, SUM([count]) as [count]
		  FROM Billinfo
		  WHERE billID = @IdBillOld
		  GROUP BY foodID
		) AS t
		WHERE t.foodID = Billinfo.foodID
	  ),
	  0
	 )
	WHERE billID = @IdBillNew AND foodID IN (SELECT foodID FROM Billinfo WHERE billID = @IdBillOld)


	update Billinfo set billID = @IdBillNew where billID = @IdBillOld and foodID not in (select foodID from Billinfo where billID = @IdBillNew)
	Delete Billinfo where  billID = @IdBillOld
	Delete Bill where billID = @IdBillOld
end



-- procedure insert Food
go
alter proc PR_InsertFood
@name nvarchar(100),
@category int,
@price float,
@sellStop int
as
begin
	insert dbo.Food(name,CategoryID,price,sellStop)
	values(
		@name,
		@category,
		@price,
		@sellStop
	)
end	

exec PR_InsertFood @name , @category , @price

-- procedure deleteFood
GO
alter proc PR_DeleteFood
@FOODID INT
as
begin

	Delete Food where FoodID = @FOODID and FoodID not in(Select FoodID from Billinfo)
	
end

--Trigger delete Billinfo

go 
create TRIGGER TR_DeleteBillinfo
on dbo.Billinfo for delete
as
begin
	Declare @idBIllInfo int
	Declare @idBill int
	Declare @idTable int
	Declare @count int = 0
	select @idBIllInfo =  billInfoID, @idBill = billID From deleted

	select @idTable = TableID from Bill where billID = @idBill
	
	select @count = COUNT(*) from dbo.Billinfo as bi ,dbo.Bill as b where bi.billID = @idBill and b.billID = bi.billID and b.statusBill = 0

	if(@count = 0 )
		update dbo.TableFood set statusTable = N'Trống' where TableID = @idTable
	
end

--procedure deleteCategory when Food null
go 
alter proc PR_DELETECategory 
@id int
as
begin
	declare @count int = -1
	select @count = COUNT(*)
	from Food where CategoryID = @id

	if( @count = 0)
		delete FoodCategory where FoodCategoryID = @id
	
end


-- create proc Get Top selling of the month
go 
create procedure GetTopSellingOfTheMonth
as
begin
	SELECT TOP 10 Food.name, SUM(Billinfo.count) AS count, price = 0, total = 0
    FROM Food join Billinfo ON Food.FoodID = Billinfo.foodID join Bill ON Billinfo.billID = Bill.billID
	Where  dateCheckOut >=DATEADD(DAY, -30, GETDATE())  AND dateCheckOut <= getdate()
    GROUP BY Food.name
    ORDER BY count DESC
end

-- create proc Get Top selling of the week

go 
create procedure GetTopSellingOfTheWeek
as
begin
	SELECT TOP 10 Food.name, SUM(Billinfo.count) AS count, price = 0, total = 0
    FROM Food join Billinfo ON Food.FoodID = Billinfo.foodID join Bill ON Billinfo.billID = Bill.billID
	Where  dateCheckOut >=DATEADD(DAY, -7, GETDATE())  AND dateCheckOut <= getdate()
    GROUP BY Food.name
    ORDER BY count DESC
end


go 
alter procedure GetTopSellingOfTheDay
as
begin
	SELECT TOP 10 Food.name, SUM(Billinfo.count) AS count, price = 0, total = 0
    FROM Food join Billinfo ON Food.FoodID = Billinfo.foodID join Bill ON Billinfo.billID = Bill.billID
	WHERE CAST(dateCheckOut AS DATE) = CAST(GETDATE() AS DATE)
    GROUP BY Food.name
    ORDER BY count DESC
end

exec GetTopSellingOfTheDay

select * from Bill
select * from Food
select * from Billinfo
select * from TableFood
select * from Account
select * from FoodCategory

delete dbo.Bill
delete dbo.Billinfo
delete dbo.TableFood

DBCC CHECKIDENT ('Bill', RESEED, 0);
DBCC CHECKIDENT ('Billinfo', RESEED, 0);
DBCC CHECKIDENT ('TableFood', RESEED, 0);
DBCC CHECKIDENT ('Food', RESEED, 0);



-- doanh thu thống kê doanh thu ra Bill/  bàn/  ngày vào/ ngày ra tổng tiền	
	
	