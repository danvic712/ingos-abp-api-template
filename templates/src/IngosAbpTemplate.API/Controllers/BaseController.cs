//-----------------------------------------------------------------------
// <copyright file= "BaseController.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/3/7 18:59:23
// Modified by:
// Description: Inherit your controllers from this class
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain.Shared.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace IngosAbpTemplate.API.Controllers
{
    /// <summary>
    ///     Base controller
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RemoteServiceErrorResponse))]
    public abstract class BaseController : AbpController
    {
        /// <summary>
        ///     The base controller
        /// </summary>
        protected BaseController()
        {
            LocalizationResource = typeof(IngosAbpTemplateResource);
        }
    }
}