using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Api.Management.Security;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MedPlusUmbraco.Models;
using System.Text.Json;

namespace MedPlusUmbraco.Controllers
{
    [ApiController]
    [Route("api/Search")]
    public class SearchController : ControllerBase
    {
        private readonly IBackOfficeSignInManager _backOfficeSignInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISearchIndex _index;

        public SearchController(
            IBackOfficeSignInManager backOfficeSignInManager, IHttpContextAccessor httpContextAccessor)
        {
            _backOfficeSignInManager = backOfficeSignInManager;
            _httpContextAccessor = httpContextAccessor;

            var client = new SearchClient("XPFCKUSU69", "044db54fb619ea54907990ab820abd2e");
            _index = client.InitIndex("MedPlusIndex");
        }

        [Route("SearchData")]
        [HttpGet]
        public async Task<IActionResult> SearchData(string? searchTerm)
        {
            var searchResponse = await _index.SearchAsync<SearchModel>(new Query(searchTerm));
            var searchResults = new List<Data>();

            if (searchResponse.Hits != null)
            {
                // want to get only banner items in output of search
                foreach (var hit in searchResponse.Hits.Where(u => u.ContentTypeAlias == "bannerItem"))
                {
                    var searchResult = JsonSerializer.Deserialize<Data>(hit.Data.ToString());

                    if (searchResult != null)
                    {
                        if (searchTerm != null && searchTerm != "")
                        {
                            if (searchResult.Heading.Any(h => h.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                                searchResult.Description.Any(d => d.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                            {
                                searchResults.Add(searchResult);
                            }
                        }
                        else
                        {
                            searchResults.Add(searchResult);
                        }
                    }
                }
            }
            return Ok(new { Result = searchResults, Success = true });
        }
    }
}
