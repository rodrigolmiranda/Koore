--select * From reports_async
--select * From audience_reports
/*
call import_audience_reports('[{"campaign_name":"Florese Test 1","spend":0.42,"cpc":0.0,"cpm":6.09,"impressions":69,"clicks":0,"ctr":0.0,"conversion":0,"cost_per_conversion":0.0,"conversion_rate":0.0,"real_time_conversion":0,"real_time_cost_per_conversion":0.0,"real_time_conversion_rate":0.0,"result":0,"cost_per_result":0.0,"result_rate":0.0,"real_time_result":0,"real_time_cost_per_result":0.0,"real_time_result_rate":0.0},{"campaign_name":"Traffic20210930123839","spend":2.22,"cpc":0.13,"cpm":1.89,"impressions":1175,"clicks":17,"ctr":1.45,"conversion":1,"cost_per_conversion":2.22,"conversion_rate":5.88,"real_time_conversion":1,"real_time_cost_per_conversion":2.22,"real_time_conversion_rate":5.88,"result":17,"cost_per_result":0.1306,"result_rate":1.45,"real_time_result":17,"real_time_cost_per_result":0.1306,"real_time_result_rate":1.45},{"campaign_name":"Florese Test 1","spend":2.38,"cpc":0.0,"cpm":8.88,"impressions":268,"clicks":0,"ctr":0.0,"conversion":0,"cost_per_conversion":0.0,"conversion_rate":0.0,"real_time_conversion":0,"real_time_cost_per_conversion":0.0,"real_time_conversion_rate":0.0,"result":0,"cost_per_result":0.0,"result_rate":0.0,"real_time_result":0,"real_time_cost_per_result":0.0,"real_time_result_rate":0.0},{"campaign_name":"Florese 2","spend":0.0,"cpc":0.0,"cpm":0.0,"impressions":0,"clicks":0,"ctr":0.0,"conversion":0,"cost_per_conversion":0.0,"conversion_rate":0.0,"real_time_conversion":0,"real_time_cost_per_conversion":0.0,"real_time_conversion_rate":0.0,"result":0,"cost_per_result":0.0,"result_rate":0.0,"real_time_result":0,"real_time_cost_per_result":0.0,"real_time_result_rate":0.0},{"campaign_name":"Traffic20210930123839","spend":0.0,"cpc":0.0,"cpm":0.0,"impressions":0,"clicks":0,"ctr":0.0,"conversion":9,"cost_per_conversion":0.0,"conversion_rate":0.0,"real_time_conversion":9,"real_time_cost_per_conversion":0.0,"real_time_conversion_rate":0.0,"result":0,"cost_per_result":0.0,"result_rate":0.0,"real_time_result":0,"real_time_cost_per_result":0.0,"real_time_result_rate":0.0}]'
,'[{"campaign_id":1710135894381586,"stat_time_day":"2021-09-16 00:00:00","gender":"FEMALE","age":"AGE_13_17"},{"campaign_id":1712292991426561,"stat_time_day":"2021-10-05 00:00:00","gender":"MALE","age":"AGE_18_24"},{"campaign_id":1710135894381586,"stat_time_day":"2021-09-15 00:00:00","gender":"FEMALE","age":"AGE_18_24"},{"campaign_id":1710137408454705,"stat_time_day":"2021-09-15 00:00:00","gender":"FEMALE","age":"AGE_35_44"},{"campaign_id":1712292991426561,"stat_time_day":"2021-10-05 00:00:00","gender":"NONE","age":"NONE"}]'
,null)
*/
--call import_audience_reports('C:\Users\guigo\source\repos\KooreApi\KooreApi\Reports\e734c877-0fcf-4e82-8652-a02b19e0423b.csv', null)
--drop procedure import_audience_reports
CREATE OR REPLACE PROCEDURE public.import_audience_reports(metric json, dimension json, INOUT newLines int)
LANGUAGE plpgsql 
AS $$
DECLARE
declare d int;
--declare sqltxt varchar ='COPY reports_async from '''||_report||''' DELIMITER '','' CSV HEADER';
begin

truncate table reports_async;
--execute(sqltxt); 
insert into reports_async
(campaign_id,
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
select	dimension.campaign_id,
		dimension.stat_time_day,
		dimension.age,
		dimension.gender,
		metric.spend,
		metric.cpc,
		metric.cpm,
		metric.impressions,
		metric.clicks,
		metric.ctr,
		metric.conversion,
		metric.cost_per_conversion,
		metric.conversion_rate,
		metric.real_time_conversion,
		metric.real_time_cost_per_conversion,
		metric.real_time_conversion_rate,
		metric.result,
		metric.cost_per_result,
		metric.result_rate,
		metric.real_time_result,
		metric.real_time_cost_per_result,
		metric.real_time_result_rate
from (
		SELECT row_number() over ()as metid, r.*
		FROM	json_array_elements(metric) AS a(element),
				json_populate_record(NULL::audience_reports, a.element) AS r
	) as metric
join (
		SELECT row_number() over ()as dimid, r.*
		FROM
    			json_array_elements(dimension) AS a(element),
    			json_populate_record(NULL::audience_reports, a.element) AS r
	) as dimension
on metric.metid = dimension.dimid;
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
