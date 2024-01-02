import axios, {Axios, AxiosHeaders, AxiosRequestConfig, AxiosRequestHeaders, AxiosResponse} from "axios";

export class HttpCustomClient{
    private headers: AxiosRequestHeaders;
    private baseUrl: string;
    
    constructor(){
        this.headers = {} as AxiosRequestHeaders;
        this.headers["Content-Type"] = "application/json";
        this.headers["Accept"] = "application/json";
        this.headers["Access-Control-Allow-Origin"] = "*";
        this.baseUrl = "https://localhost:7171/api";
    }
    
    public SetBearerToken(token: string): void {
        this.headers["Authorization"] = `Bearer ${token}`;
    }
    
    public RemoveBearerToken(): void {
        delete this.headers["Authorization"];
    }
    
    public setHeader(key: string, value: string): void {
        this.headers[key] = value;
    }
    
    public getHeader(key: string): string | undefined {
        return this.headers[key];
    }
    
    public async Get<T>(url: string, typeInfo? : new ()=>T): Promise<T> {
        const response : AxiosResponse<T, any> = await axios.get<T>(this.baseUrl + url, {headers: this.headers});
        console.log(`HttpCustomClient.Get<${typeInfo?.name}> status:`);
        console.log(response.status);
        const data = response.data;
        return data;
    }
    
    public async Post<T>(url: string, body: any = {}, typeInfo? : new ()=>T): Promise<T> {
        const response : AxiosResponse<T, any> = await axios.post<T>(this.baseUrl + url, body, {headers: this.headers});
        console.log(`HttpCustomClient.Post<${typeInfo?.name}> status:`);
        console.log(response.status);
        const data = response.data;
        return data;
    }
    
    public async Put<T>(url: string, body: any = {}, typeInfo? : new ()=>T): Promise<T> {
        const response : AxiosResponse<T, any> = await axios.put<T>(this.baseUrl + url, body, {headers: this.headers});
        console.log(`HttpCustomClient.Put<${typeInfo?.name}> status:`);
        console.log(response.status);
        const data = response.data;
        return data;
    }
    
    public async Delete<T>(url: string, typeInfo? : new ()=>T): Promise<T> {
        const response : AxiosResponse<T, any> = await axios.delete<T>(this.baseUrl + url, {headers: this.headers});
        console.log(`HttpCustomClient.Delete<${typeInfo?.name}> status:`);
        console.log(response.status);
        const data = response.data;
        return data;
    }
}