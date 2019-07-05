import React, { Component } from "react";

export class Background extends Component {

    constructor(props, context) {
        super(props, context);
        this.state = {
            backgroundUrl: undefined,
            width: props.width,
            height: props.height
        }
    }
    
    render() {
        const containerStyle = {
            width: this.state.width + 'vw', heigth: this.state.height + 'vh',
            top: (100 - this.state.height / 2) + 'vh', left: (100 - this.state.width) / 2 + 'vw'
        }

        return (
            <div>
                <div className="container" style={containerStyle}></div>
            </div>
        );
    }
}