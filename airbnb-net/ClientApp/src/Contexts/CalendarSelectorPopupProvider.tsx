import React from "react";

const CalendarSelectorPopupContext = React.createContext<{isOpen: boolean, setIsOpen: React.Dispatch<React.SetStateAction<boolean>>}>({} as any);

export function useCalendarSelectorPopup() {
    const context = React.useContext(CalendarSelectorPopupContext);
    if (context === undefined) {
        throw new Error("useCalendarSelectorPopup must be used within a CalendarSelectorPopupProvider");
    }
    
    return context;
}

const CalendarSelectorPopupProvider = (props:any) => {
    const [isOpen, setIsOpen] = React.useState<boolean>(false);
    
    return <CalendarSelectorPopupContext.Provider value={{isOpen, setIsOpen}}>
        {props.children}
    </CalendarSelectorPopupContext.Provider>
}

export default CalendarSelectorPopupProvider;