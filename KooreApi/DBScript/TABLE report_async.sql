/*
select sum(spend) as spend, sum(cpc) as cpc, sum(cpm) as cpm, sum(impressions) as impressions, sum(clicks) as clicks, sum(ctr) as ctr, sum(conversion) as conversion, sum(cost_per_conversion) as cost_per_conversion, sum(conversion_rate) as conversion_rate, sum(real_time_conversion) as real_time_conversion, sum(real_time_cost_per_conversion) as real_time_cost_per_conversion, sum(real_time_conversion_rate) as real_time_conversion_rate, sum(result) as result, sum(cost_per_result) as cost_per_result, sum(result_rate) as result_rate, sum(real_time_result) as real_time_result, sum(real_time_cost_per_result) as real_time_cost_per_result, sum(real_time_result_rate) as real_time_result_rate From reports_async
select * from reports_async where age = 'AGE_45_54' and stat_time_day = '7/09/2021 0:00'

drop table reports_async
truncate table reports_async
*/
CREATE TABLE public.reports_async
(
	advertiser_id	bigint,
	campaign_id	bigint,
	adgroup_id	bigint,
	ad_id	bigint,
	stat_time_day	date,
	stat_time_hour	timestamp,
	gender	varchar,
	age	varchar,
	country_code	varchar,
	province_id	bigint,
	dma_id	bigint,
	ac	varchar,
	language	varchar,
	platform	varchar,
	interest_category	varchar,
	interest_category_tier2	varchar,
	interest_category_tier3	varchar,
	interest_category_tier4	varchar,
	behavior_id	bigint,
	placement	varchar,
	device_brand_id	bigint,
	campaign_name	varchar,
	adgroup_name	varchar,
	ad_name	varchar,
	ad_text	varchar,
	tt_app_id	varchar,
	tt_app_name	varchar,
	mobile_app_id	varchar,
	device_brand_name	varchar,
	behavior_name	varchar,
	action_category	varchar,
	action_scene	varchar,
	user_action	varchar,
	action_period	varchar,
	promotion_type	varchar,
	dpa_target_audience_type	varchar,
	
	spend	double precision,
	cpc	double precision,
	cpm	double precision,
	impressions	double precision,
	clicks	double precision,
	ctr	double precision,
	conversion	double precision,
	cost_per_conversion	double precision,
	conversion_rate	double precision,
	real_time_conversion	double precision,
	real_time_cost_per_conversion	double precision,
	real_time_conversion_rate	double precision,
	result	double precision,
	cost_per_result	double precision,
	result_rate	double precision,
	real_time_result	double precision,
	real_time_cost_per_result	double precision,
	real_time_result_rate	double precision
);

--copy reports_async FROM 'C:\Users\guigo\source\repos\KooreApi\KooreApi\Reports\teste.csv' DELIMITER ',' CSV HEADER 
--copy reports_async (advertiser_id, campaign_id, adgroup_id, ad_id, stat_time_day, stat_time_hour, gender, age, country_code, province_id, dma_id, ac, language, platform, interest_category, interest_category_tier2, interest_category_tier3, interest_category_tier4, behavior_id, placement, device_brand_id, campaign_name, adgroup_name, ad_name, ad_text, tt_app_id, tt_app_name, mobile_app_id, device_brand_name, behavior_name, action_category, action_scene, user_action, action_period, promotion_type, dpa_target_audience_type, real_time_cost_per_result, conversion, real_time_conversion, real_time_result, cpm, ctr, cost_per_conversion, result_rate, real_time_cost_per_conversion, clicks, conversion_rate, real_time_conversion_rate, impressions, cost_per_result, real_time_result_rate, spend, cpc, result) FROM 'C:\\Users\\guigo\\source\\repos\\KooreApi\\KooreApi\\Reports\\84e1cae6-8700-49da-85ff-ce37e7356c44.csv' DELIMITER ',' CSV HEADER
