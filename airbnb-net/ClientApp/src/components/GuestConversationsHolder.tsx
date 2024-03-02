import {Conversation} from "../DTOs/Conversation/Conversation";

export const GuestConversationsHolder = (props: { selectedConversationId: string, conversations: Conversation[] }) => {
    
    function getAvatar() {
        
    }

    return <div className={"guest-conversations-holder messages-window"}>
        <div className="mw-header">
            Messages
        </div>
        <div className="mb-body">
            {props.conversations.map((conversation) => {
                return <div>
                    <ConversationCard conversationId={conversation.id} />
                </div>
            })}
        </div>
    </div>;
};