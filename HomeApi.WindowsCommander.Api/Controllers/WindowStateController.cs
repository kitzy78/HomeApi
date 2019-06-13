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
                    User32.IssueSystemCommand(name, SysCommand.SC_CLOSE);
                    break;
                case WindowState.Minimize:
                    User32.IssueSystemCommand(name, SysCommand.SC_MINIMIZE);
                    break;
            }
        }
    }
}