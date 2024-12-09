import { Filter } from "../../shared/models/filter.model";

/*
{
  "filters": [
    {
      "field": "Name",
      "operator": "startswith",
      "value": [
        "New"
      ]
    }
  ]
}
  */
export interface SearchProjectsRequest {
    filters: Filter[];
}

export interface SearchProjectsResponse {
    projects: any[];
}
