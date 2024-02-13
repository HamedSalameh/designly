﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Designly.Base.Exceptions
{
    [Serializable]
    public class DesignlyProblemDetails : ProblemDetails
    {
        // extending the problem details with an extensions that contains a list of key value pairs
        // it must be serializable to json and deserializable from json
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("errors")]
        public List<KeyValuePair<string, string>> Errors { get; set; }

        public DesignlyProblemDetails()
        {
            Errors = [];
        }

        public DesignlyProblemDetails(string title, int statusCode, string? detail = null)
        {
            Title = title;
            Status = statusCode;
            Detail = detail;
            Errors = [];
        }

        public DesignlyProblemDetails(string title, int statusCode, string? detail = null, List<KeyValuePair<string, string>>? errors = null) : this(title, statusCode, detail)
        {
            Errors = errors ?? [];
        }

        public DesignlyProblemDetails(string title, int statusCode, string? detail = null, IEnumerable<Error>? failures = null) : this(title, statusCode, detail)
        {
            Errors = [];

            if (failures != null)
            {
                foreach (var failure in failures)
                {
                    Errors.Add(new KeyValuePair<string, string>(failure.Code, failure.Description));
                }
            }
        }
    }
}
