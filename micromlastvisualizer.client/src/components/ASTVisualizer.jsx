import React, { Component } from 'react';
import * as d3 from 'd3';
import './ASTVisualizer.css';

export class ASTVisualizer extends Component {
  constructor(props) {
    super(props);
    this.state = {
      code: 'let x = 5 in x + 3',
      ast: null,
      error: null,
      loading: false
    };
    this.svgRef = React.createRef();
  }

  componentDidMount() {
    this.parseCode();
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.state.ast !== prevState.ast) {
      this.renderAST();
    }
  }

  parseCode = async () => {
    this.setState({ loading: true, error: null });

    try {
      const response = await fetch('api/AST', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ code: this.state.code })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to parse code');
      }

      const data = await response.json();
      this.setState({ ast: data, loading: false });
    } catch (error) {
      this.setState({ error: error.message, loading: false });
    }
  };

  handleCodeChange = (event) => {
    this.setState({ code: event.target.value });
  };

  transformData = (node) => {
    let d3Node = {
      name: node.nodeType,
      type: node.nodeType
    };

    if (node.nodeType === 'Number') {
      d3Node.value = node.value;
    } else if (node.nodeType === 'Variable') {
      d3Node.name = node.name;
    } else if (node.nodeType === 'BinaryOp') {
      d3Node.operator = node.operator;
      d3Node.children = [
        this.transformData(node.left),
        this.transformData(node.right)
      ];
    } else if (node.nodeType === 'Function') {
      d3Node.param = node.parameterName;
      d3Node.children = [this.transformData(node.body)];
    } else if (node.nodeType === 'Application') {
      d3Node.children = [
        this.transformData(node.function),
        this.transformData(node.argument)
      ];
    } else if (node.nodeType === 'Let') {
      d3Node.variable = node.variableName;
      d3Node.children = [
        this.transformData(node.value),
        this.transformData(node.inExpression)
      ];
    } else if (node.nodeType === 'If') {
      d3Node.children = [
        this.transformData(node.condition),
        this.transformData(node.thenBranch),
        this.transformData(node.elseBranch)
      ];
    }

    return d3Node;
  };

  renderAST = () => {
    const svgContainer = this.svgRef.current;
    if (!svgContainer || !this.state.ast) return;

    // Clear previous visualization
    d3.select(svgContainer).selectAll('*').remove();

    // Transform AST data for D3.js
    const root = this.transformData(this.state.ast);

    // Set dimensions
    const width = svgContainer.clientWidth;
    const height = 500;
    const margin = { top: 20, right: 90, bottom: 30, left: 90 };

    // Create SVG
    const svg = d3.select(svgContainer)
      .append('svg')
      .attr('width', width)
      .attr('height', height)
      .append('g')
      .attr('transform', `translate(${margin.left},${margin.top})`);

    // Create tree layout
    const treeLayout = d3.tree().size([height - margin.top - margin.bottom, width - margin.left - margin.right]);

    // Create hierarchy
    const hierarchy = d3.hierarchy(root);

    // Generate tree data
    const treeData = treeLayout(hierarchy);

    // Add links
    svg.selectAll('.link')
      .data(treeData.links())
      .enter()
      .append('path')
      .attr('class', 'link')
      .attr('d', d3.linkHorizontal()
        .x(d => d.y)
        .y(d => d.x));

    // Add nodes
    const nodes = svg.selectAll('.node')
      .data(treeData.descendants())
      .enter()
      .append('g')
      .attr('class', d => `node ${d.children ? 'node--internal' : 'node--leaf'}`)
      .attr('transform', d => `translate(${d.y},${d.x})`);

    // Add node circles
    nodes.append('circle')
      .attr('r', 10)
      .style('fill', d => {
        switch (d.data.type) {
          case 'Function': return '#ff8c00';
          case 'Application': return '#9932cc';
          case 'Let': return '#3cb371';
          case 'BinaryOp': return '#4682b4';
          case 'If': return '#dc143c';
          case 'Variable': return '#1e90ff';
          default: return '#ffd700';
        }
      });

    // Add node labels
    nodes.append('text')
      .attr('dy', 4)
      .attr('x', d => d.children ? -13 : 13)
      .style('text-anchor', d => d.children ? 'end' : 'start')
      .text(d => {
        if (d.data.type === 'Number') {
          return `${d.data.type} (${d.data.value})`;
        } else if (d.data.type === 'Variable') {
          return `${d.data.type} (${d.data.name})`;
        } else if (d.data.type === 'BinaryOp') {
          return `${d.data.type} (${d.data.operator})`;
        } else if (d.data.type === 'Function') {
          return `${d.data.type} (${d.data.param})`;
        } else if (d.data.type === 'Let') {
          return `${d.data.type} (${d.data.variable})`;
        } else {
          return d.data.type;
        }
      });
  };

  render() {
    return (
      <div className="ast-visualizer">
        <div className="code-input">
          <textarea
            value={this.state.code}
            onChange={this.handleCodeChange}
            rows={5}
            placeholder="Enter MicroML code here..."
          />
          <button onClick={this.parseCode} disabled={this.state.loading}>
            {this.state.loading ? 'Parsing...' : 'Parse Code'}
          </button>
        </div>

        {this.state.error && (
          <div className="error-message">
            Error: {this.state.error}
          </div>
        )}

        <div className="ast-container">
          <div className="svg-container" ref={this.svgRef}></div>
        </div>

        <div className="examples">
          <h3>Example MicroML Code:</h3>
          <div className="example-buttons">
            <button onClick={() => this.setState({ code: 'let x = 5 in x + 3' }, this.parseCode)}>
              Let Expression
            </button>
            <button onClick={() => this.setState({ code: 'fun x -> x * x' }, this.parseCode)}>
              Function
            </button>
            <button onClick={() => this.setState({ code: 'if 5 > 3 then 10 else 20' }, this.parseCode)}>
              If-Then-Else
            </button>
            <button onClick={() => this.setState({ code: 'let add = fun x -> fun y -> x + y in add 5 3' }, this.parseCode)}>
              Higher-Order Function
            </button>
          </div>
        </div>
      </div>
    );
  }
}