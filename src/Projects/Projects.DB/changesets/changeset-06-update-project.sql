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
    p_property JSONB)
LANGUAGE plpgsql AS '
    BEGIN
        UPDATE public.projects
        SET
            name = p_name,
            description = p_description,
            project_lead_id = p_project_lead_id,
            client_id = p_client_id,
            start_date = p_start_date,
            deadline = p_deadline,
            completed_at = p_completed_at,
            status = p_status,
            property = p_property,
            modified_at = NOW()
        WHERE tenant_id = p_tenant_id AND id = p_id;
    END;
';
