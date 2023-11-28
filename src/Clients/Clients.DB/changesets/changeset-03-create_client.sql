/*
Description:
    A stored procedure to insert a client.
    If the client already exists, an exception is thrown.
*/

CREATE OR REPLACE PROCEDURE create_client(
    p_tenant_id UUID,
    p_first_name VARCHAR(255),
    p_family_name VARCHAR(255),
    p_city VARCHAR(255),
    p_street VARCHAR(255),
    p_building_number VARCHAR(255),
    p_address_lines text,
    p_primary_phone_number VARCHAR(255),
    p_secondary_phone_number VARCHAR(255),
    p_email_address VARCHAR(255),
	OUT p_client_id UUID
)
LANGUAGE plpgsql AS '
    DECLARE
        c_address_lines jsonb = cast(p_address_lines as jsonb);
    BEGIN
        INSERT INTO clients (
            tenant_id,
            first_name,
            family_name,
            created_at,
            modified_at,
            city,
            street,
            building_number,
            address_lines,
            primary_phone_number,
            secondary_phone_number,
            email_address
        ) VALUES (
            p_tenant_id,
            p_first_name,
            p_family_name,
            NOW(),
            NOW(),
            p_city,
            p_street,
            p_building_number,
            c_address_lines,
            p_primary_phone_number,
            p_secondary_phone_number,
            p_email_address
        )
	RETURNING id INTO p_client_id;
    EXCEPTION WHEN unique_violation THEN
        RAISE EXCEPTION ''A client with the provided ID already exists.'';
    END;
';
