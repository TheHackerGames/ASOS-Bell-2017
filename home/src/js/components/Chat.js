import React from "react";


export default class ActionButtons extends React.Component {

  render() {
    return (
      <div className="textArea">
        <ul>
        {this.props.messages.map(function(name, index){
                   return <li key={ index }>{name}</li>;
                 })}
        </ul>
      </div>
    );
  }
}
