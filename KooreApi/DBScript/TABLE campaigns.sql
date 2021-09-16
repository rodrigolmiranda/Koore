--drop table campaigns
CREATE TABLE public.campaigns
(
	campaign_id	bigint primary key,
	campaign_name	varchar,
	campaign_type	varchar,
	advertiser_id	bigint,
	budget	double precision,
	budget_mode	varchar,
	status	varchar,
	opt_status	varchar,
	objective	varchar,
	objective_type	varchar,
	budget_optimize_switch	int,
	bid_type	int,
	optimize_goal	varchar,
	split_test_variable	varchar,
	is_new_structure bool,
	create_time	timestamp,
	modify_time	timestamp
)