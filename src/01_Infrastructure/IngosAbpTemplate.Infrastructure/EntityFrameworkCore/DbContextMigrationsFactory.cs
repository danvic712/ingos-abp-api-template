//-----------------------------------------------------------------------
// <copyright file= "DbContextMigrationsFactory.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 22:27:29
// Modified by:
// Description: EF Core migrations command setting like Add-Migration and Update-Database commands
//-----------------------------------------------------------------------

using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IngosAbpTemplate.Infrastructure.EntityFrameworkCore
{
    public class DbContextMigrationsFactory : IDesignTimeDbContextFactory<IngosAbpTemplateDbContext>
    {
        public IngosAbpTemplateDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<IngosAbpTemplateDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new IngosAbpTemplateDbContext(builder.Options);
        }

        public static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../IngosAbpTemplate.HttpApi/"))
                .AddJsonFile("appsettings.json", false, true);

            return builder.Build();
        }
    }
}