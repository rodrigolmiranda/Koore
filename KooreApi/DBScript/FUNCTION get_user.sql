--select * from users
--select * from profiles
--truncate table profiles cascade
--drop function get_user
--select get_user(pemail := 'thomas@hotmail.com')
--select get_user(pid := '31a3308b-ea31-4d42-ad2d-6435993f9ca6')
CREATE OR REPLACE function get_user
(
	pemail	varchar default(null),
	pid		uuid default(null)
)
RETURNS table (j json) AS
$$
begin

	RETURN QUERY --select row_to_json(u) from ( select id, email, password, salt from users where email = pemail or id = pid ) u;
select row_to_json(t)
from (	
		select u.id, email, password, salt, p as profile from users u join profiles p on u.profile = p.id where u.email = pemail or u.id = pid 
	) t;   
	

	
end
$$ LANGUAGE plpgsql;

