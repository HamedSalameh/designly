--liquibase formatted sql

--changeset Hamed.Salameh:update_project_procedure
--comment: A stored procedure to update a project.

CREATE OR REPLACE PROCEDURE update_project(
    p_id UUID,
    p_tenant_id UUID,
    p_name VARCHAR(255),
    p_description VARCHAR(1000),
    p_project_lead_id UUID,
    p_client_id UUID,
    p_start_date DATE,
    p_deadline DATE,
    p_completed_at DATE,
    p_status INT,
    p_property JSON)
LANGUAGE plpgsql AS $$
BEGIN
    UPDATE public.projects
    SET
        name = COALESCE(p_name, name),
        description = COALESCE(p_description, description),
        project_lead_id = COALESCE(p_project_lead_id, project_lead_id),
        client_id = COALESCE(p_client_id, client_id),
        start_date = COALESCE(p_start_date, start_date),
        deadline = COALESCE(p_deadline, deadline),
        completed_at = COALESCE(p_completed_at, completed_at),
        status = COALESCE(p_status, status),
        property = COALESCE(p_property, property),
        modified_at = NOW()
    WHERE tenant_id = p_tenant_id AND id = p_id;
END;
$$;
