import {useLocation} from "react-router-dom";
import NavBar from "../NavBar";
import {convertDateToUTC} from "./BookingPage";
import {PropertyTypeSlider} from "../PropertyTypeSlider";
import GeneralListingHolder from "../GeneralListingHolder";


export const SearchPage = () => {
    
    const query = new URLSearchParams(useLocation().search);
    
    const where = query.get("where");
    const checkIn = query.get("checkIn");
    const checkOut = query.get("checkOut");
    const guests = query.get("guests");
    const propertyType = query.get("propertyType");
    
    
    const whereText = ()=> {
        console.log(where);
        return (where === "" || !where) ? "Anywhere" : where!;
    };
    
    const getMonthName = (date: string)=>{
        return convertDateToUTC(new Date(date)).toLocaleDateString('default', {month: 'short'});
    }
    
    const datesText = ()=>{
        if (checkIn && checkOut) {
            if(convertDateToUTC(new Date(checkIn)).getMonth() === convertDateToUTC(new Date(checkOut)).getMonth()){
                const month = getMonthName(checkIn);
                return `${month} ${convertDateToUTC(new Date(checkIn)).getDate()} - ${convertDateToUTC(new Date(checkOut)).getDate()}`;
            }
            return `${getMonthName(checkIn)} ${convertDateToUTC(new Date(checkIn)).getDate()} - ${getMonthName(checkOut)} ${convertDateToUTC(new Date(checkOut)).getDate()}`;
        }
        return "Any week";
    };

    function formatGuests(guests: string | null) {
        let guestsNumber = guests ? parseInt(guests) : 0;
        if (guestsNumber === 0) {
            return "Add guests";
        }
        if (guestsNumber === 1) {
            return "1 guest";
        }
        return guestsNumber + " guests";
    }
    
    return <div className="search-page">
        <NavBar searchMode={{where:whereText(), dates:datesText(), guests: formatGuests(guests)}}></NavBar>
        <PropertyTypeSlider></PropertyTypeSlider>
        <GeneralListingHolder searchConfig={{place: where ?? undefined, checkIn: checkIn ?? undefined, checkOut: checkOut?? undefined, propertyType: propertyType ?? undefined, guests: guests ?? undefined}}></GeneralListingHolder>
    </div>;
};