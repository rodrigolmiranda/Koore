--select * from users
--select * from profiles
--truncate table profiles cascade
--drop function post_reset_password
--select post_reset_password('31a3308b-ea31-4d42-ad2d-6435993f9ca6', 'AOjs1ysr3enRq9ASUnBFYnE+0A8y3a9Lu3sSA9pLYcw5pI/U1oQ6iTq1w8d9w4CKAw==', 'hX7t2PrRfZjmEV3UDekqtg==')
CREATE OR REPLACE function post_reset_password
(
	pid	uuid,
	ppassword	varchar,
	psalt		varchar
)
RETURNS integer AS
$$
declare
rowsaffected integer;
begin

if exists(select 1 from users where id = pid) then
	
	update users
	set password = ppassword,
		salt = psalt
	where id = pid;
	
	get diagnostics rowsaffected = row_count;
	return rowsaffected;
else
	raise 'An account with email % already exists.', pemail;
end if;


end
$$ LANGUAGE plpgsql;

