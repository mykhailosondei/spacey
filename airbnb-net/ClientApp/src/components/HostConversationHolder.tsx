import {Conversation} from "../DTOs/Conversation/Conversation";
import {ConversationCard} from "./ConversationCard";
import Filter from "./Icons/Filter";
import {useMemo, useState} from "react";
import {FilterPopup} from "./FilterPopup";
import filter from "./Icons/Filter";
import ConversationService from "../services/ConversationService";
import {useNavigate} from "react-router-dom";

export const HostConversationHolder = (props: {
    selectedConversationId: string,
    setSelectedConversationId: (value: (((prevState: string) => string) | string)) => void,
    conversations: Conversation[],
    setConversations: (value: (((prevState: Conversation[]) => Conversation[]) | Conversation[])) => void
}) => {
    
    
    
    function setSelected(conversationId: string) {
        console.log(conversationId);
        props.setSelectedConversationId(conversationId);
    }
    
    return <div className={"guest-conversations-holder messages-window"}>
        <div className="mw-header">
            Messages
        </div>
        <div className="mb-body">
            {props.conversations.map((conversation) => {
                return <ConversationCard showUser={true} conversation={conversation} key={conversation.id} isSelected={props.selectedConversationId === conversation.id} onClick={() => setSelected(conversation.id)}></ConversationCard>;
            })}
        </div>
    </div>;
};