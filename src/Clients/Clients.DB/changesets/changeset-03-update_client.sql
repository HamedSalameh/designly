/* 
  Description:  
    A stored procedure to update a client.
    if the client does not exist, an exception is thrown.
*/

CREATE OR REPLACE PROCEDURE update_client(
    p_id UUID,
    p_tenant_id UUID,
    p_first_name VARCHAR(255),
    p_family_name VARCHAR(255),
	p_city VARCHAR(255),
    p_street VARCHAR(255),
    p_building_number VARCHAR(255),
	p_address_lines text,
    p_primary_phone_number VARCHAR(255),
    p_secondary_phone_number VARCHAR(255),
    p_email_address VARCHAR(255)
) LANGUAGE plpgsql AS '
DECLARE
    v_created_at TIMESTAMP WITH TIME ZONE;
	c_address_lines jsonb = cast(p_address_lines as jsonb);
BEGIN  
    -- Check if a row with the given ID exists
    SELECT created_at INTO v_created_at FROM clients 
    WHERE id = p_id and tenant_id = p_tenant_id FOR UPDATE;
    IF v_created_at IS NULL THEN
        RAISE EXCEPTION ''No client with the provided ID found.'';
    END IF;
    
    -- Update the row
    UPDATE clients SET
        first_name = p_first_name,
        family_name = p_family_name,
		city = p_city,
		street = p_street,
		building_number = p_building_number,
		address_lines = c_address_lines,
		primary_phone_number = p_primary_phone_number,
		secondary_phone_number = p_secondary_phone_number,
		email_address = p_email_address,
        updated_at = NOW()
    WHERE id = p_id AND tenant_id = p_tenant_id;

END;'