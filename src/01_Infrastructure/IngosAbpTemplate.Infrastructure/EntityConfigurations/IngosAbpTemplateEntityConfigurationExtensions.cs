//-----------------------------------------------------------------------
// <copyright file= "IngosAbpTemplateEntityConfigurationExtensions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 21:51:34
// Modified by:
// Description: Entity to table configuration
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain.AggregateModels.BookAggregate;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace IngosAbpTemplate.Infrastructure.EntityConfigurations
{
    public static class IngosAbpTemplateEntityConfigurationExtensions
    {
        public static void ConfigureIngosAbpTemplate(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(BookStoreConsts.DbTablePrefix + "YourEntities", BookStoreConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            builder.Entity<Book>(b =>
            {
                b.ToTable(IngosAbpTemplateConsts.DbTablePrefix + "Books",
                    IngosAbpTemplateConsts.DbSchema);
                b.ConfigureFullAuditedAggregateRoot(); // auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });

            //builder.Entity<BookType>(builder =>
            //{
            //    builder.ToTable(IngosAbpTemplateConsts.DbTablePrefix + "BookType",
            //        IngosAbpTemplateConsts.DbSchema);
            //    builder.ConfigureByConvention();

            //    builder.Property(x => x.Name)
            //        .IsRequired()
            //        .HasMaxLength(128);
            //});
        }
    }
}