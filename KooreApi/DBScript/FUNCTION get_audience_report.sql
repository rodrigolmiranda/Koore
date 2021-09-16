--drop function get_audience_report
--select get_audience_report(38)
--select get_audience_report(pstat_time_day_min := '2021-02-01');


CREATE OR REPLACE function get_audience_report
(
	pstat_time_day_min		timestamp DEFAULT(null),
	pstat_time_day_max		timestamp DEFAULT(null)
)
RETURNS table (j json) AS
$$
BEGIN
RETURN QUERY select array_to_json(array_agg(row_to_json(t)))
    from (	select 
				ar.id,
				ar.campaign_id,
				ar.stat_time_day,
				ar.age,
				ar.gender,
				ar.spend,
				ar.cpc,
				ar.cpm,
				ar.impressions,
				ar.clicks,
				ar.ctr,
				ar.conversion,
				ar.cost_per_conversion,
				ar.conversion_rate,
				ar.real_time_conversion,
				ar.real_time_cost_per_conversion,
				ar.real_time_conversion_rate,
				ar.result,
				ar.cost_per_result,
				ar.result_rate,
				ar.real_time_result,
				ar.real_time_cost_per_result,
				ar.real_time_result_rate,
				ar.created_at
		from	public.audience_reports ar
		where
			(ar.stat_time_day >= pstat_time_day_min
		or	pstat_time_day_min is null)
		and	(ar.stat_time_day <= pstat_time_day_max
		or	pstat_time_day_max is null)
		order by ar.stat_time_day desc) t;
		
END;
$$ LANGUAGE plpgsql;

--select get_audience_report();