using IngosAbpTemplate.Domain.Shared.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace IngosAbpTemplate.Application.Contracts.Permissions
{
    public class IngosAbpTemplatePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(IngosAbpTemplatePermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(IngosAbpTemplatePermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IngosAbpTemplateResource>(name);
        }
    }
}