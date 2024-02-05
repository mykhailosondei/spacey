import {useEffect, useMemo, useState} from "react";
import {AutocompleteService} from "../services/AutocompleteService";

interface NavBarDropdownWhereProps {
    whereQuery: string;
}

export const NavBarDropdownWhere = (props: NavBarDropdownWhereProps) => {
    
    const [results, setResults] = useState<string[]>([]);
    const autocompleteService = useMemo(() => AutocompleteService.getInstance(), []);

    useEffect(() => {
        if(props.whereQuery.length < 3) return;
        autocompleteService.getAutocompleteData(props.whereQuery, 5).then((response) => {
            console.log(response.data);
            setResults(response.data);
        });
    }, [props.whereQuery]);
    
    return <div className={"nav-dropdown"}>
        {results.map((result, index) => {
            return <div key={index} className="nav-dropdown-item">{result}</div>
        })}
    </div>
};