import React, {useEffect, useMemo} from "react";
import "../../styles/MessagesPage.scss";
import * as SignalR from "@microsoft/signalr";
import ConversationService from "../../services/ConversationService";
import MessageService from "../../services/MessageService";
import NavBar from "../NavBar";
import {GuestConversationsHolder} from "../GuestConversationsHolder";
import {ConversationHolder} from "../ConversationHolder";
import {ConversationDetails} from "../ConversationDetails";
import {Conversation} from "../../DTOs/Conversation/Conversation";
import {useUser} from "../../Contexts/UserContext";
import {useLocation, useNavigate, useParams} from "react-router-dom";
import {ConnectionService} from "../../services/ConnectionService";

export const MessagesPage = () => {
    
    const {bookingId, conversationId} = useParams();
    
    const connectionService = useMemo(() => {return ConnectionService.getInstance()}, []);
    
    const { user } = useUser();
    
    const navigate = useNavigate();
    
    const conversationService = useMemo(() => {return ConversationService.getInstance()}, []);
    const messageService = useMemo(() => {return MessageService.getInstance()}, []);
    
    const [conversations, setConversations] = React.useState<Conversation[]>([]);
    const [selectedConversationId, setSelectedConversationId] = React.useState<string>("");
    
    const location = useLocation();

    
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
    
    const loadConversations = () => {
        if (!user) return;
        conversationService.getUserConversations(user.id).then((response) => {
            setConversations(response.data);
        });
        if (conversationId) {
            if(conversations.find((c) => c.id === conversationId)) {
                setSelectedConversationId(conversationId);
            }else {
                setSelectedConversationId("");
            }
            return;
        }
        if(bookingId) {
            conversationService.createByBooking(bookingId).then((response) => {
                if (response.status === 200) {
                    conversationService.get(response.data).then((response) => {
                        setConversations(prevState => [...prevState, response.data]);
                        setSelectedConversationId(response.data.id);
                    });
                }
            });
            conversationService.getByBooking(bookingId).then((response) => {
                if (response.status === 200) {
                    setSelectedConversationId(response.data.id || "");
                }
            });
        }
    }

    useEffect(() => {
        loadConversations();
    }, [user, location.pathname]);
    
    
    async function sendCustomMessage(messageText: string) {
        messageService.sendMessage(selectedConversationId, messageText).then((response) => {
            if(response.status === 200) {
                conversationService.get(selectedConversationId).then((response) => {
                    setConversations(prevState => [ response.data, ...prevState.filter((c) => c.id !== selectedConversationId)]);
                });
            }
        });
    }
    
    const getSelectedConversation = () => {
        return conversations.find((c) => c.id === selectedConversationId) || conversations[0];
    }
    
    const selectConversation = (conversationId: string) => {
        //setSelectedConversationId(conversationId);
        navigate(`/messages/${conversationId}`)
    }

    return conversations.length !== 0 ? <div className={"messages-page"}>
        <div className="up-nav-wrapper"><NavBar></NavBar></div>
        <div className="messages-windows-holder">
            <GuestConversationsHolder conversations={conversations} selectedConversationId={selectedConversationId} setSelectedConversationId={selectConversation}/>
            <ConversationHolder conversation={getSelectedConversation()} sendMessage={sendCustomMessage} />
            <ConversationDetails conversation={getSelectedConversation()} />
        </div>
    </div> : <h1>
        You have no conversations!
        <a href="/">Go home</a> 
    </h1>;
};