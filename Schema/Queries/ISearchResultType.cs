﻿namespace GraphQLLearning.Schema.Queries
{
    [InterfaceType("SearchResult")]
    public interface ISearchResultType
    {
        Guid Id { get; }
    }
}
