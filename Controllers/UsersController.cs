using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DataProviders;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserDataProvider _userDataProvider;

        public UsersController(UserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync()
        {
            return await _userDataProvider.GetUsers();
        }

    }
}
