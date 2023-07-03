use master 
go

if exists ( select 'true' from master.dbo.SysDatabases where name = 'QuanLiChiTieu_V2')
drop database QuanLiChiTieu_V2
go

create database QuanLiChiTieu_V2
go

use QuanLiChiTieu_V2
go

create table Accounts
(
	AccID		INT IDENTITY(1,1) not null PRIMARY KEY,
	AccName 	Nvarchar(250)	not null,
	AccPassword	Nvarchar(250)	not null,
	AccDisplayname Nvarchar(250) null,
)
go

select * from Accounts
go

-- Bảng user
create table Users 
(
	u_id		INT IDENTITY(1,1) not null PRIMARY KEY,
	u_birth		date			null,
	u_Avatar_URL	varchar(255)	null,
	u_Address	Nvarchar(255)	null,
	AccID		int		not null	foreign key(AccID) references Accounts(AccID)
)
go

select * from Users
go

-- bảng loại thu nhập
create table RevenueCategories
(
	RevCateg_ID		INT IDENTITY(1,1) not null PRIMARY KEY,
	RevCateg_Name	Nvarchar(250)	not null,
)
go

insert into RevenueCategories values 
(N'Tiền lương'),
(N'Tiền thưởng'),
(N'Tiền lãi'),
(N'Tiền thuê nhà'),
(N'Thu nhập từ kinh doanh'),
(N'Thu nhập từ nghệ thuật'),
(N'Thu nhập từ viết blog hoặc viết sách'),
(N'Khoản thu nhập không chính thức')
go

select * from RevenueCategories
go

-- bảng thu nhập
create table Revenues
(
	Rev_ID		INT		IDENTITY(1,1)	not null	PRIMARY KEY, 
	Rev_Name	Nvarchar(250)	null,
	Rev_Date	date			null,
	Rev_Price	Money			not null	
)
go

select * from Revenues
go

-- Bảng chi tiết chi tiêu
create table RevenueInfo
(
	RevI_ID		INT		IDENTITY(1,1)	not null	PRIMARY KEY,
	RevI_Note	Nvarchar(255)	null,
	Rev_ID		INT		not null	foreign key(Rev_ID) references Revenues(Rev_ID),
	RevCateg_ID	Int		not null	foreign key(RevCateg_ID) references RevenueCategories(RevCateg_ID)
)
go

select * from RevenueInfo
go

--	MỚI SỬA ĐẾN ĐÂY =============================================================================================
-- Bảng Chi tiêu
create table Fml_Expenses
(
	F_exp_ID	INT IDENTITY(1,1) not null PRIMARY KEY,
	F_exp_Name	Nvarchar(250)	null,
	F_exp_Date	Date			null,
	F_exp_Price	Money			not null,
)
go

select * from Fml_Expenses

-- Thông tin chi tiêu
create table Fml_ExpenseInfo
(
	F_expI_ID		INT		IDENTITY(1,1) not null PRIMARY KEY,
	F_expI_Note		Nvarchar(255)		null,
	Ec_ID			Int		not null	foreign key(Ec_ID) references ExpenseCategories(Ec_ID),
	F_exp_ID		Int		not null	foreign key(F_exp_ID) references Fml_Expenses(F_exp_ID)
)
go

select * from Fml_ExpenseInfo
go

-- ================
create table Psn_Expenses
(
	P_exp_ID		INT IDENTITY(1,1) not null PRIMARY KEY,
	P_exp_Name	Nvarchar(250)	null,
	P_exp_Date	Date			null,
	P_exp_Price	Money			not null,
)
go

select * from Psn_Expenses
go

create table Psn_ExpenseInfo
(
	P_expI_ID		INT IDENTITY(1,1) not null PRIMARY KEY,
	P_expI_Note		Nvarchar(255)		null,
	Ec_ID			Int			not null	foreign key(Ec_ID) references ExpenseCategories(Ec_ID),
	P_exp_ID		Int		not null	foreign key(P_exp_ID) references Psn_Expenses(P_exp_ID)
)
go

select * from Psn_Expenses
select * from Psn_ExpenseInfo
SELECT * FROM images 
go

-- ================

-- Danh mục chi tiêu
create table ExpenseCategories
(
	Ec_ID		INT IDENTITY(1,1) not null PRIMARY KEY,
	Ec_Name		Nvarchar(200)	not null
)
go

insert into ExpenseCategories values (N'Nhà ở')
insert into ExpenseCategories values (N'Ăn uống')
insert into ExpenseCategories values (N'Đi lại')
insert into ExpenseCategories values (N'Y tế')
insert into ExpenseCategories values (N'Giáo dục')
insert into ExpenseCategories values (N'Giải trí')
insert into ExpenseCategories values (N'Mua sắm')
insert into ExpenseCategories values (N'Trang trí nhà cửa')
insert into ExpenseCategories values (N'Trả nợ')
insert into ExpenseCategories values (N'Bảo hiểm')
insert into ExpenseCategories values (N'Tiền hỗ trợ gia đình')
insert into ExpenseCategories values (N'Làm đẹp')
insert into ExpenseCategories values (N'Học tập')
insert into ExpenseCategories values (N'Chi phí phát sinh khác')
go

select * from ExpenseCategories
go

-- ===========================================

create table Images 
(
	Img_ID		INT IDENTITY(1,1) not null PRIMARY KEY,
	Img_Name	Nvarchar(255)	null,
	Img_Url		varchar(max)	null,
	Img_Relation_ID		int		null, -- Tương tự như khóa ngoài
	Img_Type	varchar(100)
)
go

SELECT * FROM images 
go

create table Videos 
(
	Vid_ID		INT IDENTITY(1,1) not null PRIMARY KEY,
	Vid_Name	Nvarchar(255)	null,
	Vid_Url		varchar(max)	null,
	Vid_Relation_ID		int		null, -- Tương tự như khóa ngoài
	Vid_Type	varchar(100)
)
go

SELECT * FROM Videos 
go

-- ===========================================

create proc USP_LOGIN
@accName Nvarchar(250), @accPassword Nvarchar(100)
as
begin
	select * from Accounts
	where AccName = @accName and AccPassword = @accPassword
end
go

select * from Accounts
exec USP_LOGIN 'TranVanAnh', '1234'
go

create proc USP_UpdateAccount
@AccID				varchar(6),
@AccDisplayName		Nvarchar(250),
@AccNewPassword		Nvarchar(100),
@AccPasswordConfirm	Nvarchar(100)
as
begin 
	declare @Is_Password int = 0
	select @Is_Password = COUNT(*) from Accounts where AccID  = @AccID

	if ( @Is_Password = 1 )
	begin
		if ( @AccNewPassword = null or @AccNewPassword = '' )
			update Accounts set AccDisplayname = @AccDisplayName where AccID = @AccID
		else 
			update Accounts set AccDisplayname = @AccDisplayName, AccPassword = @AccNewPassword where AccID = @AccID
	end
end
go

--create proc Psn_Expenses 
--@ExpTypeID char(6)
--as
--begin
--	select Exp_ID as N'Mã mặt hàng', Exp_Date as N'Ngày tháng', Exp_Name as N'Tên mặt hàng', Exp_Price as N'Giá thành', ExpType_Name as N'Loại chi tiêu' from Expenses, ExpensesType 
--	where Expenses.ExpType_ID = ExpensesType.ExpType_ID and ExpensesType.ExpType_ID = @ExpTypeID
--	order by Exp_Date asc
--end
--go

--exec Psn_Expenses 1
--go

--create proc ExpensesFamily
--@ExpTypeID char(6)
--as
--begin
--	select Exp_ID as N'Mã mặt hàng', Exp_Date as N'Ngày tháng', Exp_Name as N'Tên mặt hàng', Exp_Price as N'Giá thành', ExpType_Name as N'Loại chi tiêu' from Expenses, ExpensesType 
--	where Expenses.ExpType_ID = ExpensesType.ExpType_ID and ExpensesType.ExpType_ID = @ExpTypeID
--	order by Exp_Date asc
--end
--go

--exec ExpensesFamily 1

--==========================================================================
insert into Accounts values 
( N'TranVanAnh', '1234', N'Tran van Anh')

select * from Accounts


