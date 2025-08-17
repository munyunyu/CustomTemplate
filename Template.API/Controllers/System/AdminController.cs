using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Template.Business.Interfaces;
using Template.Library.Enums;
using Template.Library.Models;
using Template.Library.ViewsModels.System;

namespace Template.Service.Controllers.System
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPortalService portalService;

        public AdminController(IPortalService portalService)
        {
            this.portalService = portalService;
        }

    }
}
