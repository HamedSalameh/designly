namespace Projects.Application.Filter
{
    public enum FilterConditionOperator
    {
        None,
        Equals,
        NotEquals,
        In,
        NotIn,
        Contains,
        NotContains,
        GreaterEqualThan,
        LessEqualThan,
        Range,
        LessThan,
        GreaterThan,
        IsNull,
        JsonArrayContains,
        AnyExist,
        NoneExist,
        IsEmpty
    }
}
