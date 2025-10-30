using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Template.Business.Interfaces;
using Template.Library.Constants;
using Template.Library.Enums;
using Template.Library.Models;
using Template.Library.ViewsModels.System;

namespace Template.Service.Controllers.System
{
    //[Authorize(Roles = SystemRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPortalService portalService;

        public AdminController(IPortalService portalService)
        {
            this.portalService = portalService;
        }

        [HttpGet]
        [Route("GetSystemUsers")]
        public async Task<Response<IEnumerable<ViewUserViewModel>>> GetSystemUsers()
        {
            try
            {
                var users = await portalService.Admin.GetSystemUsersAsync();

                return new Response<IEnumerable<ViewUserViewModel>> { Code = Status.Success, Payload = users };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ViewUserViewModel>> { Code = Status.Failed, Message = ex.Message };
            }
        }
    }
}
