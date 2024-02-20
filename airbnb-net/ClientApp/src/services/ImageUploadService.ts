import {HttpCustomClient, HttpResponse} from "./HttpCustomClient";

export class ImageUploadService {
    http: HttpCustomClient;
    baseUrl: string;
    private constructor() {
        this.http = HttpCustomClient.getInstance();
        this.baseUrl = "/imageUpload";
    }
    
    private static instance: ImageUploadService;
    public static getInstance(): ImageUploadService {
        if (!ImageUploadService.instance) {
            ImageUploadService.instance = new ImageUploadService();
        }
        return ImageUploadService.instance;
    }
    
    async uploadImage(file: File) {
        console.log(file);
        let formData = new FormData();
        formData.append("image", file);
        return await this.http.Post<string>(`${this.baseUrl}/upload`, formData, {headers: {"Content-Type": "multipart/form-data"}})
    }
    
    async uploadMany(files: File[]) {
        let result = [] as HttpResponse<string>[];
        await Promise.all(files.map(async (file) => {
            result.push(await this.uploadImage(file));
        }));
        return result;
    }
    
    async deleteImage(imageUrl: string) {
        return await this.http.Delete(`${this.baseUrl}/delete?imageUrl=${imageUrl}`);
    }
}