--liquibase formatted sql

--changeset Hamed.Salameh:create_property_procedure_v2
--comment: A stored procedure to insert a new property only if it does not exist

CREATE OR REPLACE PROCEDURE create_or_update_property(
    OUT p_property_id UUID,
    p_id UUID,
    p_tenant_id UUID,
    p_name VARCHAR(255),
    p_property_type INT DEFAULT 0, -- Assuming 0 represents 'Unset'
    p_address JSONB DEFAULT '{}'::JSONB, -- Changed to JSONB
    p_floors JSONB DEFAULT '[]'::JSONB,  -- Changed to JSONB
    p_total_area DOUBLE PRECISION DEFAULT 0
)
LANGUAGE plpgsql AS '
    BEGIN
        -- Check if a property with the same tenant_id and id already exists
        SELECT id
        INTO p_property_id
        FROM public.properties
        WHERE tenant_id = p_tenant_id AND id = p_id;

        IF p_property_id IS NOT NULL THEN
            -- Property exists, update it
            UPDATE public.properties
            SET
                property_type = p_property_type,
                address = p_address,
                floors = p_floors,
                total_area = p_total_area,
                modified_at = NOW()
            WHERE id = p_property_id AND tenant_id = p_tenant_id
            RETURNING id INTO p_property_id;
        ELSE
            -- Insert a new property (created_at will be automatically set by the database)
            INSERT INTO public.properties (
                tenant_id,
                name,
                property_type,
                address,
                floors,
                total_area,
                created_at,
                modified_at
            )
            VALUES (
                p_tenant_id,
                p_name,
                p_property_type,
                p_address,  -- JSONB value
                p_floors,   -- JSONB value
                p_total_area,
                NOW(),
                NOW()
            )
            RETURNING id INTO p_property_id;
        END IF;
    EXCEPTION WHEN unique_violation THEN
        RAISE EXCEPTION ''A property with the provided tenant_id and name already exists.''; 
    END;
';
