import {Conversation} from "../DTOs/Conversation/Conversation";

export const ConversationHolder = (props: { conversation?: Conversation }) => {
    return <div className={"conversation-holder messages-window"}>
        <div className="mw-header">
            Conversation
        </div>
        <div className="mb-body mw-conversation"></div>
    </div>;
};