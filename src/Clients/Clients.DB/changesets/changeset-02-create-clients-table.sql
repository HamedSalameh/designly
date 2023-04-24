CREATE TABLE IF NOT EXISTS clients (
  id UUID PRIMARY KEY,
  first_name VARCHAR(255) NOT NULL,
  family_name VARCHAR(255) NOT NULL,
  city VARCHAR(255) NOT NULL,
  street VARCHAR(255) NOT NULL,
  building_number VARCHAR(255) NOT NULL,
  address_lines JSONB,
  primary_phone_number VARCHAR(255) NOT NULL,
  secondary_phone_number VARCHAR(255),
  email_address VARCHAR(255),
  created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT now()
)