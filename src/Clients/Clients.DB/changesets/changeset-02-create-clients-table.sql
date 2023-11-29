-- The uuid_generate_v4() function assumes that the uuid-ossp, hence we make sure to enable it before table creation
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS clients (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  tenant_id UUID NOT NULL,
  first_name VARCHAR(255) NOT NULL,
  family_name VARCHAR(255) NOT NULL,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  modified_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  city VARCHAR(255) NOT NULL,
  street VARCHAR(255) NOT NULL,
  building_number VARCHAR(255) NOT NULL,
  address_lines JSONB,
  primary_phone_number VARCHAR(255) NOT NULL,
  secondary_phone_number VARCHAR(255),
  email_address VARCHAR(255)
)