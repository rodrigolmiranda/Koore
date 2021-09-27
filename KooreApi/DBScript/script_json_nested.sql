-- MAIN SELECT WHERE EXTRACT THE YEAR FIELDS + months (which includes the age layer)
select
    json_build_object(
        'years', json_agg(
            json_build_object(
                'year', y.stat_year,
				'spend', spend,
				'cpc', cpc,
				'cpm', cpm,
				'impressions', impressions,
				'clicks', clicks,
				'ctr', ctr,
				'conversion', conversion,
				'cost_per_conversion', cost_per_conversion,
				'conversion_rate', conversion_rate,
				'real_time_conversion', real_time_conversion,
				'real_time_cost_per_conversion', real_time_cost_per_conversion,
				'real_time_conversion_rate', real_time_conversion_rate,
				'result', result,
				'cost_per_result', cost_per_result,
				'result_rate', result_rate,
				'real_time_result', real_time_result,
				'real_time_cost_per_result', real_time_cost_per_result,
				'real_time_result_rate', real_time_result_rate,
				'ctr_trend', ctr_trend,
				'cpm_trend', cpm_trend,
				'cpc_trend', cpc_trend,
				'impressions_trend', impressions_trend,
				'months', months
            )
        )
    ) audience
from (	select 	*,
				((ctr/lag(CTR) OVER ())-1)*100  as CTR_TREND,
				((CPM/lag(CPM) OVER ())-1)*100  as CPM_TREND,
				((CPC/lag(CPC) OVER ())-1)*100  as CPC_TREND,
				((IMPRESSIONS/lag(IMPRESSIONS) OVER ())-1)*100  as IMPRESSIONS_TREND
		from	(	select 
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
					group by stat_year
					order by stat_year
				 ) y
	 	)y


left join 
-- SELECT WHERE EXTRACT THE YEAR FIELDS + the subquery age layer
		(	select 
				m.stat_year,
				json_agg(
					json_build_object(
						'month', m.stat_month,

						--'spend_ant', lag(spend) OVER w,
						'spend', spend,
						'cpc', cpc,
						'cpm', cpm,
						'impressions', impressions,
						'clicks', clicks,
						'ctr', ctr,
						'conversion', conversion,
						'cost_per_conversion', cost_per_conversion,
						'conversion_rate', conversion_rate,
						'real_time_conversion', real_time_conversion,
						'real_time_cost_per_conversion', real_time_cost_per_conversion,
						'real_time_conversion_rate', real_time_conversion_rate,
						'result', result,
						'cost_per_result', cost_per_result,
						'result_rate', result_rate,
						'real_time_result', real_time_result,
						'real_time_cost_per_result', real_time_cost_per_result,
						'real_time_result_rate', real_time_result_rate,
						'CTR_TREND', CTR_TREND,
						'CPM_TREND', CPM_TREND,
						'CPC_TREND', CPC_TREND,
						'IMPRESSIONS_TREND', IMPRESSIONS_TREND,
						'age', age)) months
    		from	(	select 	*,
								((ctr/lag(CTR) OVER ())-1)*100  as CTR_TREND,
								((CPM/lag(CPM) OVER ())-1)*100  as CPM_TREND,
								((CPC/lag(CPC) OVER ())-1)*100  as CPC_TREND,
								((IMPRESSIONS/lag(IMPRESSIONS) OVER ())-1)*100  as IMPRESSIONS_TREND
						from	(	select	date_part('year', ar.stat_time_day) as stat_year,
										date_part('month', ar.stat_time_day) as stat_month_numb,
										trim(TO_CHAR(ar.stat_time_day, 'Mon')) as stat_month,
										trim(TO_CHAR(ar.stat_time_day, 'Mon')) || '-' || date_part('year', ar.stat_time_day) as stat_month_year,
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
									--WINDOW w AS (ORDER BY ar.stat_time_day)
									group by stat_year, stat_month_numb, stat_month, stat_month_year
									order by stat_year, stat_month_numb
								)m
					) m

		 -- SELECT WHERE EXTRACT THE YEAR FIELDS + the subquery age layer
        	left join (	select 	a.stat_year,
								a.stat_month_numb,
								json_agg(
									json_build_object(

										'age', age,
										'spend', spend,
										'cpc', cpc,
										'cpm', cpm,
										'impressions', impressions,
										'clicks', clicks,
										'ctr', ctr,
										'conversion', conversion,
										'cost_per_conversion', cost_per_conversion,
										'conversion_rate', conversion_rate,
										'real_time_conversion', real_time_conversion,
										'real_time_cost_per_conversion', real_time_cost_per_conversion,
										'real_time_conversion_rate', real_time_conversion_rate,
										'result', result,
										'cost_per_result', cost_per_result,
										'result_rate', result_rate,
										'real_time_result', real_time_result,
										'real_time_cost_per_result', real_time_cost_per_result,
										'real_time_result_rate', real_time_result_rate)) age
							from	(	select	date_part('year', ar.stat_time_day) as stat_year,
												date_part('month', ar.stat_time_day) as stat_month_numb,
												trim(TO_CHAR(ar.stat_time_day, 'Mon')) as stat_month,
												trim(TO_CHAR(ar.stat_time_day, 'Mon')) || '-' || date_part('year', ar.stat_time_day) as stat_month_year,
												ar.age,
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
										group by stat_year, stat_month_numb, stat_month, stat_month_year, ar.age
										order by stat_year, stat_month_numb, ar.age 
									) a
							group by a.stat_year, a.stat_month_numb
						) a 
		 	on 	m.stat_year = a.stat_year
			and	m.stat_month_numb = a.stat_month_numb
     		group by m.stat_year
		) mm on y.stat_year = mm.stat_year
