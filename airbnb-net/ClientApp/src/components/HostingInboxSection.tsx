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
import {ConnectionService} from "../services/ConnectionService";

export const HostingInboxSection = () => {

    const connectionService = useMemo(() => {return ConnectionService.getInstance()}, []);
    const { host } = useHost();

    const conversationService = useMemo(() => {return ConversationService.getInstance()}, []);
    const messageService = useMemo(() => {return MessageService.getInstance()}, []);

    const [conversations, setConversations] = React.useState<Conversation[]>([]);
    const [selectedConversationId, setSelectedConversationId] = React.useState<string>("");
    
    
    useEffect(() => {
        connectionService.startConnection();
        connectionService.addMessageListener("ReceiveNotification", (conversationId: string) => {
            console.log("Received message in conversation: " + conversationId);
            if(conversationId === selectedConversationId){
                console.log("Marking as read right away");
                markAsRead(conversationId);
            }
            conversationService.get(conversationId).then((response) => {
                if(response.status === 200)
                    setConversations(prevState => [response.data, ...prevState.filter((c) => c.id !== response.data.id)]);
            });
        });
        connectionService.addMessageListener("ReadNotification", (conversationId: string) => {
            console.log("Received read notification in conversation: " + conversationId);
            conversationService.get(conversationId).then((response) => {
                if(response.status === 200)
                    setConversations(prevState => prevState.map((c) => c.id === conversationId ? response.data : c));
            });
        });
    }, []);

    useEffect(() => {
        if (!host) return;
        conversationService.getHostConversations(host.id).then((response) => {
            setConversations(response.data);
            setSelectedConversationId(response.data[0]?.id || "")
        });
    }, [host]);

    useEffect(() => {
        if (!host) return;
        markAsRead(selectedConversationId);
    }, [selectedConversationId]);

    const markAsRead = (conversationId: string) => {
        conversationService.markAsRead(conversationId).then((response) => {
            if(response.status === 200) {
                conversationService.get(conversationId).then((response) => {
                    setConversations(prevState => prevState.map((c) => c.id === conversationId ? response.data : c));
                });
            }
        });
    }
    
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