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
    const guests = parseInt(query.get("guests")??"1");
    
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

    function formatGuests(guests: number) {
        if (guests === 0) {
            return "";
        }
        if (guests === 1) {
            return "1 guest";
        }
        return guests + " guests";
    }
    
    return <div className="search-page">
        <NavBar searchMode={{where:whereText(), dates:datesText(), guests: formatGuests(guests)}}></NavBar>
        <GeneralListingHolder></GeneralListingHolder>
    </div>;
};