import React from "react";


export default class Video extends React.Component {

  render() {
    return (
      <div className="videoCon"><img src={this.props.image}/></div>
    );
  }
}
