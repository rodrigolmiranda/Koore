--select * From reports_async
--select * From audience_reports

--call import_audience_reports('C:\Users\guigo\source\repos\KooreApi\KooreApi\Reports\e734c877-0fcf-4e82-8652-a02b19e0423b.csv', null)
CREATE OR REPLACE PROCEDURE public.import_audience_reports(_report varchar, INOUT newLines int)
LANGUAGE plpgsql 
AS $$
DECLARE
declare d int;
declare sqltxt varchar ='COPY reports_async from '''||_report||''' DELIMITER '','' CSV HEADER';
begin

truncate table reports_async;
execute(sqltxt); 

--copy reports_async FROM $_report DELIMITER ',' CSV HEADER

--truncate table audience_reports
insert into audience_reports (
		campaign_id,
		stat_time_day,
		age,
		gender,
		spend,
		cpc,
		cpm,
		impressions,
		clicks,
		ctr,
		conversion,
		cost_per_conversion,
		conversion_rate,
		real_time_conversion,
		real_time_cost_per_conversion,
		real_time_conversion_rate,
		result,
		cost_per_result,
		result_rate,
		real_time_result,
		real_time_cost_per_result,
		real_time_result_rate)
		
select	ra.campaign_id,
		ra.stat_time_day,
		ra.age,
		ra.gender,
		ra.spend,
		ra.cpc,
		ra.cpm,
		ra.impressions,
		ra.clicks,
		ra.ctr,
		ra.conversion,
		ra.cost_per_conversion,
		ra.conversion_rate,
		ra.real_time_conversion,
		ra.real_time_cost_per_conversion,
		ra.real_time_conversion_rate,
		ra.result,
		ra.cost_per_result,
		ra.result_rate,
		ra.real_time_result,
		ra.real_time_cost_per_result,
		ra.real_time_result_rate
from reports_async ra
where not exists(select * 
				from	audience_reports 	ar
			  	where	ra.campaign_id 		= ar.campaign_id
				and		ra.stat_time_day	= ar.stat_time_day
				and		ra.gender			= ar.gender
				and		ra.age				= ar.age)
order by ra.stat_time_day, ra.campaign_id, ra.age, ra.gender;
get diagnostics newLines = row_count;

end
$$;
