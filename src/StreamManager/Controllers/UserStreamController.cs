using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StreamManager.Repositories;

namespace StreamManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserStreamController : ControllerBase
    {
        private readonly IUserStreamRepository _userStreamRepository;
        private readonly ILogger<UserStreamController> _logger;

        public UserStreamController(IUserStreamRepository userStreamRepository,
                                    ILogger<UserStreamController> logger)
        {
            _userStreamRepository = userStreamRepository;
            _logger = logger;
        }

        [HttpGet("count/{username}")]
        public async Task<IActionResult> Count(string username)
        {
            var streamCount = await _userStreamRepository.GetUserStreamCount(username);

            return Ok(streamCount);
        }
    }
}
