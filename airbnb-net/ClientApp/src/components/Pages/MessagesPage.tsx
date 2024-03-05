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

export const MessagesPage = () => {
    
    const {bookingId} = useParams();
    
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