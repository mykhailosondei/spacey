import React from "react";

const SelectedDatesContext = React.createContext<{startDate: Date | null, setStartDate: React.Dispatch<React.SetStateAction<Date | null>>, endDate: Date | null, setEndDate: React.Dispatch<React.SetStateAction<Date | null>>}>({} as any);

export function useSelectedDates() {
    const context = React.useContext(SelectedDatesContext);
    if (context === undefined) {
        throw new Error("useSelectedDates must be used within a SelectedDatesProvider");
    }
    
    return context;
}

const SelectedDatesProvider = (props:any) => {
    const [startDate, setStartDate] = React.useState<Date | null>(null);
    const [endDate, setEndDate] = React.useState<Date | null>(null);
    
    return <SelectedDatesContext.Provider value={{startDate, setStartDate, endDate, setEndDate}}>
        {props.children}
    </SelectedDatesContext.Provider>
}

export default SelectedDatesProvider;