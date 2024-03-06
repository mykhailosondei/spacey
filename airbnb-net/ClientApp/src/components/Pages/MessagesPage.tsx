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
import {useParams} from "react-router-dom";
import {ConnectionService} from "../../services/ConnectionService";

export const MessagesPage = () => {
    
    const {bookingId} = useParams();
    
    const connectionService = useMemo(() => {return ConnectionService.getInstance()}, []);
    
    const { user } = useUser();
    
    
    const conversationService = useMemo(() => {return ConversationService.getInstance()}, []);
    const messageService = useMemo(() => {return MessageService.getInstance()}, []);
    
    const [conversations, setConversations] = React.useState<Conversation[]>([]);
    const [selectedConversationId, setSelectedConversationId] = React.useState<string>("");

    
    useEffect(() => {
        connectionService.startConnection();
        connectionService.addMessageListener("ReceiveNotification", (conversationId: string) => {
            console.log("Received message in conversation: " + conversationId);
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
        if (!user) return;
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

    useEffect(() => {
        if (!user) return;
        conversationService.getUserConversations(user.id).then((response) => {
            setConversations(response.data);
            setSelectedConversationId(response.data[0]?.id || "");
        });
    }, [user]);

    useEffect(() => {
        if(bookingId) {
            conversationService.createByBooking(bookingId).then((response) => {
                if (response.status === 200) {
                    conversationService.get(response.data).then((response) => {
                        setConversations(prevState => [...prevState, response.data]);
                        setSelectedConversationId(response.data.id);
                    });
                }
            });
        }
    }, []);
    
    
    function sendCustomMessage(messageText: string) {
        messageService.sendMessage(selectedConversationId, messageText).then((response) => {
            if(response.status === 200) {
                conversationService.get(selectedConversationId).then((response) => {
                    setConversations(prevState => [ response.data, ...prevState.filter((c) => c.id !== selectedConversationId)]);
                });
            }
        });
    }
    
    const getSelectedConversation = () => {
        return conversations.find((c) => c.id === selectedConversationId)!;
    }

    return conversations.length !== 0 ? <div className={"messages-page"}>
        <div className="up-nav-wrapper"><NavBar></NavBar></div>
        <div className="messages-windows-holder">
            <GuestConversationsHolder conversations={conversations} selectedConversationId={selectedConversationId} setSelectedConversationId={setSelectedConversationId}/>
            <ConversationHolder conversation={getSelectedConversation()} sendMessage={sendCustomMessage} />
            <GuestConversationDetails />
        </div>
    </div> : <h1>
        You have no conversations!
        <a href="/">Go home</a> 
    </h1>;
};