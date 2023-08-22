import React, {ReactComponentElement, ReactSVGElement, useState} from "react";
import './Arrow.scss'

interface ArrowProps{
    svg : ReactComponentElement<any>;
    isVisibleInitial:boolean;
    onClickFunction:Function;
}

const Arrow : React.FC<ArrowProps> = ({svg, isVisibleInitial, onClickFunction})  =>{
    
    return (
        <>
            <div className={"isVisible-"+isVisibleInitial}>
                <button className="arrow-button" onClick={()=>{onClickFunction()}}>
                    {svg}
                </button>
            </div>
        </>
    )
}

export default Arrow;