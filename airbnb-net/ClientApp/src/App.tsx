import React, {useState} from 'react';
import pic from './Pictures/img1.webp';
import pic2 from './Pictures/img2.webp';
import './App.scss';
import NavBar from "./components/NavBar";
import {wait} from "@testing-library/user-event/dist/utils";
import ListingHolder from "./components/ListingHolder";

function App() {
    const [imageUrlArray, setImageUrlArray] = useState([''])
  const getPhotosFromJson = () => {
      
  }
  
  
  return (
      <>
        {/*<ListingBox title={"1"} distance={"1"} availability={"1"} price={12} rating={1} pictures={[pic, pic2]}></ListingBox>*/}
    <div className="App">
        <NavBar />
        <ListingHolder></ListingHolder>
    </div>
      </>
  );
}

export default App;
