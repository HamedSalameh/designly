--changeset Hamed.Salameh:add_property_to_project
--comment: A stored procedure to update the property JSON object for a project with tenant validation.

CREATE OR REPLACE PROCEDURE add_property_to_project(
    p_project_id UUID,
    p_tenant_id UUID,
    p_property JSONB
)
LANGUAGE plpgsql AS '
    BEGIN
        UPDATE projects
        SET property = p_property,
            modified_at = NOW()
        WHERE id = p_project_id
        AND tenant_id = p_tenant_id;
        
        -- Check if the project with the matching tenant_id exists, raise an exception if not
        IF NOT FOUND THEN
            RAISE EXCEPTION ''Project with ID % for Tenant % does not exist'', p_project_id, p_tenant_id;
        END IF;
    END;
';
