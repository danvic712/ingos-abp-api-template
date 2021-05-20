// -----------------------------------------------------------------------
// <copyright file= "DbNamingConventionRewriterExtensions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021-05-20 19:47
// Modified by:
// Description: Database naming conversion extension methods 
// -----------------------------------------------------------------------

using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IngosAbpTemplate.Infrastructure.EntityConfigurations.NamingConventions
{
    public static class DbNamingConventionRewriterExtensions
    {
        // https://github.com/abpframework/abp/blob/b390e3cc7b79a6216c1787f5f7bc598aae35eb14/framework/src/Volo.Abp.EntityFrameworkCore/Volo/Abp/EntityFrameworkCore/DbNamingConventionRewriterExtensions.cs

        public static void NamingConventionsRewriteName(this DbContextOptionsBuilder optionsBuilder,
            DbNamingConvention namingConvention = DbNamingConvention.Default,
            CultureInfo culture = null)
        {
            
            
            switch (namingConvention)
            {
                case DbNamingConvention.Default:
                    break;
                case DbNamingConvention.SnakeCase:
                    optionsBuilder.UseSnakeCaseNamingConvention(culture);
                    break;
                case DbNamingConvention.LowerCase:
                    optionsBuilder.UseLowerCaseNamingConvention(culture);
                    break;
                case DbNamingConvention.UpperCase:
                    optionsBuilder.UseUpperCaseNamingConvention(culture);
                    break;
                case DbNamingConvention.UpperSnakeCase:
                    optionsBuilder.UseUpperSnakeCaseNamingConvention(culture);
                    break;
                case DbNamingConvention.CamelCase:
                    optionsBuilder.UseCamelCaseNamingConvention(culture);
                    break;
                default:
                    break;
            }
        }

        public static void NamingConventionsRewriteName(this ModelBuilder modelBuilder,
            DbNamingConvention namingConvention = DbNamingConvention.Default,
            CultureInfo culture = null)
        {
            if (namingConvention == DbNamingConvention.Default) return;

            INameRewriter nameRewriter = null;
            culture ??= CultureInfo.InvariantCulture;
            switch (namingConvention)
            {
                case DbNamingConvention.Default:
                    break;
                case DbNamingConvention.SnakeCase:
                    nameRewriter = new SnakeCaseNameRewriter(culture);
                    break;
                case DbNamingConvention.LowerCase:
                    nameRewriter = new LowerCaseNameRewriter(culture);
                    break;
                case DbNamingConvention.UpperCase:
                    nameRewriter = new UpperCaseNameRewriter(culture);
                    break;
                case DbNamingConvention.UpperSnakeCase:
                    nameRewriter = new UpperSnakeCaseNameRewriter(culture);
                    break;
                case DbNamingConvention.CamelCase:
                    nameRewriter = new CamelCaseNameRewriter(culture);
                    break;
                default:
                    break;
            }

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(nameRewriter.RewriteName(entity.GetTableName()));

                foreach (var property in entity.GetProperties())
                {
                    // property.SetColumnName(nameRewriter.RewriteName(property.GetColumnName()));
                    var columnName = property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName(), null));
                    property.SetColumnName(nameRewriter.RewriteName(columnName));
                }

                foreach (var key in entity.GetKeys()) key.SetName(nameRewriter.RewriteName(key.GetName()));

                foreach (var key in entity.GetForeignKeys())
                    key.SetConstraintName(nameRewriter.RewriteName(key.GetConstraintName()));

                foreach (var index in entity.GetIndexes())
                    // index.SetName(nameRewriter.RewriteName(index.GetName());
                    index.SetDatabaseName(nameRewriter.RewriteName(index.GetDatabaseName()));
            }
        }
    }
}