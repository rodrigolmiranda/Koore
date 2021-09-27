--select * From advertisers

--call insert_update_advertiser(2, 'adfgdfgsd', null)
-- drop procedure insert_update_advertiser
CREATE OR REPLACE PROCEDURE public.insert_update_advertiser(padvertiser_id bigint, padvertiser_name varchar, INOUT newLines int)
LANGUAGE plpgsql 
AS $$
begin

--truncate table advertisers
if not exists(select 1 from advertisers where advertiser_id = padvertiser_id) then

	insert into advertisers (advertiser_id, advertiser_name) values (padvertiser_id, padvertiser_name);
	get diagnostics newLines = row_count;
else
	update advertisers
	set advertiser_name = padvertiser_name
	where advertiser_id = padvertiser_id;
	
	get diagnostics newLines = row_count;
end if;


end
$$;
