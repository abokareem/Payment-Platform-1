import React, { Component } from "react";
import "./App.sass";
import Header from "./Header/Header.jsx";
import Main from "./Main/Main.jsx";
import Footer from "./Footer/Footer.jsx";

class App extends Component {
    render() {
        return (
            <>
                <Header />
                <Main />
                <Footer />
            </>
        )
    }
}

export default App;