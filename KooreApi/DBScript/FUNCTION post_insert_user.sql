--select * from users
--select * from profiles
--truncate table profiles cascade
--drop function post_insert_user
--select post_insert_user('rodrigolmiranda@gmail.com', 'asdasdasdasdasdasd', 'salt', 'Rodrigo', 'Miranda')
CREATE OR REPLACE function post_insert_user
(
	pemail	varchar,
	ppassword	varchar,
	psalt		varchar,
	pfirst_name varchar,
    plast_name	varchar)
RETURNS table (j json) AS
$$
declare
rowaffectProfile integer=0;
profileId uuid;
userId uuid;
begin

if not exists(select 1 from users where email = pemail) then
	profileId = uuid_generate_v4();
	--delete from campaigns
	insert into profiles (id, first_name, last_name)
	values (profileId, pfirst_name, plast_name);
	
	get diagnostics rowaffectProfile = row_count;
	if rowaffectProfile != 1 then
		raise 'An error ocurred while including profile​.';
	else
		userId = uuid_generate_v4();
		insert into users (id, email, password, salt, profile)
		values (userId, pemail, ppassword, psalt, profileId);
		
		RETURN QUERY select row_to_json(u) from ( select email, id from users where id = userId ) u;
	end if;
else
	raise 'An account with email % already exists.', pemail;
end if;


end
$$ LANGUAGE plpgsql;

