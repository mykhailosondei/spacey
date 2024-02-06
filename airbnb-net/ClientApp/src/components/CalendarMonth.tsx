import '../styles/CalendarMonth.scss'
import React, {useState} from "react";

interface CalendarMonthProps {
    date: Date;
    currentHoveredDate: Date | null;
    setCurrentHoveredDate: React.Dispatch<React.SetStateAction<Date | null>>;
    startDate: Date | null;
    setStartDate: React.Dispatch<React.SetStateAction<Date | null>>;
    endDate: Date | null;
    setEndDate: React.Dispatch<React.SetStateAction<Date | null>>;
    unavailableDates: Date[];
}

interface Day {
    date: Date;
    dayOfWeek: number;
}

const CalendarMonth = (props:CalendarMonthProps) => {
    
    function nextDayOfWeek(dayOfWeek:0|1|2|3|4|5|6) : 0|1|2|3|4|5|6 {
        
        return (dayOfWeek+1) % 7 as 0|1|2|3|4|5|6;
    }
    
    function daysInMonth(date : Date) : number {
        const d= new Date(date.getFullYear(), date.getMonth()+1, 0);
        return d.getDate();
    }
    
    function daysOfMonth(date: Date): Day[] {
        const _firstDayOfWeek = firstDayOfWeek(date);
        const result = [] as Day[];
        let dayOfWeekIterableValue = _firstDayOfWeek;
        let _daysInMonth = daysInMonth(date);
        for (let i = 1; i <= _daysInMonth; i++){
            result.push({date:new Date(date.getFullYear(), date.getMonth(), i), dayOfWeek:dayOfWeekIterableValue});
            dayOfWeekIterableValue = nextDayOfWeek(dayOfWeekIterableValue);
        }
        
        return result;
    }
    
    const firstDayOfWeek = (date : Date) : 0|1|2|3|4|5|6 => {
        return new Date(date.getFullYear(), date.getMonth(), 1).getDay() as 0|1|2|3|4|5|6;
    };
    
    const isDayInHighlightedRange = (day : Day) : boolean => {
        if(!props.startDate) return false;
        if (!props.endDate) {
            if(props.currentHoveredDate === null) return false;
            return day.date >= props.startDate && day.date <= props.currentHoveredDate;
        }
        
        return day.date >= props.startDate && day.date <= props.endDate;
    }
    
    const isDateUnavailable = (date : Date) : boolean => {
        return props.unavailableDates.some((unavailableDate) => {
            return unavailableDate.toDateString() === date.toDateString();
        });
    }
    
    const isSomeDateUnavailable = (range : {startDate: Date, endDate: Date}) : boolean => {
        return props.unavailableDates.some((unavailableDate) => {
            return unavailableDate >= range.startDate && unavailableDate <= range.endDate;
        });
    }
    
    const handleDayClick = (day : Day) => {
        if(isDateUnavailable(day.date)) return;
        if (!props.startDate) {
            props.setStartDate(day.date);
            return;
        }
        if (!props.endDate) {
            if (day.date < props.startDate) {
                props.setStartDate(day.date);
                return;
            }
            if(isSomeDateUnavailable({startDate: props.startDate, endDate: day.date})) return;
            props.setEndDate(day.date);
            return;
        }
        props.setStartDate(day.date);
        props.setEndDate(null);
    }
    
    const isSelectedDay = (day : Day) : " calendar-day-selected-start" | " calendar-day-selected-end" | " " => {
        if (!props.startDate) return " ";
        switch (day.date.toDateString()) {
            case props.startDate.toDateString():
                return " calendar-day-selected-start";
            case props.endDate?.toDateString():
                return " calendar-day-selected-end";
            default:
                return " ";
        }
    }
    
    const isDayUnavailable = (day : Day) : " calendar-day-unavailable" | " " => {
        switch (props.unavailableDates.some((date) => {
            return date.toDateString() === day.date.toDateString();
        })){
            case true:
                return " calendar-day-unavailable";
            default:
                return " ";
        }
    }
    
    const memoizedDays = React.useMemo(() => {
        return daysOfMonth(props.date);
    }, [props.date]);
    
    return <div className={"calendar-month"} style={{transform: "translateX(0%)"}}>
        <h3 className={"month-header"}>{ props.date.toLocaleString('default', {month:"long"})}, {props.date.getFullYear() }</h3>
        <div className="calendar-days">
            {Array.from({length:firstDayOfWeek(props.date)}, (_, index) => {
                return <div className="blank-day"></div>
            })}
            {memoizedDays.map(day =>{
                return <div className={"calendar-day" + 
                            (isDayInHighlightedRange(day) ? " calendar-day-highlighted" : " ") + 
                            (isSelectedDay(day)) +
                            (isDayUnavailable(day))} 
                            onClick={() => handleDayClick(day)}
                            onMouseEnter={() => props.setCurrentHoveredDate(day.date)}
                            onMouseLeave={() => props.setCurrentHoveredDate(null)}
                >{day.date.getDate()}
                </div>;
            })}
            <span className="dummy-day"></span>
        </div>
    </div>
};

export default CalendarMonth;