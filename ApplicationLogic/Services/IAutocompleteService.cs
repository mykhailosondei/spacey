using ApplicationCommon.GeospatialUtilities;

namespace ApplicationLogic.Services;

public interface IAutocompleteService
{
    Task<IEnumerable<string>> GetAutocomplete(string query, int limit);

    Task<Coordinates> GetGeocode(string query);
}