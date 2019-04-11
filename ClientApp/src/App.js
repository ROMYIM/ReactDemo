import React, { Component } from 'react';
import { Route } from 'react-router';
// import { Layout } from './components/Layout';
import { Home } from './components/Home';
// import { FetchData } from './components/FetchData';
// import { Counter } from './components/Counter';
// import { Conference } from "./components/Conference";
import { Login } from "./components/Login";

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Login>
        <Route exact path='/' component={Home} />
      </Login>
      // <Layout>
      //   <Route exact path='/' component={Home} />
      //   <Route path='/counter' component={Counter} />
      //   <Route path='/fetchdata' component={FetchData} />
      //   <Route path='/conference' component={Conference} />
      // </Layout>
    );
  }
}
