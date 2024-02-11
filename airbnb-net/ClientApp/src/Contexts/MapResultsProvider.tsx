import {createContext, useContext, useState} from "react";

const MapResultsContext = createContext<{
    mapBounds: { x1: number, y1: number, x2: number, y2: number },
    setMapBounds: Function,
    pushPins: {location: {latitude: number, longitude: number}, price: string}[],
    setPushPins: Function}>
    ({
        mapBounds: { x1: 0, y1: 0, x2: 0, y2: 0 },
        setMapBounds: () => {}, 
        pushPins: [], 
        setPushPins: () => {} });

export function useMapResults() {
    const context = useContext(MapResultsContext);
    if (context === undefined) {
        throw new Error("useMapResults must be used within a MapResultsProvider");
    }
    return context;
}

export function MapResultsProvider(props: any) {
    const [mapBounds, setMapBounds] = useState<{ x1: number, y1: number, x2: number, y2: number }>({ x1: 0, y1: 0, x2: 0, y2: 0 });
    const [pushPins, setPushPins] = useState<{location: {latitude: number, longitude: number}, price: string}[]>([]);
    
    const value = {
        mapBounds: mapBounds,
        setMapBounds: setMapBounds,
        pushPins: pushPins,
        setPushPins: setPushPins
    };

    return (
        <MapResultsContext.Provider value={value}>
            {props.children}
        </MapResultsContext.Provider>
    );
}