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
        this.handleInputChange = this.handleInputChange.bind(this);
    }
    
    render() {
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
            const text = response.text();
            alert(text['<value>']);
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