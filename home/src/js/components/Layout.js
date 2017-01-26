import React from "react";

import ActionButtons from "./ActionButtons";
import Chat from "./Chat";
import Video from "./Video";
import Accept from "./Accept";

export default class Layout extends React.Component {
  constructor() {
    super();
    this.state = {
      image: "img/locked.jpg",
      callState : 0,
      messages:["first message"]
    };

    $.connection.hub.url = "http://hackergameshubazureapi.azurewebsites.net/signalr";
    this.chat = $.connection.homeHub;

    this.chat.client.bellPressed = function (message, imageId) {
          this.changeImage("http://hackergameshubazureapi.azurewebsites.net/api/image/asos");
          this.setState({callState:1});

        }.bind(this);
           $.connection.hub.start().done(function () {
             this.chat.server.registerHome();

         }.bind(this));


  }

  changeImage(image) {
    this.setState({image});


  }

  openDoor(){
    this.changeImage("img/unlock.jpg");
    setTimeout(function() {
      this.changeImage("img/locked.jpg");

    }.bind(this), 5000);

  }
  rejectCall(){
    this.setState({callState:0});

  }
  acceptCall(){
    $.connection.hub.start().done(function () {
         this.chat.server.acceptHome("Accepted");
         this.setState({callState:2})
         this.changeImage("img/delivery.jpg");
      }.bind(this));
  }
  sendMessage(meesage){

  }

  render() {
    return (
      <div>
        <Video image={this.state.image}/>
        { this.state.callState === 1 ? <Accept acceptCall={this.acceptCall.bind(this)} />: null }
        { this.state.callState === 2 ? <Chat messages={this.state.messages} />: null }

        <ActionButtons rejectCall={this.rejectCall.bind(this)} openDoor={this.openDoor.bind(this)}/>




      </div>
    );
  }
}
