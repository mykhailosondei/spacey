/// <reference path="../../node_modules/bingmaps/types/MicrosoftMaps/Microsoft.Maps.d.ts" />

import {useEffect, useRef, useState} from "react";
import {useMapResults} from "../Contexts/MapResultsProvider";
import {useNavigate} from "react-router-dom";

interface BingMapsProps {
    center?: {latitude: number, longitude: number};
    zoom?: number;
}

export default function BingMaps(props: BingMapsProps) {
    console.log("BingMaps.tsx");
    
    const mapContainer = useRef(null);
    const map = useRef(null);
    
    const {pushPins} = useMapResults();
    
    const _window = window as Window & any;
    
    const constructMapOptions = () => {
        if(props.center && props.zoom) {
            return {
                center: new _window.Microsoft.Maps.Location(props.center.latitude, props.center.longitude),
                zoom: props.zoom
            }
        } else if (props.center) {
            return {
                center: new _window.Microsoft.Maps.Location(props.center.latitude, props.center.longitude),
                zoom: 8
            }
        }
        else {
            return {
                center: new _window.Microsoft.Maps.Location(0, 0),
                zoom: 2
            }
        }
    }
    
    const loadMap = () => {
        const {Maps} = _window.Microsoft;
        const map = new Maps.Map(mapContainer.current, {
            center: constructMapOptions().center,
            zoom: constructMapOptions().zoom
        });
        
        Microsoft.Maps.Events.addHandler(map, 'viewchangeend', function () {
            console.log(map.getBounds());
        });
        
        pushPins.forEach(pin => {
            map.entities.push(new Maps.Pushpin(mapsLocationFromLocation(pin.location), {enableClickedStyle: true ,text: pin.price, color:"black", icon: `<svg xmlns="http://www.w3.org/2000/svg" width="80" height="25"><rect rx="10" width="80" height="25" stroke="lightgray" stroke-width="1" fill="black" /></svg>`}));
        });
    }
    
    const mapsLocationFromLocation = (location: {longitude: number, latitude: number}) => {
        return new _window.Microsoft.Maps.Location(location.latitude, location.longitude);
    }
    

    useEffect(() => {
        _window.onerror = (e: any) => {}
        
        if(_window.Microsoft) {
            try {
                loadMap();
            }
            catch (e) {
                console.log(e);
            }
        } else {
            console.log("Loading Bing Maps");
            const script = document.createElement('script');
            script.src = `https://www.bing.com/api/maps/mapcontrol?branch=release&callback=loadMap&key=${process.env.REACT_APP_BING_MAPS_KEY}`;
            script.async = true;
            script.defer = true;
            document.body.appendChild(script);
            _window.loadMap = loadMap;
        }
    }, [loadMap]);
    return <div className={"map-container"} ref={mapContainer} style={{height: "100%", width:"100%"}}></div>
}