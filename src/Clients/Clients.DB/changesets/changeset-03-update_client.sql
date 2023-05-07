CREATE OR REPLACE PROCEDURE update_client(
    p_id UUID,
    p_first_name VARCHAR(255),
    p_family_name VARCHAR(255),
	p_city VARCHAR(255),
    p_street VARCHAR(255),
    p_building_number VARCHAR(255),
	P_address_lines VARCHAR(255),
    p_primary_phone_number VARCHAR(255),
    p_secondary_phone_number VARCHAR(255),
    p_email_address VARCHAR(255)
) LANGUAGE plpgsql AS '
DECLARE
    v_created_at TIMESTAMP WITH TIME ZONE;
	c_address_lines jsonb = cast(P_address_lines as jsonb);
BEGIN
    -- Start transaction
    BEGIN
    
    -- Check if a row with the given ID exists
    SELECT created_at INTO v_created_at FROM clients WHERE id = p_id FOR UPDATE;
    IF v_created_at IS NULL THEN
        RAISE EXCEPTION ''No client with the provided ID found.'';
    END IF;
    
    -- Update the row
    UPDATE clients SET
        first_name = p_first_name,
        family_name = p_family_name,
		city = p_city,
        street = p_street,
		address_lines = c_address_lines,
        building_number = p_building_number,
        primary_phone_number = p_primary_phone_number,
        secondary_phone_number = p_secondary_phone_number,
        email_address = p_email_address,
        updated_at = NOW()
    WHERE id = p_id;

    -- Commit the transaction
    END;
EXCEPTION
    -- Roll back the transaction on error
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;'