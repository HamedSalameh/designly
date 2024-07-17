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

export interface Filter {
    field: string;
    operator: string;
    value: string[];
}

export interface SearchProjectsResponse {
    projects: any[];
}
