--liquibase formatted sql

--changeset Hamed.Salameh:create_update_project_procedure
--comment: Create stored procedure to update data in the projects table

CREATE OR REPLACE PROCEDURE update_project(
    p_id UUID,
    p_tenant_id UUID,
    p_name VARCHAR(255),
    p_project_lead_id UUID,
    p_client_id UUID,
    p_status INT,
    p_modified_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    p_description VARCHAR(1000) DEFAULT NULL,
    p_start_date DATE DEFAULT NULL,
    p_deadline DATE DEFAULT NULL,
    p_completed_at DATE DEFAULT NULL,
    p_property_id UUID DEFAULT NULL
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Update the projects table with the new data
    UPDATE public.projects
    SET
        name = p_name,
        description = COALESCE(p_description, description),
        project_lead_id = p_project_lead_id,
        client_id = p_client_id,
        start_date = COALESCE(p_start_date, start_date),
        deadline = COALESCE(p_deadline, deadline),
        completed_at = COALESCE(p_completed_at, completed_at),
        status = p_status,
        property_id = COALESCE(p_property_id, property_id),
        modified_at = p_modified_at
    WHERE id = p_id AND tenant_id = p_tenant_id;
END;
$$;
