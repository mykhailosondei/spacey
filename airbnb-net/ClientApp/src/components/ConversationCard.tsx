import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import ConversationService from "../services/ConversationService";
import {useEffect, useMemo, useState} from "react";
import {Conversation} from "../DTOs/Conversation/Conversation";
import {UserService} from "../services/UserService";
import {UserDTO} from "../DTOs/User/UserDTO";
import {BookingDTO} from "../DTOs/Booking/BookingDTO";
import {BookingService} from "../services/BookingService";
import {ListingService} from "../services/ListingService";
import {HostService} from "../services/HostService";
import {useUser} from "../Contexts/UserContext";
import {useHost} from "../Contexts/HostContext";

export const ConversationCard = (props: { conversation: Conversation, showUser: boolean, isSelected: boolean, onClick: Function }) => {

    const [bookingDates, setBookingDates] = useState("");
    const [listingTitle, setListingTitle] = useState("");
    const [user, setUser] = useState<UserDTO | null>(null);
    
    const userService = useMemo(() => {return UserService.getInstance()}, []);
    const hostService = useMemo(() => {return HostService.getInstance()}, []);
    const bookingService = useMemo(() => {return BookingService.getInstance()}, []);
    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    
    const fetchUser = () => {
        if (props.showUser) {
            userService.get(props.conversation.userId).then((response) => {
                if(response.status === 200)
                setUser(response.data);
            });
        } else {
            hostService.get(props.conversation.hostId).then((response) => {
                if (response.status === 200)
                    userService.get(response.data.userId).then((response) => {
                        if(response.status === 200)
                            setUser(response.data);
                    });
            });
        }
    }
    
    useEffect(() => {
        fetchUser();
        bookingService.get(props.conversation.bookingId).then((response) => {
            let booking = response.data;
            setBookingDates(`${smallDate(booking.checkIn.toString())} - ${smallDate(booking.checkOut.toString())}`);
            listingService.get(response.data.listingId).then((response) => {
                setListingTitle(response.data.title);
            });
        });
    }, []);
    
    const getLastMessage = () => {
        return props.conversation.messages[props.conversation.messages.length - 1] || {content: ""};
    }
    
    const smallDate = (date: string) => {
        return new Date(date).toLocaleDateString("en-US", {month: "short", day: "numeric"}); 
    }
    
    const showUnread = () => {
        if(props.conversation.isRead) return false;
        return (props.showUser && props.conversation.messages[props.conversation.messages.length - 1].userId !== null)
            || (!props.showUser && props.conversation.messages[props.conversation.messages.length - 1].hostId !== null);
    }
    
    return user && <div className={`conversation-card ${showUnread() ? "unread" : ""}`} onClick={() => props.onClick()}>
        <div className={`cc-contents ${props.isSelected ? "cc-active" : ""}`}>
            <div className="cc-avatar-holder">
                <div className="cc-user-avatar">
                    {user.avatar?.url ? <img src={user.avatar.url} alt=""/> : <DefaultAvatar/>}
                </div>
            </div>
            <div className="cc-text-content">
                <div className="cc-top-info">
                    <div className="reg-text-small">Request withdrawn</div>
                    <div className="reg-text-small gray-text">Mon</div>
                </div>
                <div className="cc-name">{user.name}</div>
                <div className={"cc-last-message-wrap"}>
                    <div className="cc-last-message">
                        {getLastMessage().content}
                    </div>
                </div>
                <div className="cc-footer-info gray-text">
                    <div className="reg-text-small">{bookingDates}</div>
                    <div className="dot-divider"></div>
                    <div className="reg-text-small">{listingTitle}</div>
                </div>
            </div>
        </div>
    </div>
        
};