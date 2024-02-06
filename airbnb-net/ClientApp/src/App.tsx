import React, {useMemo} from 'react';
import './App.scss';
import {Route, Routes} from "react-router-dom";
import {UserProvider} from "./Contexts/UserContext";
import {PopupProvider} from "./Contexts/PopupContext";
import {HostingPage} from "./components/Pages/HostingPage";
import {MainPage} from "./components/Pages/MainPage";
import {AuthStateProvider} from "./Contexts/AuthStateProvider";
import {HostProvider} from "./Contexts/HostContext";
import {RequireSwitchToHost} from "./components/RequireSwitchToHost";
import {LoginPage} from "./components/Pages/LoginPage";
import {ListingPage} from "./components/Pages/ListingPage";
import {ListingService} from "./services/ListingService";
import SelectedDatesProvider from "./Contexts/SelectedDatesProvider";
import {BookingPage} from "./components/Pages/BookingPage";
import {TripsPage} from "./components/Pages/TripsPage";
import {RequireLogin} from "./components/RequireLogin";
import {UserProfilePage} from "./components/Pages/UserProfilePage";
import {SearchPage} from "./components/Pages/SearchPage";

function App() {

    const listingService = useMemo(() => {return ListingService.getInstance()}, []);
    
  return (
      <>
    <div className="App">
        <AuthStateProvider>
            <UserProvider>
            <PopupProvider>
                <Routes>
                    <Route path={"/login"} element={<LoginPage></LoginPage>}/>
                    <Route path="/hosting/*" element={<HostProvider><RequireSwitchToHost><HostingPage/></RequireSwitchToHost></HostProvider>}/>
                    <Route path="/*" element={<SelectedDatesProvider><MainPage/></SelectedDatesProvider>}/>
                    <Route path={"/search"} element={<SelectedDatesProvider><SearchPage/></SelectedDatesProvider>}/>
                    <Route path="/listing/:id" element={
                        <SelectedDatesProvider><ListingPage/></SelectedDatesProvider>}></Route>
                    <Route path="/booking/*" element={<BookingPage></BookingPage>}></Route>
                    <Route path="/trips/*" element={<RequireLogin><TripsPage/></RequireLogin>}></Route>
                    <Route path="/user/:id" element={<RequireLogin><UserProfilePage/></RequireLogin>}></Route>
                </Routes>
            </PopupProvider>
            </UserProvider>
        </AuthStateProvider>
    </div>
      </>
  );
}

export default App;
