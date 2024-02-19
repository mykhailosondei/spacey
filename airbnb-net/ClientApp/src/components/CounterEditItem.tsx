import React from "react";

interface CounterEditItemProps {
    name: string;
    value: number;
    onSubmit: (value: number) => void;
}

export const CounterEditItem = (props: CounterEditItemProps) => {
    return <div className="counter-edit-item">
            <div className="name-title-and-edit">
                <div className="name-title">{props.name}</div>
                <div className="guest-counter">
                    <button className="guest-counter-button">-</button>
                    <div className="guest-counter-number">{props.value}</div>
                    <button className="guest-counter-button">+</button>
                </div>
            </div>
        <div className="horizontal-divider width-100"></div>
    </div>;
};