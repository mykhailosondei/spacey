interface IconLabelButtonParams {
    icon: JSX.Element;
    label: string;
}

export const IconLabelButton = (props: IconLabelButtonParams) => {
    return <div className={"icon-label-btn"}>
        {props.icon}
        <div>{props.label}</div>
    </div>;
};