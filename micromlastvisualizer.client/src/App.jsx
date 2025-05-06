import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import { Home } from './components/Home';
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <div className="container">
        <Routes>
          <Route path="/" element={<Home />} />
        </Routes>
      </div>
    );
  }
}