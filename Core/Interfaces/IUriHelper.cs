using System;
using System.Threading.Tasks;
using DAL.Models.Internal;

namespace Core.Interfaces
{
    public interface IUriHelper
    {
        public Task<Uri> GetPageUriAsync(PaginationFilter filter, string route);
    }
}