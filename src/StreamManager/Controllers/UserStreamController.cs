using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StreamManager.Repositories;

namespace StreamManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserStreamController : ControllerBase
    {
        private readonly IUserStreamRepository _userStreamRepository;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UserStreamController> _logger;

        public UserStreamController(IUserStreamRepository userStreamRepository,
                                    IOptions<AppSettings> appSettings,
                                    ILogger<UserStreamController> logger)
        {
            _userStreamRepository = userStreamRepository;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [HttpGet("count/{username}")]
        public async Task<IActionResult> Count(string username)
        {
            var streamCount = await _userStreamRepository.GetUserStreamCount(username);

            return Ok(streamCount);
        }

        [HttpGet("start/{username}")]
        public async Task<IActionResult> Start(string username)
        {
            var streamCount = await _userStreamRepository.GetUserStreamCount(username);

            if(streamCount >= _appSettings.MaximumConcurrentUserStreams)
                return Forbid();

            await _userStreamRepository.IncrementUserStreamCount(username);

            return Ok();
        }
    }
}
