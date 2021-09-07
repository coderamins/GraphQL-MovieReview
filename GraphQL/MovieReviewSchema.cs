
using System;
using GraphQL.Types;

public class MovieReviewSchema : Schema
{
    public MovieReviewSchema(QueryObject query, MutationObject mutation, IServiceProvider sp) : base(sp)
    {
        Query = query;
        Mutation = mutation;
    }
}