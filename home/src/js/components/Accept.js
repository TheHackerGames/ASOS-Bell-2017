import React from "react";


export default class Accept extends React.Component {
  acceptCall(e){
    this.props.acceptCall();
  }
  rejectCall(e){
    this.props.rejectCall();
  }


  render() {
    return (
      <div>
        <div className="answerButton" onClick={this.acceptCall.bind(this)}>ANSWER</div>
        <div className="rejectButton" onClick={this.rejectCall.bind(this)}>REJECT</div>
        </div>
    );
  }
}
