-- Enable uuid-ossp extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create accounts table
CREATE TABLE IF NOT EXISTS accounts (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    created_at TIMESTAMP,
    modified_at TIMESTAMP,
    name VARCHAR(100) NOT NULL,
    account_owner_id UUID,
    status INT NOT NULL
);

-- Create teams table
CREATE TABLE IF NOT EXISTS teams (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    created_at TIMESTAMP,
    modified_at TIMESTAMP,
    account_id UUID NOT NULL,
    status INT NOT NULL,
    name VARCHAR(50) NOT NULL
);

-- Create users table
CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    created_at TIMESTAMP,
    modified_at TIMESTAMP,
    account_id UUID NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(320) NOT NULL,
    job_title VARCHAR(50),
    status INT NOT NULL
);

-- Create team_members table for the many-to-many relationship
CREATE TABLE IF NOT EXISTS team_members (
    user_id UUID,
    team_id UUID,
    PRIMARY KEY (user_id, team_id)
);

-- Add foreign key constraints
ALTER TABLE accounts
    ADD FOREIGN KEY (account_owner_id) REFERENCES users(id) ON DELETE SET NULL;

ALTER TABLE teams
    ADD FOREIGN KEY (account_id) REFERENCES accounts(id) ON DELETE CASCADE;

ALTER TABLE users
    ADD FOREIGN KEY (account_id) REFERENCES accounts(id) ON DELETE CASCADE;

ALTER TABLE team_members
    ADD FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    ADD FOREIGN KEY (team_id) REFERENCES teams(id) ON DELETE CASCADE;
