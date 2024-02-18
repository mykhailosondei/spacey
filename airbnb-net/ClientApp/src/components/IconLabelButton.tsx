interface IconLabelButtonParams {
    icon?: JSX.Element;
    label: string;
    image?: string;
    className?: string;
    chosen?: boolean;
    onClick?: () => void;
}

export const IconLabelButton = (props: IconLabelButtonParams) => {
    return <div className={"icon-label-btn " + (props.className ? props.className : " ")+ (props.chosen? " chosen": " ")} onClick={props.onClick}>
        {props.icon}
        {props.image ? <img className={"icon-image"} src={props.image} alt=""/> : null}
        <div>{props.label}</div>
    </div>;
};