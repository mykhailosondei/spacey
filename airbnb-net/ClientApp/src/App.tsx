import React from 'react';
import './App.scss';
import {Route, Routes} from "react-router-dom";
import NavBar from "./components/NavBar";
import ListingHolder from "./components/ListingHolder";
import {PropertyTypeSlider} from "./components/PropertyTypeSlider";
import {AuthProvider} from "./Contexts/AuthContext";
import LoginPopup from "./components/LoginPopup";
import {LoginPopupProvider} from "./Contexts/LoginPopupContext";

function App() {
    
  return (
      <>
    <div className="App">
        <LoginPopupProvider><LoginPopup></LoginPopup>
        <AuthProvider>
            <NavBar/>
            <PropertyTypeSlider></PropertyTypeSlider>
            <Routes>
                <Route path="/" element={<ListingHolder/>}/>
                <Route path="/:propertyType" element={<ListingHolder/>}/>
                <Route path="/listing/address" element={<ListingHolder/>}></Route>
                <Route path="/listing/boundingBox" element={<ListingHolder/>}></Route>
            </Routes>
        </AuthProvider>
        </LoginPopupProvider>
    </div>
      </>
  );
}

export default App;
