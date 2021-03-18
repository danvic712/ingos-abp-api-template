using IngosAbpTemplate.Domain.Shared.Localization;
using Volo.Abp.Application.Services;

namespace IngosAbpTemplate.Application
{
    /* Inherit your application services from this class.
     */

    public abstract class IngosAbpTemplateAppService : ApplicationService
    {
        protected IngosAbpTemplateAppService()
        {
            LocalizationResource = typeof(IngosAbpTemplateResource);
        }
    }
}