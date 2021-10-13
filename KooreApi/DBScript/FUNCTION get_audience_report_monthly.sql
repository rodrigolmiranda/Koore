--drop function get_audience_report_monthly
--select get_audience_report_monthly(pstat_time_day_min := '2021-02-01 00:00');

CREATE OR REPLACE function get_audience_report_monthly
(
	pstat_time_day_min		timestamp DEFAULT(null),
	pstat_time_day_max		timestamp DEFAULT(null)
)
RETURNS table (j json) AS
$$
BEGIN
RETURN QUERY select array_to_json(array_agg(row_to_json(t)))
    from (	select 
				trim(TO_CHAR(ar.stat_time_day, 'Mon')) || '-' || date_part('year', ar.stat_time_day) as stat_month_year,
				trim(TO_CHAR(ar.stat_time_day, 'Mon')) as stat_month,
		  		date_part('year', ar.stat_time_day) as stat_year,
				sum(ar.spend) as spend,
				sum(ar.cpc) as cpc,
				sum(ar.cpm) as cpm,
				sum(ar.impressions) as impressions,
				sum(ar.clicks) as clicks,
				sum(ar.ctr) as ctr,
				sum(ar.conversion) as conversion,
				sum(ar.cost_per_conversion) as cost_per_conversion,
				sum(ar.conversion_rate) as conversion_rate,
				sum(ar.real_time_conversion) as real_time_conversion,
				sum(ar.real_time_cost_per_conversion) as real_time_cost_per_conversion,
				sum(ar.real_time_conversion_rate) as real_time_conversion_rate,
				sum(ar.result) as result,
				sum(ar.cost_per_result) as cost_per_result,
				sum(ar.result_rate) as result_rate,
				sum(ar.real_time_result) as real_time_result,
				sum(ar.real_time_cost_per_result) as real_time_cost_per_result,
				sum(ar.real_time_result_rate) as real_time_result_rate
		from	public.audience_reports ar
		where
			(ar.stat_time_day >= pstat_time_day_min
		or	pstat_time_day_min is null)
		and	(ar.stat_time_day <= pstat_time_day_max
		or	pstat_time_day_max is null)
		group by trim(TO_CHAR(ar.stat_time_day, 'Mon')) || '-' || 	date_part('year', ar.stat_time_day), 
			trim(TO_CHAR(ar.stat_time_day, 'Mon')),
		  	date_part('year', ar.stat_time_day),
			date_part('month', ar.stat_time_day)
		  order by stat_year, date_part('month', ar.stat_time_day)
	) t;
END;
$$ LANGUAGE plpgsql;
