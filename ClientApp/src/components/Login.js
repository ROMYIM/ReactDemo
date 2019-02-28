import React, { Component } from 'react';
import "./Login.css"

export class Login extends Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: '',
            verifyCode: '',
            codeUrl: '/user/verifycode',
            seed: new Date().getTime()
        }
        this.updateVerifyCodePicture = this.updateVerifyCodePicture.bind(this);
        this.userLogin = this.userLogin.bind(this);
    }
    
    render() {
        return (
            <div className="container" onSubmit={this.userLogin}>
                <form className="login-form" >
                    <div className="form-item">
                        <div className="item-label">用户名</div>
                        <input className="item" placeholder="请输入用户名" value={this.state.username} required />
                    </div>
                    <div className="form-item">
                        <div className="item-label">密码</div>
                        <input className="item" placeholder="请输入密码" type="password" value={this.state.password} required />
                    </div>
                    <div className="form-item">
                        <div className="item-label">验证码</div>
                        <input className="item code" placeholder="请输入验证码" type="text" value={this.state.verifyCode} required />
                        <img className="item" src={this.state.codeUrl + '?seed=' + this.state.seed} alt="点击切换" onClick={this.updateVerifyCodePicture} />
                    </div>
                    <div className="form-item">
                        <div className="item-label"></div>
                        <button className="item">登录</button>
                    </div>
                </form>
            </div>
        );
    }

    userLogin (e) {
        console.log(e);

    }

    updateVerifyCodePicture (e) {
        this.setState({
            seed: new Date().getTime()
        })
    }
}