--liquibase formatted sql

--changeset Hamed.Salameh:create_taskitem_procedure
--comment: A stored procedure to insert a task item.

CREATE OR REPLACE PROCEDURE update_taskitem(
    p_tenant_id UUID,
    p_project_id UUID,
    p_id UUID,
    p_name VARCHAR(255),
    p_description VARCHAR(4000),
    p_assigned_to UUID,
    p_assigned_by UUID,
    p_due_date TIMESTAMPTZ,
    p_completed_at TIMESTAMPTZ,
    p_task_item_status INT)  
LANGUAGE plpgsql AS '
    BEGIN
        UPDATE task_items
        SET
            name = p_name,
            description = p_description,
            assigned_to = p_assigned_to,
            assigned_by = p_assigned_by,
            due_date = p_due_date,
            completed_at = p_completed_at,
            task_item_status = p_task_item_status,
            modified_at = NOW()
        WHERE tenant_id = p_tenant_id AND project_id = p_project_id AND id = p_id;
    END;
';

