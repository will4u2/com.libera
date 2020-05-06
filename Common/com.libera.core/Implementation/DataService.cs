using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.libera.core
{
    public partial class DataService : BaseService, IDataService
    {
        public DataService(IRepository _repo) : base(_repo)
        {
        }
    }
}
