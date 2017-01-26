import React from "react";


export default class Video extends React.Component {
  handleChange(e){
    const image = e.target.value;
    this.props.changeImage(image);
  }
  render() {
    return (
      <div className="videoCon"><img src={this.props.image}/></div>
    );
  }
}
