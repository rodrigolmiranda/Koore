 
--select * From campaigns
--drop procedure insert_update_campaign
--call insert_update_campaign(1, 'ggggg', null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)
CREATE OR REPLACE PROCEDURE public.insert_update_campaign(
	pcampaign_id	bigint,
	pcampaign_name	varchar,
	pcampaign_type	varchar,
	padvertiser_id	bigint,
	pbudget	double precision,
	pbudget_mode	varchar,
	pstatus	varchar,
	popt_status	varchar,
	pobjective	varchar,
	pobjective_type	varchar,
	pbudget_optimize_switch	int,
	pbid_type	int,
	poptimize_goal	varchar,
	psplit_test_variable	varchar,
	pis_new_structure bool,
	pcreate_time	timestamp,
	pmodify_time	timestamp,
	INOUT newLines int)
LANGUAGE plpgsql 
AS $$
begin

if not exists(select 1 from campaigns where campaign_id = pcampaign_id) then

	--delete from campaigns
	insert into campaigns (campaign_id,
		campaign_name,
		campaign_type,
		advertiser_id,
		budget,
		budget_mode,
		status,
		opt_status,
		objective,
		objective_type,
		budget_optimize_switch,
		bid_type,
		optimize_goal,
		split_test_variable,
		is_new_structure,
		create_time,
		modify_time
						  )
	values (
	pcampaign_id,
		pcampaign_name,
		pcampaign_type,
		padvertiser_id,
		pbudget,
		pbudget_mode,
		pstatus,
		popt_status,
		pobjective,
		pobjective_type,
		pbudget_optimize_switch,
		pbid_type,
		poptimize_goal,
		psplit_test_variable,
		pis_new_structure,
		pcreate_time,
		pmodify_time
	);
	get diagnostics newLines = row_count;
else
		update	campaigns
		set 	campaign_name = pcampaign_name,
				campaign_type = pcampaign_type,
				advertiser_id = padvertiser_id,
				budget = pbudget,
				budget_mode = pbudget_mode,
				status = pstatus,
				opt_status = popt_status,
				objective = pobjective,
				objective_type = pobjective_type,
				budget_optimize_switch = pbudget_optimize_switch,
				bid_type = pbid_type,
				optimize_goal = poptimize_goal,
				split_test_variable = psplit_test_variable,
				is_new_structure = pis_new_structure,
				create_time = pcreate_time,
				modify_time = pmodify_time
		where	campaign_id = pcampaign_id;
	get diagnostics newLines = row_count;

end if;


end
$$;
