using ApplicationLogic.Options;
using BingMapsRESTToolkit;
using Microsoft.Extensions.Options;

namespace ApplicationLogic.Services;

public class AutocompleteService : IAutocompleteService
{
    private readonly BingMapsConnectionOptions _bingMapsConnectionOptions;

    public AutocompleteService(IOptions<BingMapsConnectionOptions> bingMapsConnectionOptions)
    {
        _bingMapsConnectionOptions = bingMapsConnectionOptions.Value;
    }

    public async Task<IEnumerable<string>> GetAutocomplete(string query, int limit)
    {
        var request = new AutosuggestRequest()
        {
            Query = query,
            MaxResults = limit,
            IncludeEntityTypes = new List<AutosuggestEntityType>() { AutosuggestEntityType.Address, AutosuggestEntityType.Place},
            UserLocation = new Coordinate(47.60357, -122.32945),
            BingMapsKey = _bingMapsConnectionOptions.BingMapsKey
        };
        
        var response = await request.Execute();

        if (response == null ||
            response.ResourceSets == null ||
            response.ResourceSets.Length <= 0 ||
            response.ResourceSets[0].Resources == null ||
            response.ResourceSets[0].Resources.Length <= 0) return new List<string>();
        var results = (Autosuggest)response.ResourceSets[0].Resources[0];
        return results != null ? results.Value.Select(r => r.Address.FormattedAddress): throw new Exception("Unable to get autocomplete results.");
    }
}