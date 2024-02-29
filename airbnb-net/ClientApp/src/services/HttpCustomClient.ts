import axios, {Axios, AxiosHeaders, AxiosRequestConfig, AxiosRequestHeaders, AxiosResponse, CancelToken} from "axios";

export interface HttpResponse<T> {
    data: T;
    status: number;
    statusText: string;
}

export class HttpCustomClient{
    private readonly headers: AxiosRequestHeaders;
    private readonly baseUrl: string;
    
    private constructor(){
        this.headers = {} as AxiosRequestHeaders;
        this.headers["Content-Type"] = "application/json";
        this.headers["Accept"] = "application/json";
        this.headers["Access-Control-Allow-Origin"] = "*";
        this.headers["Authorization"] = `Bearer ${localStorage.getItem("token")}`;
        this.baseUrl = "https://localhost:7171/api";
    }
    
    static instance: HttpCustomClient;
    
    public static getInstance(): HttpCustomClient {
        if (!HttpCustomClient.instance) {
            HttpCustomClient.instance = new HttpCustomClient();
        }
        return HttpCustomClient.instance;
    } 
    
    public SetBearerToken(token: string): void {
        localStorage.setItem("token", token);
        this.headers["Authorization"] = `Bearer ${localStorage.getItem("token")}`;
    }
    
    public RemoveBearerToken(): void {
        localStorage.removeItem("token");
        delete this.headers["Authorization"];
    }
    
    public setHeader(key: string, value: string): void {
        this.headers[key] = value;
    }
    
    public getHeader(key: string): string | undefined {
        return this.headers[key];
    }
    
    transformResponse<T>(response: AxiosResponse<T, any>): HttpResponse<T> {
        return {
            data: response.data,
            status: response.status,
            statusText: response.statusText
        };
    }
    
    public async Get<T>(url: string, config:AxiosRequestConfig = {}): Promise<HttpResponse<T>> {
        config.headers = {...this.headers, ...config.headers};
        const response : AxiosResponse<T, any> = await axios.get<T>(this.baseUrl + url, {validateStatus: ()=>true, ...config});
        console.log(`HttpCustomClient.Get<${url}> status:`);
        console.log(response.status);
        const responseResult = this.transformResponse<T>(response);
        return responseResult;
    }
    
    public async Post<T>(url: string, body: any = {}, config:AxiosRequestConfig = {}): Promise<HttpResponse<T>> {
        config.headers = {...this.headers, ...config.headers};
        const response : AxiosResponse<T, any> = await axios.post<T>(this.baseUrl + url, body, {validateStatus: ()=>true, ...config});
        console.log(`HttpCustomClient.Post<${url}> status:`);
        console.log(response.status);
        const responseResult = this.transformResponse<T>(response);
        return responseResult;
    }
    
    public async Put<T>(url: string, body: any = {}, config:AxiosRequestConfig = {}): Promise<HttpResponse<T>> {
        config.headers = {...this.headers, ...config.headers};
        const response : AxiosResponse<T, any> = await axios.put<T>(this.baseUrl + url, body, {validateStatus: ()=>true, ...config});
        console.log(`HttpCustomClient.Put<${url}> status:`);
        console.log(response.status);
        const responseResult = this.transformResponse<T>(response);
        return responseResult;
    }
    
    public async Delete<T>(url: string, config:AxiosRequestConfig = {}): Promise<HttpResponse<T>> {
        config.headers = {...this.headers, ...config.headers};
        const response : AxiosResponse<T, any> = await axios.delete<T>(this.baseUrl + url, {validateStatus: ()=>true, ...config});
        console.log(`HttpCustomClient.Delete<${url}> status:`);
        console.log(response.status);
        const responseResult = this.transformResponse<T>(response);
        return responseResult;
    }
}