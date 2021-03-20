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
using Volo.Abp.AspNetCore.Mvc;

namespace IngosAbpTemplate.HttpApi.Host.Controllers
{
    /// <summary>
    ///     Base controller
    /// </summary>
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