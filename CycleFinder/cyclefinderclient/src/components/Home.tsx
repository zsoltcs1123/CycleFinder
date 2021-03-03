import React, { Component } from 'react';
import logo from '../logo.svg';
import SymbolList from './SymbolList';
import { Symbol } from '../types/Symbol';
import { Constants } from '../Constants';

type HomeState = {
    symbols: Symbol[]
}

class Home extends Component<{}, HomeState> {

    constructor(p: {}) {
        super(p);
        this.state = {
            symbols: [] as Symbol[]
        };
    }

    async setStateAsync(state: HomeState) {
        return new Promise((resolve: any) => {
            this.setState(state, resolve);
        });
    }

    async getSymbols(): Promise<Symbol[]> {
        try {
            const res = await fetch(Constants.symbolsUrl);
            console.log(JSON.stringify(res, null, '\t'));
            return await res.json() as Symbol[];
        } catch (error) {
            console.log(error);
            return [];
        }
    }

    async componentDidMount() {
        try {
            const symbols = await this.getSymbols();
            await this.setStateAsync({ symbols: symbols } as HomeState);
        } catch (error) {
        }
    }

    render() {
        return (
            <div className="App">
                <SymbolList symbols={this.state.symbols} />
            </div>
        );
    }
}

export default Home;