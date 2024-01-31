/*
Description:
    A stored procedure to insert a project.
    If the project already exists, an exception is thrown.
*/


CREATE OR REPLACE PROCEDURE insert_project(
    p_tenant_id UUID,
    p_name VARCHAR(255),
    p_description VARCHAR(1000),
    p_project_lead_id UUID,
    p_client_id UUID,
    p_start_date DATE,
    p_deadline DATE,
    p_completed_at DATE,
    p_status INT,
    OUT p_project_id UUID
)
LANGUAGE plpgsql AS '
    BEGIN
        INSERT INTO projects (
            tenant_id,
            name,
            description,
            project_lead_id,
            client_id,
            start_date,
            deadline,
            completed_at,
            status,
            created_at,
            modified_at
        ) VALUES (
            p_tenant_id,
            p_name,
            p_description,
            p_project_lead_id,
            p_client_id,
            p_start_date,
            p_deadline,
            p_completed_at,
            p_status,
            NOW(),
            NOW()
        )
    RETURNING id INTO p_project_id;
    EXCEPTION WHEN unique_violation THEN
        RAISE EXCEPTION ''A project with the provided ID already exists.'';
    END;
';