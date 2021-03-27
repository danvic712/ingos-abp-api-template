using IngosAbpTemplate.Domain.Shared.Localization;
using Volo.Abp.Application.Services;

namespace IngosAbpTemplate.Application
{
    /// <summary>
    /// Inherit your application services from this class.
    /// </summary>
    public abstract class IngosAbpTemplateAppService : ApplicationService
    {
        /// <summary>
        /// Base application service
        /// </summary>
        protected IngosAbpTemplateAppService()
        {
            LocalizationResource = typeof(IngosAbpTemplateResource);
        }
    }
}