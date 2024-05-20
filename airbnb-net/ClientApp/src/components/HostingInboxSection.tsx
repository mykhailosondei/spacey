import React, {useEffect, useMemo} from "react";
import ConversationService from "../services/ConversationService";
import MessageService from "../services/MessageService";
import {Conversation} from "../DTOs/Conversation/Conversation";
import {useHost} from "../Contexts/HostContext";
import {ConversationHolder} from "./ConversationHolder";
import {ConversationDetails} from "./ConversationDetails";
import "../styles/HostingInboxSection.scss";
import {HostConversationHolder} from "./HostConversationHolder";
import {ConnectionService} from "../services/ConnectionService";
import {Link, useParams, useSearchParams} from "react-router-dom";

export const HostingInboxSection = () => {

    const connectionService = useMemo(() => {return ConnectionService.getInstance()}, []);
    const { host } = useHost();
    const {bookingId} = useParams();

    const conversationService = useMemo(() => {return ConversationService.getInstance()}, []);
    const messageService = useMemo(() => {return MessageService.getInstance()}, []);

    const [conversations, setConversations] = React.useState<Conversation[]>([]);
    const [selectedConversationId, setSelectedConversationId] = React.useState<string>("");
    const [isLoaded, setIsLoaded] = React.useState<boolean>(false);
    
    
    useEffect(() => {
        connectionService.startConnection();
        connectionService.addMessageListener("ReceiveNotification", (conversationId: string) => {
            console.log("Received message in conversation: " + conversationId);
            if(conversationId === selectedConversationId){
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
    
    const loadConversations = () => {
        if (!host) return;
        if(bookingId) {
            conversationService.getByBooking(bookingId).then((response) => {
                setSelectedConversationId(response.data.id);
            });
        }
        conversationService.getHostConversations(host.id).then((response) => {
            setConversations(response.data);
            setSelectedConversationId(response.data[0]?.id || "");
            setIsLoaded(true);
        });
    }

    useEffect(() => {
        loadConversations();
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
        return conversations.find((c) => c.id === selectedConversationId);
    }
    
    const sendMessage = async (messageText: string) => {
        let response = await messageService.sendMessage(selectedConversationId, messageText);
        if (response.status === 200) {
            conversationService.get(selectedConversationId).then((response) => {
                setConversations(prevState => [response.data, ...prevState.filter((c) => c.id !== selectedConversationId)]);
            });
        }
    }
    
    if(!isLoaded) return <div className={"hosting-inbox-section"}></div>

    return (conversations.length !== 0) ? <div className={"hosting-inbox-section"}>
        <div className="messages-windows-holder">
            <HostConversationHolder conversations={conversations} setConversations={setConversations} setSelectedConversationId={setSelectedConversationId} selectedConversationId={selectedConversationId} />
            { getSelectedConversation() ? <ConversationHolder conversation={getSelectedConversation()!} sendMessage={sendMessage}/> : <div className={"conversation-holder messages-window"}></div>}
            <ConversationDetails isHostMode={true} conversation={getSelectedConversation()} />
        </div>
    </div> : <div className={"hosting-inbox-section"}>
        <div className="no-conversations-holder">
            <div className="no-conversations">No conversations yet</div>
            <div className="go-to-main-link">
                <Link to={"/hosting"}>Go to the main page to start one!</Link>
            </div>
        </div>
    </div>;
}