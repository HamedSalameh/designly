--liquibase formatted sql
--changeset Hamed Salameh:create_taskitem_procedure
--changeset-id: create_taskitem_procedure
--changeset-description: Create stored procedure for inserting a task item
--changeset-rollback-sql: DROP FUNCTION IF EXISTS create_taskitem(UUID, UUID, VARCHAR(255), TEXT, UUID, UUID, TIMESTAMPTZ, TIMESTAMPTZ, TEXT, VARCHAR(255));

/*
Description:
    A stored procedure to insert a task item.
    If the task item already exists, an exception is thrown.
*/

CREATE OR REPLACE PROCEDURE create_taskitem(
    p_tenant_id UUID,
    p_project_id UUID,
    p_name VARCHAR(255),
    p_description TEXT,
    p_assigned_to UUID,
    p_assigned_by UUID,
    p_due_date TIMESTAMPTZ,
    p_completed_at TIMESTAMPTZ,
    p_task_item_status VARCHAR(255),
    OUT p_task_item_id UUID)
LANGUAGE plpgsql AS '
    BEGIN
        INSERT INTO task_items (
            tenant_id,
            project_id,
            name,
            description,
            assigned_to,
            assigned_by,
            due_date,
            completed_at,
            task_item_status,
            created_at,
            modified_at
        ) VALUES (
            p_tenant_id,
            p_project_id,
            p_name,
            p_description,
            p_assigned_to,
            p_assigned_by,
            p_due_date,
            p_completed_at,
            p_task_item_status,
            NOW(),
            NOW()
        )
    RETURNING id INTO p_task_item_id;
    EXCEPTION WHEN unique_violation THEN
        RAISE EXCEPTION ''A task item with the provided ID already exists.'';
    END;
';

