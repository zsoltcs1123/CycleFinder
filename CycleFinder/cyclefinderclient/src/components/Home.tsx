import React from 'react';
import logo from '../logo.svg';
import SymbolList from './SymbolList';

class Home extends React.Component {
    render() {
        return (
            <div className="App">
                <SymbolList />
            </div>
        );
    }
}

export default Home;