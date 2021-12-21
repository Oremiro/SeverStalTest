using System;
using System.Threading.Tasks;
using Core.Interfaces;
using DAL.Models.Internal;
using Microsoft.AspNetCore.WebUtilities;

namespace Core.Helpers
{
    public class UriHelper: IUriHelper
    {
        private readonly string _baseUri;
        public UriHelper(string baseUri)
        {
            _baseUri = baseUri;
        }
        public async Task<Uri> GetPageUriAsync(PaginationFilter filter, string route)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (route == null) throw new ArgumentNullException(nameof(route));
            
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            if (_enpointUri == null) throw new ArgumentNullException(nameof(_enpointUri));
            
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}