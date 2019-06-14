using HomeApi.WindowsCommander.Api.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.WindowsCommander.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindowStateController : ControllerBase
    {
        [HttpGet("{name}/{state}")]
        public void Post(string name, WindowState state)
        {
            switch (state)
            {
                case WindowState.Close:
                    WindowStateMessenger.Close(name);
                    break;
                case WindowState.Minimize:
                    WindowStateMessenger.Minimize(name);
                    break;
            }
        }
    }
}