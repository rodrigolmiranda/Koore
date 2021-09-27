--drop function get_user_campaigns
--select get_user_campaigns(null);

CREATE OR REPLACE function get_user_campaigns
(
	puser_id		uuid
)
RETURNS table (j json) AS
$$
BEGIN
RETURN QUERY select array_to_json(array_agg(row_to_json(t)))
    from (	
		SELECT 0 as user_id, t.advertiser_id, 'fca4fa8a5ae74321ba910ca7ba1c21706cdab7d4' as access_token, json_agg(campaign_id) as list 
		FROM   campaigns t
		group by advertiser_id
	) t;   
END;
$$ LANGUAGE plpgsql;

