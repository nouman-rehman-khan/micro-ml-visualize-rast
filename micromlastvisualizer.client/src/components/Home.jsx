import React, { Component } from 'react';
import { ASTVisualizer } from './ASTVisualizer';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>MicroML AST Visualizer</h1>
        <p>This application visualizes Abstract Syntax Trees for the MicroML language.</p>
        <p>Enter some MicroML code below to see its AST:</p>
        <ASTVisualizer />
      </div>
    );
  }
}