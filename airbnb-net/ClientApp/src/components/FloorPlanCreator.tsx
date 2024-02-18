import {ListingCreateDTO} from "../DTOs/Listing/ListingCreateDTO";
import React, {useEffect} from "react";

interface FloorPlanCreatorParams {
        setListing: (value: (((prevState: ListingCreateDTO) => ListingCreateDTO) | ListingCreateDTO)) => void;
        listing: ListingCreateDTO
}

export const FloorPlanCreator = (props: FloorPlanCreatorParams) => {
        
        const [numberOfGuests, setNumberOfGuests] = React.useState<number>(1);
        const [numberOfRooms, setNumberOfRooms] = React.useState<number>(1);
        const [numberOfBathrooms, setNumberOfBathrooms] = React.useState<number>(0);

        useEffect(() => {
                props.setListing({...props.listing, numberOfGuests: numberOfGuests, numberOfRooms: numberOfRooms, numberOfBathrooms: numberOfBathrooms});
        }, [numberOfRooms, numberOfBathrooms, numberOfGuests]);
        
        return <div className={"floor-plan-creator"}>
                <div className="counter">
                        Guests
                        <div className="guest-counter">
                                <button className="guest-counter-button" onClick={() => setNumberOfGuests(numberOfGuests-1)} disabled={numberOfGuests==1}>-</button>
                                <div className="guest-counter-number">{numberOfGuests}</div>
                                <button className="guest-counter-button" onClick={() => setNumberOfGuests(numberOfGuests+1)} disabled={numberOfGuests==4}>+</button>
                        </div>
                </div>
                <div className="horizontal-divider"></div>
                <div className="counter">
                        Rooms
                        <div className="guest-counter">
                                <button className="guest-counter-button" onClick={() => setNumberOfRooms(numberOfRooms-1)} disabled={numberOfRooms==1}>-</button>
                                <div className="guest-counter-number">{numberOfRooms}</div>
                                <button className="guest-counter-button" onClick={() => setNumberOfRooms(numberOfRooms+1)} disabled={numberOfRooms==4}>+</button>
                        </div>
                </div>
                <div className="horizontal-divider"></div>
                <div className="counter">
                        Bathrooms
                        <div className="guest-counter">
                                <button className="guest-counter-button" onClick={() => setNumberOfBathrooms(numberOfBathrooms-1)} disabled={numberOfBathrooms==0}>-</button>
                                <div className="guest-counter-number">{numberOfBathrooms}</div>
                                <button className="guest-counter-button" onClick={() => setNumberOfBathrooms(numberOfBathrooms+1)} disabled={numberOfBathrooms==4}>+</button>
                        </div>
                </div>
        </div>;
};