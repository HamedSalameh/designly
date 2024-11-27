--liquibase formatted sql

--changeset Hamed.Salameh:create_property_procedure_v2
--comment: A stored procedure to insert a new property only if it does not exist

CREATE OR REPLACE PROCEDURE create_property(
    OUT p_property_id UUID,
    p_tenant_id UUID,
    p_name VARCHAR(255),
    p_property_type INT DEFAULT 0, -- Assuming 0 represents 'Unset'
    p_address JSONB DEFAULT '{}'::JSONB, -- Changed to JSONB
    p_floors JSONB DEFAULT '[]'::JSONB,  -- Changed to JSONB
    p_total_area DOUBLE PRECISION DEFAULT 0
)
LANGUAGE plpgsql AS '
    BEGIN
        -- Check if a property with the same tenant_id and name already exists
        SELECT id
        INTO p_property_id
        FROM public.properties
        WHERE tenant_id = p_tenant_id AND name = p_name;

        IF p_property_id IS NOT NULL THEN
            -- Property already exists, return the existing property ID
            RETURN;
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
