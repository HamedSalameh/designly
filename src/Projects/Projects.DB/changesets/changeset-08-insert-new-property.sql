--liquibase formatted sql

--changeset Hamed.Salameh:create_insert_property_procedure
--comment: Create stored procedure to insert data into the property table

CREATE OR REPLACE FUNCTION insert_property(
    p_tenant_id UUID,
    p_name VARCHAR(255),
    p_property_type propertytype DEFAULT 'Unset',
    p_city VARCHAR(255) DEFAULT NULL,
    p_street VARCHAR(255) DEFAULT NULL,
    p_building_number VARCHAR(255) DEFAULT NULL,
    p_address_lines JSONB DEFAULT '{}'::JSONB,
    p_floors JSONB DEFAULT '[]'::JSONB,
    p_total_area DOUBLE PRECISION DEFAULT 0,
    p_created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    p_modified_at TIMESTAMP WITH TIME ZONE DEFAULT now()
)
RETURNS UUID AS $$
DECLARE
    new_property_id UUID;
BEGIN
    -- Insert a new row into the property table
    INSERT INTO public.property (
        tenant_id,
        name,
        property_type,
        city,
        street,
        building_number,
        address_lines,
        floors,
        total_area,
        created_at,
        modified_at
    )
    VALUES (
        p_tenant_id,
        p_name,
        p_property_type,
        p_city,
        p_street,
        p_building_number,
        p_address_lines,
        p_floors,
        p_total_area,
        p_created_at,
        p_modified_at
    )
    RETURNING id INTO new_property_id;

    -- Return the generated ID of the new property
    RETURN new_property_id;
END;
$$ LANGUAGE plpgsql;
