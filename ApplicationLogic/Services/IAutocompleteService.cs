namespace ApplicationLogic.Services;

public interface IAutocompleteService
{
    Task<IEnumerable<string>> GetAutocomplete(string query, int limit);
}