using MMSC.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MMSC.Controllers
{
    public class ServiceStatusController : ApiController
    {
        public string Get()
        {
            return AppSettings.Service.Status;
            
        }
    }
}
