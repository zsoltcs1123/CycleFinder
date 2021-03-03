import React, { FunctionComponent } from 'react';
import { ListGroup } from 'react-bootstrap';
import { Symbol } from '../types/Symbol'

type SymbolListProps = {
    symbols: Symbol[]
}

const SymbolList: FunctionComponent<SymbolListProps> = ({ symbols }) =>
    <ul className="list-group">
        {symbols.map(symbol => (
            <li key={symbol.Name} className={"list-group-item list-group-item-primary"}>
                {symbol.Name}
            </li>
        ))}
    </ul>

export default SymbolList