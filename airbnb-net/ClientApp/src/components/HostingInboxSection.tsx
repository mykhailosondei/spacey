import * as SignalR from "@microsoft/signalr";
import React, {useEffect, useMemo} from "react";
import ConversationService from "../services/ConversationService";
import MessageService from "../services/MessageService";
import {Conversation} from "../DTOs/Conversation/Conversation";
import {useHost} from "../Contexts/HostContext";
import {ConversationHolder} from "./ConversationHolder";
import {GuestConversationDetails} from "./GuestConversationDetails";
import {GuestConversationsHolder} from "./GuestConversationsHolder";
import "../styles/HostingInboxSection.scss";
import {HostConversationHolder} from "./HostConversationHolder";

export const HostingInboxSection = () => {

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

    const { host } = useHost();

    const connection = useMemo(() => {return buildHubConnection()}, []);

    const conversationService = useMemo(() => {return ConversationService.getInstance()}, []);
    const messageService = useMemo(() => {return MessageService.getInstance()}, []);

    const [conversations, setConversations] = React.useState<Conversation[]>([]);
    const [selectedConversationId, setSelectedConversationId] = React.useState<string>("");


    connection.on("ReceiveNotification", function (conversationId) {
        console.log("Received message in conversation: " + conversationId);
        conversationService.get(conversationId).then((response) => {
            if(response.status === 200)
                setConversations(prevState => [response.data, ...prevState.filter((c) => c.id !== response.data.id)]);
        });
    });
    
    useEffect(() => {
        connection.start().then(function () {
            console.log("Connected to message hub");
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }, []);

    useEffect(() => {
        if (!host) return;
        conversationService.getHostConversations(host.id).then((response) => {
            setConversations(response.data);
            setSelectedConversationId(response.data[0]?.id || "")
        });
    }, [host]);
    
    const getSelectedConversation = () => {
        return conversations.find((c) => c.id === selectedConversationId)!;
    }
    
    const sendMessage = (messageText: string) => {
        messageService.sendMessage(selectedConversationId, messageText).then((response) => {
            if(response.status === 200) {
                conversationService.get(selectedConversationId).then((response) => {
                    setConversations(prevState => [response.data, ...prevState.filter((c) => c.id !== selectedConversationId)]);
                });
            }
        });
    
    }
    
    return conversations.length !== 0 ? <div className={"hosting-inbox-section"}>
        <div className="messages-windows-holder">
            <HostConversationHolder conversations={conversations} setSelectedConversationId={setSelectedConversationId} selectedConversationId={selectedConversationId} />
            <ConversationHolder conversation={getSelectedConversation()} sendMessage={sendMessage}/>
            <GuestConversationDetails conversation={getSelectedConversation()} />
        </div>
    </div> : <div className={"hosting-inbox-section"}>No conversations</div>;
}