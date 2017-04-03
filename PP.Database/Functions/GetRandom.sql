CREATE function fn_random()
RETURNS varchar(16)
--with encryption
begin
declare @pass varchar(16)
declare @n varchar(max)
select @n=new_id from NewId
Select @pass=replace(SUBSTRING(CONVERT(varchar(255),CAST(@n as varchar(max)) ),8, 8),'-','')
return @pass
end

