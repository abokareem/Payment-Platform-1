import React from "react";
import ReactDOM from 'react-dom';
import "normalize.css";
import App from "./containers/App.jsx";
import { store } from "./store/store.js";
import { Provider } from "react-redux";
import { Router } from "react-router";
import { createBrowserHistory } from "history";

const history = createBrowserHistory();

ReactDOM.render(
    <Provider store={ store }>
        <Router history={ history }>
            <App />
        </Router>
    </Provider>,
    document.getElementById("root")
);