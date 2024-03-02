import {Conversation} from "../DTOs/Conversation/Conversation";

export const GuestConversationDetails = (props: { conversation?: Conversation }) => {
    return <div className={"guest-conversation-details messages-window"}>
        <div className="mw-header">
            Details
        </div>
        <div className="mb-body"></div>
    </div>;
};