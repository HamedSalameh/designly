﻿--liquibase formatted sql

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

CREATE TABLE public.properties (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID NOT NULL,
    name VARCHAR(255),
    property_type INT NOT NULL DEFAULT 0,
    address JSONB,
    floors JSONB,
    number_of_floors INT GENERATED ALWAYS AS (jsonb_array_length(floors)) STORED,  -- Calculate from floors
    total_area DOUBLE PRECISION NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    modified_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);

-- Index for faster lookup by tenant_id
CREATE INDEX idx_properties_tenant_id ON public.properties (tenant_id);
