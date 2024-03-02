import React, {useEffect, useMemo} from "react";
import "../../styles/MessagesPage.scss";
import * as SignalR from "@microsoft/signalr";
import ConversationService from "../../services/ConversationService";
import MessageService from "../../services/MessageService";
import NavBar from "../NavBar";
import {GuestConversationsHolder} from "../GuestConversationsHolder";
import {ConversationHolder} from "../ConversationHolder";
import {GuestConversationDetails} from "../GuestConversationDetails";
import {Conversation} from "../../DTOs/Conversation/Conversation";
import {useUser} from "../../Contexts/UserContext";

export const MessagesPage = () => {
    
    
    const buildHubConnection = () => {
        return new SignalR.HubConnectionBuilder()
            .withUrl("https://localhost:7171/message",
                {
                    accessTokenFactory: () => {
                        return localStorage.getItem("token")!;
                    }
                })
            .build();
    }
    
    const { user } = useUser();
    
    const connection = useMemo(() => {return buildHubConnection()}, []);
    
    const conversationService = useMemo(() => {return ConversationService.getInstance()}, []);
    const messageService = useMemo(() => {return MessageService.getInstance()}, []);
    
    const [conversations, setConversations] = React.useState<Conversation[]>([]);
    const [selectedConversationId, setSelectedConversationId] = React.useState<string>("");
    

    useEffect(() => {
        connection.on("ReceiveMessage", function (user, message) {
            console.log("Received message: " + message);
        });
        connection.start().then(function () {
            console.log("Connected to message hub");
        }).catch(function (err) {
            return console.error(err.toString());
        });
        
    }, []);

    useEffect(() => {
        if (!user) return;
        conversationService.getUserConversations(user.id).then((response) => {
            setConversations(response.data);
            setSelectedConversationId(response.data[0].id)
        });
    }, [user]);
    
    
    function sendCustomMessage() {
        messageService.sendMessage("454f8d36-56b2-4f5a-8bf3-597286c6a4c3", "Hello world!").then((response) => {
            console.log(response);
        });
    }

    function newConversation() {
        conversationService.create(
            "4961f9a3-9251-4c45-a251-a34101991c38",
            "964b10b5-a47c-4a64-a4ab-1f96bd5820ec",
            "93178b36-1f09-4baf-92dc-941a520a3473"
        ).then((response) => {
            console.log(response);
        });
    }

    return <div className={"messages-page"}>
        <div className="up-nav-wrapper"><NavBar></NavBar></div>
        <div className="messages-windows-holder">
            <GuestConversationsHolder conversations={conversations} selectedConversationId={""}/>
            <ConversationHolder />
            <GuestConversationDetails />
        </div>
    </div>;
};