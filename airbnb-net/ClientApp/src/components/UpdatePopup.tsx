import {UserDTO} from "../DTOs/User/UserDTO";
import "../styles/UpdatePopup.scss";
import Cross from "./Icons/Cross";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import Photo from "./Icons/Photo";
import InputField from "./InputFIeld";
import React, {useMemo} from "react";
import {UserService} from "../services/UserService";
import {UserUpdateDTO} from "../DTOs/User/UserUpdateDTO";
import {ImageDTO} from "../DTOs/Image/ImageDTO";

interface UpdatePopupProps {
    onClose: () => void;
    user: UserDTO;
}

export const UpdatePopup = (props: UpdatePopupProps) => {
    
    const [name, setName] = React.useState<string>(props.user.name ?? "");
    const nameIsValid = (s:string) => s.length > 0 && RegExp("^([a-zA-Z]+( )?)*$").test(s);
    
    const [phone, setPhone] = React.useState<string>(props.user.phoneNumber ?? "");
    const phoneIsValid = (s:string) => s.length == 0 || RegExp("^(\\+)?[0-9]{10}$").test(s);    
    
    const [about, setAbout] = React.useState<string>(props.user.description ?? "");
    const aboutIsValid = (s:string) => true;
    
    const addressFormatted = () => {
        if(!props.user.address) return "";
        if(!props.user.address.street || !props.user.address.city || !props.user.address.country) return "";
        return `${props.user.address.street}, ${props.user.address.city}, ${props.user.address.country}`;
    }
    
    const [location, setLocation] = React.useState<string>(addressFormatted());

    const userService = useMemo(() => {return UserService.getInstance()}, []);
    function updateTheUser() {
        if(!nameIsValid(name) || !phoneIsValid(phone) || !aboutIsValid(about)) return;
        let user : UserUpdateDTO = {} as UserUpdateDTO;
        user.name = name;
        user.phoneNumber = phone;
        user.description = about;
        user.address = location;
        user.avatar = props.user.avatar ?? null;
        userService.update(props.user.id, user).then((result) => {
            if(result) {
                props.onClose();
                window.location.reload();
            }
        });
    }

    return <div>
        <div className={"update-popup-background"} onClick={props.onClose}></div>
        <div className={"update-popup"}>
            <div className="up-header margin-b20">
                <span onClick={props.onClose}>
                    <Cross/>
                </span>
                <h3>Update your profile</h3>
            </div>
            <div className="up-body">
                <div className="upop-avatar">
                    {props.user.avatar ? <img src={props.user.avatar.url} alt="avatar"/> : <DefaultAvatar></DefaultAvatar>}
                    <div className="up-avatar-overlay">
                        <Photo></Photo>  
                        <input type={"image"} value={""} className="change-avatar"/>
                    </div>
                </div>
                <div className="up-form">
                    <div className="up-form-group">
                        <InputField type={"text"} label={"Name"} value={name} onChange={(e)=>{setName(e.target.value)}} className={"input-container"} isValid={(s)=>nameIsValid(s)} onInvalid={(bool)=>{}}></InputField>
                    </div>
                    <div className="up-form-group">
                        <InputField type={"text"} label={"Phone number"} value={phone} onChange={(e)=>{setPhone(e.target.value)}} className={"input-container"} isValid={(s)=>phoneIsValid(s)} onInvalid={(bool)=>{}}></InputField>
                    </div>
                    <div className="up-form-group">
                        <InputField type={"text"} label={"About"} value={about} onChange={(e)=>{setAbout(e.target.value)}} className={"input-container"} isValid={(s)=>aboutIsValid(s)} onInvalid={(bool)=>{}}></InputField>
                    </div>
                    <div className="up-form-group">
                        <InputField type={"text"} label={"Location"} value={location} onChange={(e)=>{setLocation(e.target.value)}} className={"input-container"} isValid={(s)=>true} onInvalid={(bool)=>{}}></InputField>
                    </div>
                </div>
                <div className="clear-dates-button custom-width" onClick={updateTheUser}>Save</div>
            </div>
        </div>
    </div>;
};