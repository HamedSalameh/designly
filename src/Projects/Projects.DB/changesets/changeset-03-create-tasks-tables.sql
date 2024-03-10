--liquibase formatted sql

--changeset Hamed.Salameh:create_tasks_tables
--comment: Create the tasks tables

CREATE TABLE TaskItem (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tenant_id UUID NOT NULL,
    project_id UUID NOT NULL,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    assigned_to UUID,
    assigned_by UUID,
    due_date TIMESTAMP WITH TIME ZONE,
    completed_at TIMESTAMP WITH TIME ZONE,
    task_item_status INT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    modified_at TIMESTAMP WITH TIME ZONE DEFAULT now(),

    -- Foriegn Key Constraints --
    CONSTRAINT FK_TaskItems_Projects FOREIGN KEY(project_id) REFERENCES Projects(id)
);
