import React, { Component } from "react";
import "./App.sass";
import Header from "./Header/Header.jsx";
import Main from "./Main/Main.jsx";

class App extends Component {
    render() {
        return (
            <>
                <Header />
                <Main />
            </>
        )
    }
}

export default App;