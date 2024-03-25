# Feature Requirement: Tasks Search

## Description
This feature describes the required capabilities and behaviuor of task search.

## Requirements  
Searching for tasks must provide the following capabilities:  
1. Get Single Task By Id

2. Search Tasks by Keyword

    - The system should allow users to search for tasks using keywords.
    - The search should return a list of tasks that match the provided keywords.
    - The search should be case-insensitive.

3. Filter Tasks by Assignment, Due Date, Project, Status, and Completion Date

    - The system should allow users to filter tasks based on assignment, due date, project, status, and completion date.
    - Users should be able to specify one or more of these criteria to narrow down their search results.
    - The filter should return a list of tasks that match the provided criteria.
    - The filter should be case-insensitive.

4. The client search request DTO will provide the filtering criteria.

5. Search Request DTO

    - The DTO (Data Transfer Object) is used to provide filtering criteria.
    - It contains a list of FilterConditions.
        * Each FilterCondition includes:
        FieldName, Operator, Value
    - The operator between FilterConditions is "AND".
    - The operator inside FilterCondition values is "OR".
