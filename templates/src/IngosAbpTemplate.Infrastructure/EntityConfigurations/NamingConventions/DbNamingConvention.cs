// -----------------------------------------------------------------------
// <copyright file= "DbNamingConvention.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021-05-20 19:55
// Modified by:
// Description: Database naming style enum
// -----------------------------------------------------------------------

namespace IngosAbpTemplate.Infrastructure.EntityConfigurations.NamingConventions
{
    public enum DbNamingConvention
    {
        // https://github.com/abpframework/abp/blob/b390e3cc7b/framework/src/Volo.Abp.Data/Volo/Abp/Data/DbNamingConvention.cs

        Default,
        SnakeCase,
        LowerCase,
        CamelCase,
        UpperCase,
        UpperSnakeCase
    }
}