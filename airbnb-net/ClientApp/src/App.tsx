import React from 'react';
import pic from './Pictures/img1.webp';
import pic2 from './Pictures/img2.webp';
import './App.scss';
import ListingBox from "./ListingBox";
import NavBar from "./NavBar";

function App() {
  return (
      <>
        {/*<ListingBox title={"1"} distance={"1"} availability={"1"} price={12} rating={1} pictures={[pic, pic2]}></ListingBox>*/}
    <div className="App">
        <NavBar />
    </div>
      </>
  );
}

export default App;
