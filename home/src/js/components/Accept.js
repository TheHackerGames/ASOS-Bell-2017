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
        <h1 className="answerTitle">Some one is at the door</h1>

        <div className="answerButton" onClick={this.acceptCall.bind(this)}>ANSWER</div>
        <div className="rejectButton" onClick={this.rejectCall.bind(this)}>REJECT</div>
        </div>
    );
  }
}
