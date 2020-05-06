using System;

namespace com.libera.api.Controllers
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        internal long ApplicationId
        {
            get
            {
                return Convert.ToInt64(HttpContext.Request.HttpContext.Request?.Headers["ApplicationId"] ?? "0");
            }
            set { }
        }
        internal long UserId
        {
            get
            {
                return 1;
            }
            set { }
        }
    }
}