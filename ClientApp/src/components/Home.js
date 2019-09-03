import React, { Component } from 'react';
import { Header } from "./Header";
import { Login } from "./Login";
import { globalValue } from "../Global";
import "./Home.css"

export class Home extends Component {
    displayName = Home.name

    constructor(props, context) {
        super(props, context);

        this.globalValue = globalValue;

        this.userLogout = this.userLogout.bind(this);
		this.userTest = this.userTest.bind(this);
    }
    
    userLogout(e) {
        const headers = new Headers();
        headers.append("Authorization", globalValue.Token);
        fetch('http://localhost:5000/user/logout', {
            method: 'get',
            headers: headers,
            credentials: "include"
        }).then(response => {
            console.log('response.headers.Authorization', response.headers.Authorization);
            globalValue.Token = '';
            console.log(globalValue.Token)
            const promise = response.json();
            promise.then(result => alert(result.message));
        }).catch(reason => alert(reason))
    }

	userTest(e) {
        const _this = this;
		const headers = new Headers();
        headers.append("Authorization", _this.globalValue.Token);
        fetch('http://localhost:5000/sampledata/test', {
            method: 'get',
            headers: headers,
            credentials: "include"
        }).then(response => {
            _this.globalValue.Token = response.headers.get("Authorization");
            const promise = response.json();
            promise.then(result => {
                if (result.code == -1) {
                    alert("系统异常");
                }
            });
        }).catch(reason => alert(reason))
	}

    render() {
        return (
        // <div>
        //     <Header></Header>
        // </div>
        
        // <Login></Login>
        <div>
          <h1>Hello, world!</h1>
          <p>Welcome to your new single-page application, built with:</p>
          <ul>
            <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
            <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
            <li><a href='/api/sampledata/WeatherForecasts'>Bootstrap</a> for layout and styling</li>
          </ul>
          <p>To help you get started, we've also set up:</p>
          <ul>
            <li><strong>Client-side navigation</strong>. For example, click <em>Counter</em> then <em>Back</em> to return here.</li>
            <li><strong>Development server integration</strong>. In development mode, the development server from <code>create-react-app</code> runs in the background automatically, so your client-side resources are dynamically built on demand and the page refreshes when you modify any file.</li>
            <li><strong>Efficient production builds</strong>. In production mode, development-time features are disabled, and your <code>dotnet publish</code> configuration produces minified, efficiently bundled JavaScript files.</li>
          </ul>
          <p>The <code>ClientApp</code> subdirectory is a standard React application based on the <code>create-react-app</code> template. If you open a command prompt in that directory, you can run <code>npm</code> commands such as <code>npm test</code> or <code>npm install</code>.</p>
          <p><a onClick={this.userTest}>Test</a></p>
          <p><a onClick={this.userLogout}>Sign Out</a></p>
        </div>
        );
    }
}
