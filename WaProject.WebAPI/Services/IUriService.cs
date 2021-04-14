using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaProject.WebAPI.Filters;

namespace WaProject.WebAPI.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
