import {UserDTO} from "../DTOs/User/UserDTO";
import "../styles/UpdatePopup.scss";
import Cross from "./Icons/Cross";
import {ReactComponent as DefaultAvatar} from "../values/svgs/default-profile.svg";
import Photo from "./Icons/Photo";
import InputField from "./InputFIeld";
import React from "react";

interface UpdatePopupProps {
    onClose: () => void;
    user: UserDTO;
}

export const UpdatePopup = (props: UpdatePopupProps) => {
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
                        <InputField type={"text"} label={"Name"} value={""} onChange={(e)=>{}} className={"input-container"} isValid={(s)=>true} onInvalid={(bool)=>{}}></InputField>
                    </div>
                    <div className="up-form-group">
                        <InputField type={"text"} label={"Phone number"} value={""} onChange={(e)=>{}} className={"input-container"} isValid={(s)=>true} onInvalid={(bool)=>{}}></InputField>
                    </div>
                    <div className="up-form-group">
                        <InputField type={"text"} label={"About"} value={""} onChange={(e)=>{}} className={"input-container"} isValid={(s)=>true} onInvalid={(bool)=>{}}></InputField>
                    </div>
                    <div className="up-form-group">
                        <InputField type={"text"} label={"Location"} value={""} onChange={(e)=>{}} className={"input-container"} isValid={(s)=>true} onInvalid={(bool)=>{}}></InputField>
                    </div>
                </div>
                <div className="clear-dates-button custom-width">Save</div>
            </div>
        </div>
    </div>;
};