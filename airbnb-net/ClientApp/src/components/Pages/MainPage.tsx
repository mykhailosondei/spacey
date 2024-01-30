import NavBar from "../NavBar";
import {PropertyTypeSlider} from "../PropertyTypeSlider";
import ListingHolder from "../ListingHolder";
import {SwitchToUser} from "./SwitchToUser";

export const MainPage = () => {
    return <>
        <SwitchToUser>
            <NavBar/>
            <PropertyTypeSlider/>
            <ListingHolder/>
        </SwitchToUser>
    </>
}