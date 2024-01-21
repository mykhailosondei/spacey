import React from 'react';
import './App.scss';
import {Route, Routes} from "react-router-dom";
import {UserProvider} from "./Contexts/UserContext";
import {PopupProvider} from "./Contexts/PopupContext";
import {HostingPage} from "./components/Pages/HostingPage";
import {MainPage} from "./components/Pages/MainPage";
import {AuthStateProvider} from "./Contexts/AuthStateProvider";
import {HostProvider} from "./Contexts/HostContext";
import {RequireAuth} from "./components/RequireAuth";
import {LoginPage} from "./components/Pages/LoginPage";
import {ListingPage} from "./components/Pages/ListingPage";
import ListingDTO from "./DTOs/Listing/ListingDTO";
import {PropertyType} from "./values/PropertyType";
import {HostDTO} from "./DTOs/Host/HostDTO";

function App() {

    const placeholderListing: ListingDTO = {
        id: "1",
        title: "Luxurious Beachfront Villa",
        description: "🍃 PORTAGE PADDLE CAMP - LAST WEEKEND OF THE SEASON!\n" +
            "\n" +
            "SPRING - FALL ONLY\n" +
            "\n" +
            "Experience the essence of primitive camping on the serene banks of the Mississippi River at Portage Paddle Camp. Our vintage 1968 Shasta camper offers simplicity and tranquility, nestled on private acres. Just 45 minutes from St. Louis and 20 minutes from St. Charles, it's the perfect escape.\n" +
            "\n" +
            "The Space:\n" +
            "This is not glamping; it's a step back in time. Enjoy a full-size memory foam bed inside the camper and an additional twin bed upon request. Outdoor chairs provided. No consumable water or electric inside the camper for a genuine back-to-basics experience.\n" +
            "\n" +
            "Bathroom & Shower:\n" +
            "Outdoor bathroom on the property with an invigorating hot water shower.\n" +
            "\n" +
            "Guest Access:\n" +
            "Complimentary use of kayaks or paddleboards, shared among guests.\n" +
            "\n" +
            "Other Considerations:\n" +
            "\n" +
            "    Weekends and holidays may be lively; opt for weekdays for a quieter experience.\n" +
            "    Nearest gas station and grocery store are 15 minutes away, plan accordingly.\n" +
            "    Essentials for summer: bug spray, citronella candles, sunscreen, towels, shower items, bottled water.\n" +
            "\n" +
            "Important Points:\n" +
            "\n" +
            "    Stove, refrigerator, and electric hookups inside the camper are non-operational.\n" +
            "    Bugs and mosquitoes are part of the natural surroundings.\n" +
            "    Vintage doesn't mean unclean; our space is well-maintained.\n" +
            "\n" +
            "Join us for the final weekend of the season at Portage Paddle Camp and savor the simplicity of nature!",
        address: {
            street: "123 Ocean Drive",
            city: "Paradise City",
            country: "Dreamland",
        },
        propertyType: PropertyType.Bungalow,
        latitude: 37.7749,
        longitude: -122.4194,
        pricePerNight: 500,
        numberOfRooms: 4,
        numberOfBathrooms: 5,
        numberOfGuests: 8,
        imagesUrls: [
            {url: "https://placehold.co/1080x1080", id:"123fsafa"},
            {url: "https://placehold.co/1080x1080", id:"123fsafa"},
            {url: "https://placehold.co/1080x1080", id:"123fsafa"},
            {url: "https://placehold.co/1080x1080", id:"123fsafa"},
            {url: "https://placehold.co/1080x1080", id:"123fsafa"}
        ],
        host: {
            id: "2ace09b0-893b-4f5a-8aa8-bdbe90c5b576",
            userId: "b4865752-d500-4d80-86dc-62bd34aee7ce"
        } as HostDTO,
        bookingsIds: ["booking1", "booking2"],
        amenities: ["Pool", "Gym", "FreeParking", "Wifi", "AirConditioning"],
        likedUsersIds: ["user1", "user2", "user3"],
    };
    
  return (
      <>
    <div className="App">
        <AuthStateProvider>
            <UserProvider>
            <PopupProvider>
                <Routes>
                    <Route path={"/login"} element={<LoginPage></LoginPage>}/>
                    <Route path="/hosting/*" element={<HostProvider><RequireAuth><HostingPage/></RequireAuth></HostProvider>}/>
                    <Route path="/" element={<MainPage/>}/>
                    <Route path="/:propertyType" element={<MainPage/>}/>
                    <Route path="/listing/address" element={<MainPage/>}></Route>
                    <Route path="/listing/boundingBox" element={<MainPage/>}></Route>
                    <Route path="/listing/:id" element={<ListingPage listing={placeholderListing}/>}></Route>
                </Routes>
            </PopupProvider>
            </UserProvider>
        </AuthStateProvider>
    </div>
      </>
  );
}

export default App;
