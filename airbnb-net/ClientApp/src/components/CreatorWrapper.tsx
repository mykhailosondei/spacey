import {ReactNode} from "react";

interface CreatorWrapperParams {
    children: ReactNode,
    title?: string,
    subtitle?: string
}

export const CreatorWrapper = (props: CreatorWrapperParams) => {
    return <div className="creator-wrapper">
        <div className="creator-wrapper-title">{props.title}</div>
        <div className="creator-wrapper-subtitle">{props.subtitle}</div>
        {props.children}
    </div>;
};