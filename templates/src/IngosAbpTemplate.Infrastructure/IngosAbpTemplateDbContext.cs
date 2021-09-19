using IngosAbpTemplate.Infrastructure.EntityConfigurations;
using IngosAbpTemplate.Infrastructure.EntityConfigurations.NamingConventions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace IngosAbpTemplate.Infrastructure
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See IngosAbpTemplateMigrationsDbContext for migrations.
     */

    [ConnectionStringName("Default")]
    public class IngosAbpTemplateDbContext : AbpDbContext<IngosAbpTemplateDbContext>
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside IngosAbpTemplateDbContextModelCreatingExtensions.ConfigureIngosAbpTemplate
         */

        public IngosAbpTemplateDbContext(DbContextOptions<IngosAbpTemplateDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.NamingConventionsRewriteName(DbNamingConvention.SnakeCase);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */
            builder.ConfigureAbpEntities();

            /* Configure your own tables/entities inside the ConfigureIngosAbpTemplate method */
            builder.ConfigureIngosAbpTemplate();
            
            // Due to https://github.com/abpframework/abp/pull/7849 has not release, adopt the temporary method
            builder.NamingConventionsRewriteName(DbNamingConvention.SnakeCase);
        }
    }
}