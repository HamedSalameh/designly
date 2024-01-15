-- The uuid_generate_v4() function assumes that the uuid-ossp, hence we make sure to enable it before table creation
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create accounts table
CREATE TABLE IF NOT EXISTS accounts (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    created_at TIMESTAMP,
    modified_at TIMESTAMP,
    name VARCHAR(100) NOT NULL,
    account_owner UUID NOT NULL,
    status INT NOT NULL,
    FOREIGN KEY (account_owner) REFERENCES users(id) ON DELETE CASCADE
);

-- Create teams table
CREATE TABLE IF NOT EXISTS teams (
    id UUID PRIMARY KEY,
    created_at TIMESTAMP,
    modified_at TIMESTAMP,
    name VARCHAR(50) NOT NULL,
    member_of UUID NOT NULL,
    account_id UUID NOT NULL,
    status INT NOT NULL,
    FOREIGN KEY (member_of) REFERENCES teams(id) ON DELETE CASCADE,
    FOREIGN KEY (account_id) REFERENCES accounts(id) ON DELETE CASCADE
);

-- Create users table
CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY,
    created_at TIMESTAMP,
    modified_at TIMESTAMP,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(320) NOT NULL,
    job_title VARCHAR(50),
    member_of UUID NOT NULL,
    status INT NOT NULL,
    FOREIGN KEY (member_of) REFERENCES teams(id) ON DELETE CASCADE
);