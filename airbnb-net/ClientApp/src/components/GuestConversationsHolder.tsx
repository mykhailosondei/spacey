import {Conversation} from "../DTOs/Conversation/Conversation";
import {ConversationCard} from "./ConversationCard";

export const GuestConversationsHolder = (props: { selectedConversationId: string, conversations: Conversation[], setSelectedConversationId: Function }) => {
    
    function setSelected(conversationId: string) {
        console.log(conversationId);
        props.setSelectedConversationId(conversationId);
    }

    return <div className={"guest-conversations-holder messages-window"}>
        <div className="mw-header">
            Messages
        </div>
        <div className="mb-body">
            {props.conversations.map((conversation, index) => {
                return <ConversationCard showUser={false} conversation={conversation} key={conversation.id} isSelected={props.selectedConversationId === conversation.id} onClick={() => setSelected(conversation.id)}></ConversationCard>;
            })}
        </div>
    </div>;
};