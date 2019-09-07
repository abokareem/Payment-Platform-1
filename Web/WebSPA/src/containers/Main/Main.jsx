import React, { Component } from "react";
import "./Main.sass";
// import Goods from "../Pages/Goods/Goods.jsx";
import Auth from "../Pages/Auth/Auth.jsx";

class Main extends Component {
    render() {
        return (
            <div id="Main">
                <div className="layout">
                    {/*<Goods />*/}
                    <Auth />
                </div>
            </div>
        )
    }
}

export default Main;