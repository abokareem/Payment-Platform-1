import React, { Component } from "react";
import "./Header.sass";
import Search from "../../components/Search/Search.jsx";
import img from "../../img/logo.png";

class Header extends Component {
    render() {
        return (
            <div id="header">
                <div className="layout">
                    <div id="logo">
                        <img src={ img } alt="secure" />
                    </div>
                    <Search />
                    <div id="sell-btn">
                        <button>продать</button>
                    </div>
                    <div id="auth-link">
                        <a href="#">Войти</a>
                    </div>
                </div>
            </div>
        )
    }
}

export default Header;