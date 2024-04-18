--liquibase formatted sql

--changeset Hamed.Salameh:create_taskitem_procedure
--comment: A stored procedure to insert a task item.

CREATE OR REPLACE PROCEDURE update_basicproject(
    p_id UUID,
    p_tenant_id UUID,
    p_name VARCHAR(255),
    p_description VARCHAR(4000),
    p_project_lead_id UUID,
    p_client_id UUID,
    p_start_date TIMESTAMPTZ,
    p_deadline TIMESTAMPTZ,
    p_completed_at TIMESTAMPTZ,
    p_status INT)  
LANGUAGE plpgsql AS '
    BEGIN
        UPDATE projects
        SET
            name = p_name,
            description = p_description,
            project_lead_id = p_project_lead_id,
            client_id = p_client_id,
            start_date = p_start_date,
            deadline = p_deadline,
            completed_at = p_completed_at,
            status = p_status,
            modified_at = NOW()
        WHERE tenant_id = p_tenant_id AND id = p_id;
    END;
';

