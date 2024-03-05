import {Conversation} from "../DTOs/Conversation/Conversation";
import {ConversationCard} from "./ConversationCard";

export const HostConversationHolder = (props: {
    selectedConversationId: string,
    setSelectedConversationId: (value: (((prevState: string) => string) | string)) => void,
    conversations: Conversation[]
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