import {CalendarSelector} from "./CalendarSelector";
import ListingDTO from "../DTOs/Listing/ListingDTO";

interface NavBarDropdownDatesProps {
    active: boolean;
}

export const NavBarDropdownDates = (props: NavBarDropdownDatesProps) => {
    return props.active? <div className={"nav-dropdown right-40 nav-dates w-full"}>
        <CalendarSelector ></CalendarSelector>
    </div>: <></>;
};