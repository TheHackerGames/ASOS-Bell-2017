import React from "react";


export default class Video extends React.Component {

  render() {
    return (
      <div className="videoCon"><img src={this.props.image}/><div className={this.props.knownUser ? "userScreen knownUser": "userScreen unknownUser" }><img src="img/tick.png"/></div></div>
    );
  }
}
