import React, {useEffect, useMemo, useRef} from "react";
import {Conversation} from "../DTOs/Conversation/Conversation";
import Send from "./Icons/Send";
import {UserDTO} from "../DTOs/User/UserDTO";
import {UserService} from "../services/UserService";
import {HostService} from "../services/HostService";
import {HostDTO} from "../DTOs/Host/HostDTO";
import {Message} from "../DTOs/Message/Message";
import {Message as MessageComponent} from "./Message";

export const ConversationHolder = (props: { conversation: Conversation, sendMessage: (content: string) => void }) => {
    
    const conversationRef = useRef<HTMLDivElement>(null);
    const inputRef = useRef<HTMLInputElement>(null);
    
    const [isFaded, setIsFaded] = React.useState<boolean>(true);
    
    const [messagesAndDateDividers, setMessagesAndDateDividers] = React.useState<JSX.Element[]>([]);
    
    const [user, setUser] = React.useState<UserDTO | null>(null);
    const [hostUser, setHostUser] = React.useState<UserDTO | null>(null);
    
    const userService = useMemo(() => {return UserService.getInstance()}, []);
    const hostService = useMemo(() => {return HostService.getInstance()}, []);

    useEffect(() => {
        const result : JSX.Element[] = [];
        for (let i = 0; i < props.conversation.messages.length; i++) {
            const previousMessage : Message | null = i > 0 ? props.conversation.messages[i - 1] : null;
            const currentMessage = props.conversation.messages[i];
            if(previousMessage === null || new Date(currentMessage.createdAt).getDate() !== new Date(previousMessage.createdAt).getDate()){
                result.push(<div key={i} className="date-divider">{new Date(currentMessage.createdAt).toLocaleDateString("en-us", {dateStyle: "medium"})}</div>);
            }
            const userName = currentMessage.userId ? user?.name : hostUser?.name;
            const avatarUrl = currentMessage.userId ? user?.avatar?.url : hostUser?.avatar?.url;
            result.push(<MessageComponent
                userName={userName || ""}
                avatarUrl={avatarUrl || ""}
                messageTime={new Date(currentMessage.createdAt).toLocaleTimeString("en-us", {timeStyle: "short"})}
                messageContent={currentMessage.content}/>
            )
        }
        setMessagesAndDateDividers(result);
    }, [props.conversation]);
    
    useEffect(() => {
        if (conversationRef.current) {
            setIsFaded(conversationRef.current.scrollTop !== 0);
            conversationRef.current.addEventListener("scroll", (e) => {
                if (conversationRef.current) {
                    if (conversationRef.current.scrollTop === 0) {
                        setIsFaded(false);
                    } else {
                        setIsFaded(true);
                    }
                }
            });
        }
    }, [props.conversation]);

    useEffect(() => {
        userService.get(props.conversation.userId).then((response) => {
            setUser(response.data);
        });
        hostService.get(props.conversation.hostId).then((response) => {
            if(!response.data) return;
            userService.get(response.data.userId).then((user) => {
                setHostUser(user.data);
            });
        });
    }, []);
    
    const handleSendMessage = () => {
        if (inputRef.current) {
            props.sendMessage(inputRef.current.value);
            inputRef.current.value = "";
        }
    }
    
    return <div className={"conversation-holder messages-window"}>
        <div className="mw-header">
            Conversation
        </div>
        <div ref={conversationRef} className={`mw-body mw-conversation ${isFaded ? "top-fade" : ""}`}>
            <div className="ch-content">
                {messagesAndDateDividers}
            </div>
            <div className="mw-footer">
                <div className="mw-input">
                    <input ref={inputRef} type="text" placeholder={"Type a message"}/>
                </div>
                <div className="mw-send" onClick={handleSendMessage}>
                    <Send/>
                </div>
            </div>
        </div>
    </div>;
};