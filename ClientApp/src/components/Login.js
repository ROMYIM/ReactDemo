import React, { Component } from 'react';
import { Route } from 'react-router';
import { Home } from "./Home";
import "./Login.css"

export class Login extends Component {
    
    constructor(props, context) {
        super(props, context);
        this.state = {
            username: '',
            password: '',
            verifyCode: '',
            codeUrl: '/user/verifycode',
            seed: new Date().getTime(),
            loginFlag: false
        }
        this.updateVerifyCodePicture = this.updateVerifyCodePicture.bind(this);
        this.userLogin = this.userLogin.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }
    
    render() {
        if (this.state.loginFlag) {
            return <Route exact path='/' component={Home} />;
        }

        return (
            <div className="container">
                <form className="login-form" onSubmit={this.userLogin} method="post">
                    <div className="form-item">
                        <div className="item-label">用户名</div>
                        <input name="username" className="item" placeholder="请输入用户名" type="text" value={this.state.username} onChange={this.handleInputChange} required />
                    </div>
                    <div className="form-item">
                        <div className="item-label">密码</div>
                        <input name="password" className="item" placeholder="请输入密码" type="password" value={this.state.password} onChange={this.handleInputChange} required />
                    </div>
                    <div className="form-item">
                        <div className="item-label">验证码</div>
                        <input name="verifyCode" className="item code" placeholder="请输入验证码" type="text" value={this.state.verifyCode} onChange={this.handleInputChange} required />
                        <img className="item" src={this.state.codeUrl + '?seed=' + this.state.seed} alt="点击切换" onClick={this.updateVerifyCodePicture} />
                    </div>
                    <div className="form-item">
                        <div className="item-label"></div>
                        <button className="item" type="submit">登录</button>
                    </div>
                </form>
            </div>
        );
    }

    userLogin (e) {
        e.preventDefault();
        const _this = this;
        const formData = new FormData();
        formData.append('username', this.state.username);
        formData.append('password', this.state.password);
        formData.append('verifycode', this.state.verifyCode);
        fetch('/user/login', {
            method: 'post',
            body: formData,
            credentials: "same-origin",
            // headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).then(response => {
            console.log(_this)
            const promise = response.text();        
            promise.then(value => {
                if (value == '登录成功') {
                    alert(value);
                    _this.setState({
                        loginFlag: true
                    })
                }
            }, reason => {
                alert(reason);
                _this.updateVerifyCodePicture()
            });
        }).catch(data => alert(data))
    }

    updateVerifyCodePicture (e) {
        this.setState({
            seed: new Date().getTime()
        })
    }

    handleInputChange (e) {
        const name = e.target.name;
        const value = e.target.value;
        this.setState({
            [name]: value
        })
    }
}

// Login.contextType = {
//     router: Route
// }