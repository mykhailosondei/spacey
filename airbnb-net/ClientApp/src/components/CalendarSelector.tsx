import React, {useEffect, useMemo} from "react";
import "./../styles/CalendarSelector.scss";
import CalendarMonth from "./CalendarMonth";
import {log} from "node:util";
import ListingDTO from "../DTOs/Listing/ListingDTO";
import {ListingService} from "../services/ListingService";
import CalendarLeftArrow from "./Icons/LeftArrow";
import {useSelectedDates} from "../Contexts/SelectedDatesProvider";

interface CalendarSelectorProps {
    listing: ListingDTO;
}

export const CalendarSelector = (props:CalendarSelectorProps) => {
    
    const calendarSelectorRef = React.useRef<HTMLDivElement>(null);
    
    const {startDate, setStartDate, endDate, setEndDate} = useSelectedDates();
    
    const listingService = useMemo(() => {return  ListingService.getInstance()}, []);
    
    const [unavailableDates, setUnavailableDates] = React.useState<Date[]>([]);
    
    useEffect(() => {
        listingService.getUnavailableDates(props.listing.id).then((dates) => {
            const datesArray = [] as Date[];
            for (let i = 0; i < dates.length; i++) {
                datesArray.push(new Date(dates[i]));
            }
            setUnavailableDates(datesArray);
        });
    }, []);
    
    const [currentHoveredDate, setCurrentHoveredDate] = React.useState<Date | null>(null);
    
    const [currentMonth, setCurrentMonth] = React.useState<Date>(new Date(2024,0,1));
    
    const previousMonth = (date: Date) => {
        const isJanuary = date.getMonth() === 0;
        if (isJanuary) {
            return new Date(date.getFullYear()-1, 11, 1);
        }
        return new Date(date.getFullYear(), date.getMonth()-1, 1);
    }
    
    const nextMonth = (date: Date) => {
        const isDecember = date.getMonth() === 11;
        if (isDecember) {
            return new Date(date.getFullYear()+1, 0, 1);
        }
        return new Date(date.getFullYear(), date.getMonth()+1, 1);
    }
    
    let transitionInProgress = false;
    
    async function moveCalendarLeft() {
        if (transitionInProgress) {
            return;
        }
        const months = calendarSelectorRef.current!.getElementsByClassName("calendar-month");
        for (let i = 0; i < months.length; i++) {
            const month = months[i];
            moveMonthX(month as HTMLElement, 100);
        }
        transitionInProgress = true;
        setTimeout(() => {
            transitionInProgress = false;
        }, 220);
        setTimeout(() => {
            for (let i = 0; i < months.length; i++) {
                const month = months[i];
                moveMonthX(month as HTMLElement, -100, false);
            }
            setCurrentMonth(previousMonth(currentMonth));
        }, 220);
    }
    
    function moveCalendarRight() {
        if (transitionInProgress) {
            return;
        }
        const months = calendarSelectorRef.current!.getElementsByClassName("calendar-month");
        for (let i = 0; i < months.length; i++) {
            const month = months[i];
            moveMonthX(month as HTMLElement, -100);
        }
        transitionInProgress = true;
        setTimeout(() => {
            transitionInProgress = false;
        }, 220);
        setTimeout(() => {
            for (let i = 0; i < months.length; i++) {
                const month = months[i];
                moveMonthX(month as HTMLElement, 100, false);
            }
            setCurrentMonth(nextMonth(currentMonth));
        }, 220);
    }
    
    function moveMonthX(month: HTMLElement, percentage:number, withTransition = true) {
        const currentTransform = window.getComputedStyle(month).transform;
        const currentWidth = month.getBoundingClientRect().width;
        const transformX = new DOMMatrix(currentTransform).m41;
        const currentTransformPercentage = transformX / currentWidth * 100;
        if(withTransition){
            month.style.transition = "transform 0.2s ease-out";
        }else {
            month.style.transition = "none";
        }
        month.style.transform = `translateX(${currentTransformPercentage + percentage}%)`;
    }
    
    
    return <div className={"calendar-selector-holder"}>
        <div className="calendar-selector" ref={calendarSelectorRef}>
            <div className="month-header-and-day-cells_dynamic">
                <div className="moving-container">
                    <CalendarMonth date={previousMonth(currentMonth)} startDate={startDate} endDate={endDate}
                                   currentHoveredDate={currentHoveredDate} setCurrentHoveredDate={setCurrentHoveredDate}
                                   setEndDate={setEndDate} setStartDate={setStartDate}
                                   unavailableDates={unavailableDates}></CalendarMonth>
                    <CalendarMonth date={currentMonth} startDate={startDate} endDate={endDate}
                                   currentHoveredDate={currentHoveredDate} setCurrentHoveredDate={setCurrentHoveredDate}
                                   setEndDate={setEndDate} setStartDate={setStartDate}
                                   unavailableDates={unavailableDates}></CalendarMonth>
                    <CalendarMonth date={nextMonth(currentMonth)} startDate={startDate} endDate={endDate}
                                   currentHoveredDate={currentHoveredDate} setCurrentHoveredDate={setCurrentHoveredDate}
                                   setEndDate={setEndDate} setStartDate={setStartDate}
                                   unavailableDates={unavailableDates}></CalendarMonth>
                    <CalendarMonth date={nextMonth(nextMonth(currentMonth))} startDate={startDate} endDate={endDate}
                                   currentHoveredDate={currentHoveredDate} setCurrentHoveredDate={setCurrentHoveredDate}
                                   setEndDate={setEndDate} setStartDate={setStartDate}
                                   unavailableDates={unavailableDates}></CalendarMonth>
                </div>
            </div>
            <div className="arrows-and-days-of-week_static">
                <div className="calendar-arrows">
                    <div className="left-calendar-arrow" onClick={moveCalendarLeft}>
                        <CalendarLeftArrow>
                        </CalendarLeftArrow>
                    </div>
                    <div className="right-calendar-arrow" onClick={moveCalendarRight}>
                        <CalendarLeftArrow></CalendarLeftArrow>
                    </div>
                </div>
                <div className="days-of-the-week-container">
                    <div className="days-of-week">
                        <div className="day-of-week">Su</div>
                        <div className="day-of-week">Mo</div>
                        <div className="day-of-week">Tu</div>
                        <div className="day-of-week">We</div>
                        <div className="day-of-week">Th</div>
                        <div className="day-of-week">Fr</div>
                        <div className="day-of-week">Sa</div>
                    </div>
                    <div className="days-of-week">
                        <div className="day-of-week">Su</div>
                        <div className="day-of-week">Mo</div>
                        <div className="day-of-week">Tu</div>
                        <div className="day-of-week">We</div>
                        <div className="day-of-week">Th</div>
                        <div className="day-of-week">Fr</div>
                        <div className="day-of-week">Sa</div>
                    </div>
                </div>
            </div>
        </div>
        <div className="clear-dates-button" onClick={() => {
            setStartDate(null);
            setEndDate(null)
        }}>Clear dates
        </div>
    </div>;
}