import React from "react";

interface CounterEditItemProps {
    name: string;
    value: number;
    onSubmit: (value: number) => void;
    unavailable?: boolean;
    topLimit?: number;
}

export const CounterEditItem = (props: CounterEditItemProps) => {

    function handleChange(number: number) {
        if(number >= 0 && (!props.topLimit || number <= props.topLimit)) {
            props.onSubmit(number);
        }
    }

    return <div className="counter-edit-item">
            <div className="name-title-and-edit">
                <div className="name-title">{props.name}</div>
                <div className="guest-counter">
                    <button className="guest-counter-button" disabled={!!props.unavailable} onClick={() => handleChange(props.value - 1)}>-</button>
                    <div className="guest-counter-number">{props.value}</div>
                    <button className="guest-counter-button" disabled={!!props.unavailable} onClick={() => handleChange(props.value + 1)}>+</button>
                </div>
            </div>
        <div className="horizontal-divider width-100"></div>
    </div>;
};