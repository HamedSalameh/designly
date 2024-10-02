--liquibase formatted sql

--changeset Hamed.Salameh:create_projects_table
--comment: Create the projects table

-- The uuid_generate_v4() function assumes that the uuid-ossp, hence we make sure to enable it before table creation
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE public.projects (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID NOT NULL,
    name VARCHAR(255) NOT NULL,
    description VARCHAR(1000),
    project_lead_id UUID NOT NULL,
    client_id UUID NOT NULL,
    start_date DATE,
    deadline DATE,
    completed_at DATE,
    status INT NOT NULL,
    property_id UUID NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    modified_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    -- Constraints --
    -- deadline cannot be before start_date
    CONSTRAINT deadline_after_start_date CHECK (deadline >= start_date),
    -- completed_at cannot be before start_date
    CONSTRAINT completed_at_after_start_date CHECK (completed_at >= start_date)
);
    
-- Create PropertyType enum (assuming PropertyType is an enum in the database)
CREATE TYPE property_type AS ENUM ('Residential', 'Commercial', 'Industrial', 'MixedUse');

-- Create Property table
CREATE TABLE public.properties (
	id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
	tenant_id UUID NOT NULL,
	name VARCHAR(255) NOT NULL,
	property_type property_type NOT NULL,
    city VARCHAR(255) NOT NULL,
    street VARCHAR(255) NOT NULL,
    building_number VARCHAR(255) NOT NULL,
    address_lines JSONB,
    floors JSONB,
    total_area DOUBLE PRECISION NOT NULL,
	created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
	modified_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);
