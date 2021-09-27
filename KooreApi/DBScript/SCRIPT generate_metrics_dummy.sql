--select (array['FEMALE', 'MALE', 'NONE'])[floor(random() * 3 + 1)]
--select (array['AGE_13_17','AGE_18_24', 'AGE_25_34', 'AGE_35_44', 'AGE_45_54', 'AGE_55_100'])[floor(random() * 6 + 1)]


--select uuid_generate_v1()
--select current_timestamp
--select floor(random()*(10000-100+1))+100;
--SELECT TO_TIMESTAMP('2021-09-06 00:00', 'YYYY-MM-DD HH:MI') + interval '1 day'
--select * from public.audience_reports
--truncate table audience_reports
do $$
declare
   n integer:= 10;
   fib integer := 0;
   counter integer := 0 ; 
   i integer := 0 ; 
   j integer := 1 ;
   --d timestamp = '2021-09-06 11:00';
   startdate timestamp = '2015-01-01 00:00';
   enddate timestamp = '2021-09-06 00:00';
begin
	if (n < 1) then
		fib := 0 ;
	end if; 
	loop 
		exit when startdate > enddate ; 
		raise notice '%', startdate; 
		counter := counter + 1 ; 
		--select j, i + j into i,	j ;
		insert into audience_reports (campaign_id, stat_time_day, age, gender, 
									  	spend,
										cpc,
										cpm,
										impressions,
										clicks,
										ctr)
			select 	1 as campaign_id, 
					startdate as stat_time_day, 
					(array['AGE_13_17','AGE_18_24', 'AGE_25_34', 'AGE_35_44', 'AGE_45_54', 'AGE_55_100'])[floor(random() * 6 + 1)] as age, 
					(array['FEMALE', 'MALE', 'NONE'])[floor(random() * 3 + 1)] as gender, 
					floor(random()*(10-0.1+1))*0.9 as spend, 
					floor(random()*(5-0.1+1))*0.9 cpc, 
					floor(random()*(10-0.1+1))*0.9 cpm, 
					floor(random()*(20-10+1)) + 5 impressions, 
					floor(random()*(10-5.01+1))+5 clicks, 
					floor(random()*(10-1+1)) *0.1 ctr;
		select startdate + interval '1 day' into startdate;
		
	end loop; 
	--fib := i;
    --raise notice '%', fib; 
end; $$
