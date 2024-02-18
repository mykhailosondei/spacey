import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import {useEffect, useState} from "react";

export const PriceCreator = (props: {
    setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void,
    listing: ListingCreateDTO
}) => {
    
    const [price, setPrice] = useState<number>(props.listing.pricePerNight ?? 0);
    
    const validatePrice = (value: string) => {
        return !isNaN(Number(value)) && Number(value) > 15 && Number(value) < 10000;
    }
    
    const changePrice = (value: string) => {
        if (isNaN(Number(value))) return;
        if(Number(value) < 15) value = "15";
        if(Number(value) > 10000) value = "10000";
        setPrice(Number(value));
    }

    useEffect(() => {
        if (!validatePrice(price.toString())) return;
        props.setListing((prevState: ListingCreateDTO) => {
            return {...prevState, pricePerNight: price};
        });
    }, [price]);
    
    const calculatePrice = () => {
        return Math.ceil(price * 1.25);
    }
    
    return <div className="price-creator">
        <div className={"price-holder"}>
            <span className="big-dollar">$</span>
            <input type="text" className={"big-price input-field " + (validatePrice(price.toString()) ? "" : "invalid")}
                   value={price} onChange={(e) => changePrice(e.target.value)}/>
        </div>
        Guest price ${calculatePrice()}
    </div>  
};