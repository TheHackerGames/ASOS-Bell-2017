import React from "react";


export default class Accept extends React.Component {
  acceptCall(e){
    this.props.acceptCall();
  }


  render() {
    return (
        <div className="answerButton" onClick={this.acceptCall.bind(this)}>ANSWER</div>
    );
  }
}
