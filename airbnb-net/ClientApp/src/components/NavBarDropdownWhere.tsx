import {useEffect, useMemo, useState} from "react";
import {AutocompleteService} from "../services/AutocompleteService";
import PinPoint from "./Icons/PinPoint";

interface NavBarDropdownWhereProps {
    whereQuery: string;
    active: boolean;
    setWhereQuery: (query: string) => void;
    className?: string;
}

export const NavBarDropdownWhere = (props: NavBarDropdownWhereProps) => {
    
    const [results, setResults] = useState<string[]>([]);
    const autocompleteService = useMemo(() => AutocompleteService.getInstance(), []);
    const [isMounted, setIsMounted] = useState<boolean>(props.active);
    
    useEffect(() => {
        if(props.whereQuery.length < 3){
            setResults([]);
            return;
        } 
        if(props.whereQuery.match(/^ *$/) !== null) {
            setResults([]);
            return;
        }
        autocompleteService.getAutocompleteData(props.whereQuery, 10).then((response) => {
            const firstFiveDifferent = response.data.filter((value, index, self) => self.indexOf(value) === index).slice(0, 5);
            setResults(firstFiveDifferent);
        });
    }, [props.whereQuery]);

    useEffect(() => {
        setIsMounted(props.active && results.length > 0);
    }, [props.active, results]);
    
    return isMounted ? <div className={"nav-dropdown " + (props.className ? props.className : "")}>
        <div className="addresses">
            {results.map((result, index) => {
               return <div key={index} className="address" onClick={() => {
                   props.setWhereQuery(result);
                   setIsMounted(false);
               }}><div className={"address-icon"}><PinPoint></PinPoint></div> {result}</div>
            })}
        </div>
    </div> : <></>;
};