import React, { Component } from "react";
import "./Goods.sass";
import SellItem from "../../../components/Sell-item/Sell-item.jsx";

class Goods extends Component {
    render() {
        return (
            <div id="goods-wrap">
                <SellItem />
            </div>
        )
    }
}

export default Goods;