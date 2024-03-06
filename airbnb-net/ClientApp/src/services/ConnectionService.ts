import * as SignalR from "@microsoft/signalr";

export class ConnectionService{
    private connection: SignalR.HubConnection;
    
    private constructor(){
        this.connection = new SignalR.HubConnectionBuilder()
            .withUrl("https://localhost:7171/message",
                {
                    accessTokenFactory: () => {
                        return localStorage.getItem("token")!;
                    }
                })
            .build();
    }
    
    private static _instance: ConnectionService;
    
    public static getInstance(){
        return this._instance || (this._instance = new this());
    }
    
    public startConnection = async () => {
        if(this.connection.state === SignalR.HubConnectionState.Connected) return;
        try {
            await this.connection.start();
            console.log("Connected");
        } catch (err) {
            console.log(err);
            setTimeout(this.startConnection, 5000);
        }
    }
    
    public addMessageListener = (methodName: string,callback: (message: string) => void) => {
        if(this.connection.state === SignalR.HubConnectionState.Connected) return;
        this.connection.on(methodName, callback);
    }
    
    public removeMessageListener = (methodName: string,callback: (message: string) => void) => {
        this.connection.off(methodName, callback);
    }
}