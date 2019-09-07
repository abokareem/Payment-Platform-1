import React, { Component } from "react";
import "./Auth.sass";

class Auth extends Component {
    render() {
        return (
            <div id="Auth-text__Auth-form">
                <div id="Auth-Wrap">
                    <div id="Auth-text">
                        <h3>Авторизация</h3>
                    </div>
                    <div id="Auth-form">
                        <form action="" method="POST">
                            <input type="email" placeholder="Электронная почта" className="auth-reg-form" /> <br/>
                            <input type="password" placeholder="Пароль" className="auth-reg-form" /> <br/>
                        </form>
                        <div id="submit-reg">
                            <a href="#">Зарегистрироваться</a>
                            <button>Войти</button>
                        </div>
                    </div>

                </div>

            </div>
        )
    }
}

export default Auth;