import React from "react";


export default class ActionButtons extends React.Component {




  updateMessage(e){
    if(e.nativeEvent.key == "Enter"){
      this.props.updateMessage( e.target.value,"you");
    }

  }
  render() {
    return (
      <div>
      <div className="chatArea">
        {this.props.messages.map(function(user, index){
                   return <div className={user.name} key={ index }>{user.message}</div>;
                 })}
      </div>
      <div className="chatSend">

            <input className="chatInput" type="text"  onKeyPress={this.updateMessage.bind(this)} />

      </div>
      </div>
    );
  }
}
