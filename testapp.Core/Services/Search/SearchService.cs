using System.Threading.Tasks;
using MvvmCross.Logging;
using Refit;
using testapp.Core.Services.Base;
using testapp.Core.Services.LocationPrompt;

namespace testapp.Core.Services.Search
{
    public class SearchService : ServiceBase, ISearchService
    {
        bool Letting = false;
        private readonly ISearchApi _apiClient;

        public MvvmCross.Commands.IMvxAsyncCommand<SearchPropertyResult> ToLetAsyncCommand { get; private set; }
        public MvvmCross.Commands.IMvxAsyncCommand<SearchPropertyResult> ToSellAsyncCommand { get; private set; }
        
        private async Task ToLetAsync(SearchPropertyResult searchPropertyResult)
        {
            Letting = true;
        }

        private async Task ToSellAsync(SearchPropertyResult searchPropertyResult)
        {
            Letting = false;
        }

        public SearchService(IMvxLog log) : base(log, "https://search.purplebricks.co.uk")
        {
            _apiClient = RestService.For<ISearchApi>(httpClient);
        }

        public async Task<SearchResult> FindProperties(LocationPromptResult locationPrompt, bool toLet = false, int pageNumber = 1, int pageSize = 10)
        {
            return await _apiClient.SearchProperties(
                locationPrompt.Location,
                locationPrompt.Latitude,
                locationPrompt.Longitude,
                pageNumber,
                pageSize,
                Letting);
        }
    }
}
