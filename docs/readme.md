# Designly #

## Permissions ##

### Authorization Policies ###

* **Admin**: System admin
* **AccountOwner**: Registered user, owner of tenant
* **AccountMember**: Registered user, member of tenant
* **ProjectLead**: Registered user, Assignee of a project

### Authorization Roles ###


## Clients ##

### Adding new client ###

Permissions: Admin, AccountOwner, AccountMember

### Update client ###

Permissions: Admin, AccountOwner, AccountMember

### Delete client ###

Permissions: Admin, AccountOwner, AccountMember
Use cases:  

* User cannot delete a client with active project.
* User can delete a client with complete project.  
Since then user details are included in project completion report, this should be ok.
* **Consider**: Deleting a client deletes all active and non-active projects


## Projects ##

### Create new project ###

Permissions: Admin, AccountOwner.  
Validations:

* Validate dates
* Validate client exists and not blacklisted
* Validate project lead is a registered user and member of the tenant

### Update project details ###

Permissions: Admin, AccountOwner, AccountMember, ProjectLead.  
Validations: Same as 'Create new project' Validations

### Delete project ###

Permissions: Admin, AccountOwner
Validations:

* N/A

Behavior:

* Deleting a project deletes all tasks under this project
* Does not delete a client

### Complete a project ###

Permissions: Admin, AccountOwner, ProjectLead
Validations:

* Validate the project does not have open tasks

Behavior:

* Completing a project generates a **Completion Report**
* The project status is updated to complete

#### Completion Report ####

Contains the following information and details about the project:

* Project start date, dead line and actual completion date
* client details (full details, not just Id)
* project lead details (full details)
* property details
* etc ...

The report is generated as a signed PDF and can be displayed, shared, downloaded or printed.

The report completion is stored as JSON document for later re-generatiion.
