import React, { Component } from 'react'
import "./Header.css"

export class Header extends Component {

    constructor(props, context) {
        super(props, context);
        this.state = {
            loginState: false,
            username: '请登录'
        }
    }

    render() {
        return (
            <div className="background">
                <div className="item">专辑</div>
                <div className="item">MV</div>
                <div className="item">咨询</div>
                <div className="item">演唱会视频</div>
                <div className="item">
                    <div className="login">
                        <img className="logo" src="/static/header/logo.png"></img>
                        <div className="text">{this.state.username}</div>
                    </div>
                </div>
            </div>
        );
    }
}