--drop table audience_reports
--select * from audience_reports order by stat_time_day desc
CREATE TABLE public.audience_reports(
	id								bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	campaign_id						bigint,
	stat_time_day					timestamp,
	age								varchar,
	gender							varchar,
	
	
	spend							double precision default(0),
	cpc								double precision default(0),
	cpm								double precision default(0),
	impressions						double precision default(0),
	clicks							double precision default(0),
	ctr								double precision default(0),
	conversion						double precision default(0),
	cost_per_conversion				double precision default(0),
	conversion_rate					double precision default(0),
	real_time_conversion			double precision default(0),
	real_time_cost_per_conversion	double precision default(0),
	real_time_conversion_rate		double precision default(0),
	result							double precision default(0),
	cost_per_result					double precision default(0),
	result_rate						double precision default(0),
	real_time_result				double precision default(0),
	real_time_cost_per_result		double precision default(0),
	real_time_result_rate			double precision default(0),
	created_at 						timestamp default(current_timestamp)
)
