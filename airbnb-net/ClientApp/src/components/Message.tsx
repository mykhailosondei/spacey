import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import {Message as MessageDTO} from "../DTOs/Message/Message";
import {useEffect, useMemo, useState} from "react";
import {UserDTO} from "../DTOs/User/UserDTO";
import {useUser} from "../Contexts/UserContext";
import {AuthenticationState, useAuthState} from "../Contexts/AuthStateProvider";
import {useHost} from "../Contexts/HostContext";
import {UserService} from "../services/UserService";
import {HostService} from "../services/HostService";

interface MessageProps {
    userName : string
    avatarUrl : string
    messageTime : string
    messageContent : string
}

export const Message = (props: MessageProps) => {
    
    return <div className="ch-message">
        <div className="ch-avatar">
            {props.avatarUrl ? <img src={props.avatarUrl} alt=""/> : <DefaultAvatar/>}
        </div>
        
        <div className="ch-message-content">
            <div className="ch-message-name-and-time">
                <div className="ch-message-name">
                    {props.userName}
                </div>
                <div className="reg-text-med gray-text">
                    {props.messageTime}
                </div>
            </div>
            <div className="ch-message-text">
                {props.messageContent}
            </div>
        </div>
    </div>;
};