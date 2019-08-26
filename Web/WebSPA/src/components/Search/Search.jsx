import React, { Component } from "react";
import "./Search.sass";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";

class Search extends Component {
    render() {
        return (
            <div id="search-line__search-btn">
                <div id="search-line">
                    <input type="text" placeholder="Поиск"/>
                </div>
                <div id="search-btn">
                    <button>
                        <FontAwesomeIcon icon={ faSearch } />
                    </button>
                </div>
            </div>
        )
    }
}

export default Search;