import React from "react";


export default class ActionButtons extends React.Component {
  openDoor(e){
    this.props.openDoor();
  }

  rejectCall(e){
    this.props.rejectCall();
  }


  render() {
    return (
      <div className="actionButtons">
        <div className="button reject" onClick={this.rejectCall.bind(this)}>LOCK</div>
        <div className="button approve" onClick={this.openDoor.bind(this)}>UNLOCK</div>

      </div>
    );
  }
}
