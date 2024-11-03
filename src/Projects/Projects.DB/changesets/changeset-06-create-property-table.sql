--liquibase formatted sql

--changeset Hamed.Salameh:create_property_table
--comment: Create the property table

-- Enable uuid-ossp extension for UUID generation
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Define the PropertyType enum to match the C# enum values
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'propertytype') THEN
        CREATE TYPE propertytype AS ENUM ('Residential', 'Commercial', 'Industrial', 'MixedUse', 'Other', 'Unset');
    END IF;
END$$;

CREATE TABLE public.property (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID NOT NULL,
    name VARCHAR(255),
    property_type propertytype NOT NULL DEFAULT 'Unset',
    city VARCHAR(255) NOT NULL,
    street VARCHAR(255) NOT NULL,
    building_number VARCHAR(255) NOT NULL,
    address_lines JSONB,
    floors JSONB,
    number_of_floors INT GENERATED ALWAYS AS (jsonb_array_length(floors)) STORED,  -- Calculate from floors
    total_area DOUBLE PRECISION NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    modified_at TIMESTAMP WITH TIME ZONE DEFAULT now() ON UPDATE CURRENT_TIMESTAMP
);

-- Index for faster lookup by tenant_id
CREATE INDEX idx_property_tenant_id ON public.property (tenant_id);
