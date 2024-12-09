import { Filter } from "src/app/shared/models/filter.model";

export interface SearchAccountUsersRequest {
    filters: Filter[];
}

export interface SearchAccountUsersResponse {
    accountUsers: any[];
}