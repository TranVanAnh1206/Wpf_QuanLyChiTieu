use master 
go

if exists ( select 'true' from master.dbo.SysDatabases where name = 'QuanLiChiTieu')
drop database QuanLiChiTieu
go

BACKUP DATABASE QuanLiChiTieu
TO DISK = 'D:\Downloads\QuanLiChiTieu.bak'
WITH FORMAT, NAME = 'FileBackup_QuanLiChiTieu';

create database QuanLiChiTieu
go

use QuanLiChiTieu
go

create table Account
(
	AccID		varchar(6)		not null	primary key,
	AccName 	Nvarchar(250)	not null,
	AccPassword	Nvarchar(250)	not null,
	AccDisplayname Nvarchar(250) null,
	AccType		int				not null
)
go

alter table Account
add constraint uq_AccName unique (AccName)

select * from Account

-- Bảng user
create table Users 
(
	u_id		varchar(6)		not null	primary key,
	u_birth		date			null,
	u_Avatar_URL	varchar(255)	null,
	u_Address	Nvarchar(255)	null,
	AccID		varchar(6)		not null	foreign key(AccID) references Account(AccID)
)
go

insert into Users values ('', '2003-06-12', '', N'Định Tân - Yên ĐỊnh - Thanh Hóa', 'Acc001')

select * from Users

-- bảng loại thu nhập
create table RevenueType
(
	RevType_ID		varchar(6)		not null	primary key,
	RevType_Name	Nvarchar(250)	not null,
)
go

insert into RevenueType values ('rt001', N'Thu nhập chính'),
								('rt002', N'Làm thêm ngoài giờ'),
								('rt003', N'Các khoản thu nhập khác')
select * from RevenueType

-- bảng thu nhập
create table Revenue 
(
	Rev_ID		varchar(6)		not null	primary key, 
	Rev_Name	Nvarchar(250)	null,
	Rev_Date	date			null,
	Rev_Price	Money			not null,
	RevType_ID	varchar(6)		not null	foreign key(RevType_ID) references RevenueType(RevType_ID)
)
go

select format(Rev_Price, 'c', 'vi-VN') from Revenue

insert into Revenue values ('', N'Lập trình viên online', '2023-02-10', 200000000, 'rt001')
insert into Revenue values ('', N'Bưng bê ở quán cafe 1231', '2023-01-27', 3000000, 'rt002')

-- Bảng chi tiết chi tiêu
create table RevenueInfo
(
	RevI_ID		varchar(5)		not null primary key,
	RevI_Note	Nvarchar(255)	null,
	Rev_ID		varchar(6)		not null	foreign key(Rev_ID) references Revenue(Rev_ID)
)
go

-- bẳng loại chi tiêu
create table ExpensesType 
(
	ExpType_ID		varchar(6)		not null	primary key,
	ExpType_Name	Nvarchar(250)	not null,
)
go

insert into ExpensesType values ('et001', N'Chi tiêu cá nhân'),
								('et002', N'Chi tiêu gia đình'),
								('et003', N'Chi tiêu khác')
go

-- Bảng Chi tiêu
create table Expenses 
(
	Exp_ID		varchar(6)		not null	primary key,
	Exp_Name	Nvarchar(250)	null,
	Exp_Date	date			null,
	Exp_Price	Money			not null,
	ExpType_ID	varchar(6)		not null	foreign key(ExpType_ID) references ExpensesType(ExpType_ID)
)
go

select * from Expenses

-- ================

-- Thông tin chi tiêu
create table ExpenseInfo
(
	ExpI_ID		varchar(6)		not null	primary key,
	ExpI_Node	Nvarchar(255)	null,
	Ec_ID		varchar(5)		not null	foreign key(Ec_ID) references ExpenseCategories(Ec_ID),
	Exp_ID	varchar(6)		not null	foreign key(Exp_ID) references Expenses(Exp_ID)
)
go

select * from ExpenseInfo

delete ExpenseInfo

drop table ExpenseInfo

-- Danh mục chi tiêu
create table ExpenseCategories
(
	Ec_ID		varchar(5)		not null	primary key,
	Ec_Name		Nvarchar(100)	not null
)
go

insert into ExpenseCategories values ('', N'Nhà ở')
insert into ExpenseCategories values ('', N'Ăn uống')
insert into ExpenseCategories values ('', N'Đi lại')
insert into ExpenseCategories values ('', N'Y tế')
insert into ExpenseCategories values ('', N'Giáo dục')
insert into ExpenseCategories values ('', N'Giải trí')
insert into ExpenseCategories values ('', N'Mua sắm')
insert into ExpenseCategories values ('', N'Trang trí nhà cửa')
insert into ExpenseCategories values ('', N'Trả nợ')
insert into ExpenseCategories values ('', N'Bảo hiểm')
insert into ExpenseCategories values ('', N'Tiền hỗ trợ gia đình')
insert into ExpenseCategories values ('', N'Chi phí phát sinh khác')
insert into ExpenseCategories values ('', N'Làm đẹp')
go

select * from ExpenseCategories
go

create table Images 
(
	Img_ID		varchar(6)		not null	primary key,
	Img_Name	Nvarchar(255)	null,
	Img_Url		varchar(max)	null,
	Img_Relation_ID		varchar(6)		null,
	Img_Type	varchar(100)
)
go

insert into Images values ('', 'Img1', N'D:\Pictures\00059.png', 'Exp001', 'Expenses')

-- Thử join bảng image và users vào xem kết quả thu được ra sao nhé :D
SELECT * FROM images JOIN Expenses ON Expenses.Exp_ID = Img_Relation_ID
-- Còn đây là cách chúng ta thao tác để lấy ảnh của người dùng sử dùng type =))
SELECT images.* FROM images
join Expenses on Expenses.Exp_ID = Img_Relation_ID
WHERE Img_Relation_ID = 'Exp001' and Img_Type = 'Expenses'

select * from Images

delete from Images

Drop table Images
go

-- =============================================================

create function func_NextID( @lastUserID varchar(6), @prevfix varchar(4), @size int)
	returns varchar(6)
as
begin
	if (@lastUserID = '')
		set @lastUserID = @prevfix + REPLICATE(0,@size - LEN(@prevfix))
	declare @num_NextUserID int, @nextUserID varchar(6)
	set @lastUserID = LTRIM(RTRIM(@lastUserID))
	set @num_NextUserID = REPLACE(@lastUserID,@prevfix,'') + 1
	set @size = @size - LEN(@prevfix)
	set @nextUserID = @prevfix + REPLICATE(0,@size - LEN(@prevfix))
	set @nextUserID = @prevfix + RIGHT(REPLICATE(0,@size) + CONVERT(varchar(MAX), @num_NextUserID), @size)
	return @nextUserID
end
go

create trigger UTG_AccountID 
on Account for insert
as
begin
	declare @lastUserID varchar(6)
	set @lastuserID = (select top 1 AccID from Account order by AccID desc)
	update Account set AccID = dbo.func_NextID(@lastUserID, 'Acc', 6) where AccID = ''
end
go

create trigger UTG_RevenueID 
on Revenue for insert
as
begin
	declare @lastUserID varchar(6)
	set @lastuserID = (select top 1 Rev_ID from Revenue order by Rev_ID desc)
	update Revenue set Rev_ID = dbo.func_NextID(@lastUserID, 'Rev', 6) where Rev_ID = ''
end
go

create trigger UTG_ExpensesID 
on Expenses for insert
as
begin
	declare @lastUserID varchar(6)
	set @lastuserID = (select top 1 Exp_ID from Expenses order by Exp_ID desc)
	update Expenses set Exp_ID = dbo.func_NextID(@lastUserID, 'Exp', 6) where Exp_ID = ''
end
go

create trigger UTG_Images_ID 
on Images for insert
as
begin
	declare @lastUserID varchar(6)
	set @lastuserID = (select top 1 Img_ID from Images order by Img_ID desc)
	update Images set Img_ID = dbo.func_NextID(@lastUserID, 'Img', 6) where Img_ID = ''
end
go

create trigger UTG_ExpCateg_ID 
on ExpenseCategories for insert
as
begin
	declare @lastUserID varchar(5)
	set @lastuserID = (select top 1 Ec_ID from ExpenseCategories order by Ec_ID desc)
	update ExpenseCategories set Ec_ID = dbo.func_NextID(@lastUserID, 'Ec', 5) where Ec_ID = ''
end
go

create trigger UTG_User_ID 
on Users for insert
as
begin
	declare @lastUserID varchar(6)
	set @lastuserID = (select top 1 u_id from Users order by u_id desc)
	update Users set u_id = dbo.func_NextID(@lastUserID, 'Usr', 6) where u_id = ''
end
go

create trigger UTG_ExpInfo_ID 
on ExpenseInfo for insert
as
begin
	declare @lastUserID varchar(6)
	set @lastuserID = (select top 1 ExpI_ID from ExpenseInfo order by ExpI_ID desc)
	update ExpenseInfo set ExpI_ID = dbo.func_NextID(@lastUserID, 'ExI', 6) where ExpI_ID = ''
end
go

-- =========================================================================

create proc USP_LOGIN
@accName Nvarchar(250), @accPassword Nvarchar(100)
as
begin
	select * from Account
	where AccName = @accName and AccPassword = @accPassword
end
go

select * from Account
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
	select @Is_Password = COUNT(*) from Account where AccID  = @AccID

	if ( @Is_Password = 1 )
	begin
		if ( @AccNewPassword = null or @AccNewPassword = '' )
			update Account set AccDisplayname = @AccDisplayName where AccID = @AccID
		else 
			update Account set AccDisplayname = @AccDisplayName, AccPassword = @AccNewPassword where AccID = @AccID
	end
end
go

create proc ExpensesPersonal 
@ExpTypeID char(6)
as
begin
	select Exp_ID as N'Mã mặt hàng', Exp_Date as N'Ngày tháng', Exp_Name as N'Tên mặt hàng', Exp_Price as N'Giá thành', ExpType_Name as N'Loại chi tiêu' from Expenses, ExpensesType 
	where Expenses.ExpType_ID = ExpensesType.ExpType_ID and ExpensesType.ExpType_ID = @ExpTypeID
	order by Exp_Date asc
end
go

exec ExpensesPersonal 'et001'
go

create proc ExpensesFamily
@ExpTypeID char(6)
as
begin
	select Exp_ID as N'Mã mặt hàng', Exp_Date as N'Ngày tháng', Exp_Name as N'Tên mặt hàng', Exp_Price as N'Giá thành', ExpType_Name as N'Loại chi tiêu' from Expenses, ExpensesType 
	where Expenses.ExpType_ID = ExpensesType.ExpType_ID and ExpensesType.ExpType_ID = @ExpTypeID
	order by Exp_Date asc
end
go

exec ExpensesFamily 'et002'

--==========================================================================
insert into Account values ('', N'TranVanAnh', '1234', N'Tran van Anh', 1)
insert into Account values ('', N'TranVanAnh2', '123456', N'Tran van Anh (Khach)', 0)
insert into Account values ('', N'TranVanAnh 3', '123456', N'Tran van Anh 3 (Khach 3)', 0)
insert into Account values ('', N'TranVanAnh 4', '123456', N'Tran van Anh 3 (Khach 3)', 0)

select * from Account where AccName = 'TranVanAnh'
delete Account

exec USP_UpdateAccount 'Acc003', N'Presented by TVA', '', ''

insert into Expenses values ('',  N'Tiền trọ tháng 1','2023-02-05', 604000, 'et002')
insert into Expenses values ('',  N'Tiền Xe','2023-01-31', 200000, 'et001')
insert into Expenses  values ('', N'Đổi bình nước', '2023-02-06', 12000, 'et002')
insert into Expenses  values ('', N'Mua chai dầu ăn', '2023-02-06', 45000, 'et002')

insert into Expenses values ('',  N'Tiền trọ 1','2023-02-05', 604000, 'et002')
insert into Expenses values ('',  N'Tiền Xe 1','2023-01-31', 200000, 'et001')
insert into Expenses  values ('', N'Đổi bình nước 1', '2023-02-07', 12000, 'et002')
insert into Expenses  values ('', N'Mua chai dầu ăn 1', '2023-05-06', 45000, 'et002')
insert into Expenses values ('',  N'Tiền trọ 1','2023-03-23', 604000, 'et002')
insert into Expenses values ('',  N'Tiền Xe 1','2023-01-23', 200000, 'et001')
insert into Expenses  values ('', N'Đổi bình nước 1', '2022-12-06', 12000, 'et002')
insert into Expenses  values ('', N'Mua chai dầu ăn 1', '2021-01-16', 45000, 'et002')

select * from Expenses

select SUM(Exp_Price) as N'Total Price In Month' from Expenses where MONTH(Exp_Date) = 2

-- ======================================


select Exp_ID as N'Mã mặt hàng', Exp_Date as N'Ngày tháng', Exp_Name as N'Tên mặt hàng', FORMAT(Exp_Price, 'c', 'vi-VN') as N'Giá thành' 
from Expenses 
where ExpType_ID = 'et002' and MONTH(Exp_Date) = 7 and YEAR(Exp_Date) = 2023
order by Exp_Date asc

select Expenses.Exp_ID as N'Mã mặt hàng', Exp_Date as N'Ngày tháng', Exp_Name as N'Tên mặt hàng', Exp_Price as N'Giá thành', ExpenseInfo.ExpI_Node as N'Ghi chú', ExpenseCategories.Ec_Name
from Expenses 
Join ExpenseInfo ON ExpenseInfo.Exp_ID = Expenses.Exp_ID
join ExpenseCategories on ExpenseCategories.Ec_ID = ExpenseInfo.Ec_ID
where ExpType_ID = 'et002' and Month(Exp_Date) = 7

select COALESCE(SUM(Exp_Price),0) as N'Tổng' from Expenses 
where ExpType_ID = 'et002' and MONTH(Exp_Date) = 1 

SELECT *
FROM Expenses
LEFT JOIN ExpenseInfo ON Expenses.Exp_ID = ExpenseInfo.Exp_ID
where MONTH(Exp_Date) = 7



