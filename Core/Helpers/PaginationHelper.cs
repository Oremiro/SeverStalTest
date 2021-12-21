using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using DAL.Models.Internal;
using DAL.Models.Responses;

namespace Core.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<PagedResponse<List<T>>> CreatePagedReponseAsync<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriHelper uriService, string route)
        {
            if (pagedData == null) throw new ArgumentNullException(nameof(pagedData));
            if (validFilter == null) throw new ArgumentNullException(nameof(validFilter));
            if (uriService == null) throw new ArgumentNullException(nameof(uriService));
            if (route == null) throw new ArgumentNullException(nameof(route));
            
            var respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? await uriService.GetPageUriAsync(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                    : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? await uriService.GetPageUriAsync(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                    : null;
            respose.FirstPage = await uriService.GetPageUriAsync(new PaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = await uriService.GetPageUriAsync(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}