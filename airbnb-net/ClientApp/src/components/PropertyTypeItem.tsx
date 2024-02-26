//add props such as svg, title, isSelected, href
import {useEffect, useState} from "react";
import {propertyTypeDictionary} from "../values/PropertyTypeTitles";
import "../styles/PropertyTypeItem.scss";

interface PropertyTypeItemProps {
    title: string;
    isSelected: boolean;
    href: string;
    onClick: Function;
    className?: string;
}

const PropertyTypeItem : React.FC<PropertyTypeItemProps> = (props) => {
    const [image, setImage] = useState<string>("");
    
    useEffect(() => {
        if(propertyTypeDictionary[props.title] === undefined){
            throw new Error("Property type title not found in dictionary");
        }   
        else {
            setImage(propertyTypeDictionary[props.title]);
        }
    }, []);
    
    return <>
        <div className={"flex-wrapper-row"}>
            <div className={'items-holder items-holder-' + (props.isSelected?'active':'') + (props.className ? ` ${props.className}` : "")} onClick={()=>props.onClick(props.title)}>
                <img className={'property-type-image'} src={image} alt={props.title}/>
                <div className={'title-wrapper title-wrapper-' + (props.isSelected?'active':'')}>
                    <span className="item-title">{props.title}</span>
                </div>
            </div>
        </div>
    </>;
}

export default PropertyTypeItem;