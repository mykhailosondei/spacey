import {HttpCustomClient} from "./HttpCustomClient";

export class AutocompleteService {
    http: HttpCustomClient;
    baseUrl: string;
    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/autocomplete";
    }
    
    private static instance: AutocompleteService;
    public static getInstance(): AutocompleteService {
        if (!AutocompleteService.instance) {
            AutocompleteService.instance = new AutocompleteService();
        }
        return AutocompleteService.instance;
    }
    
    async getAutocompleteData(query: string, limit: number) {
        return await this.http.Get<string[]>(`${this.baseUrl}/?query=${query}&limit=${limit}`);
    }
}