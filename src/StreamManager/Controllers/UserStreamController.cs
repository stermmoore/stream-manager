using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StreamManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserStreamController : ControllerBase
    {
        private readonly ILogger<UserStreamController> _logger;

        public UserStreamController(ILogger<UserStreamController> logger)
        {
            _logger = logger;
        }

    }
}
