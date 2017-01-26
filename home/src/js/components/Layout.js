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
      messages:[
        {
          "name":"message you",
          "message":"Hello who is it"
        }
      ]
    };

    $.connection.hub.url = "http://hackergameshubazureapi.azurewebsites.net/signalr";
    this.chat = $.connection.homeHub;

    this.chat.client.bellPressed = function (message, imageId) {
          this.changeImage(imageId);
          this.setState({callState:1});

        }.bind(this);


        this.chat.client.messageSent = function (message) {
          this.updateMessage(message,"guest");
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
    this.setState({callState:0});
    this.chat.server.openDoor();
    setTimeout(function() {
      this.changeImage("img/locked.jpg");
    }.bind(this), 5000);

  }
  rejectCall(){
    this.setState({callState:0});
    this.chat.server.end();
    this.tearDown();

  }
  tearDown(){
    this.setState({
      image: "img/locked.jpg",
      callState : 0,
      messages:[
        {
          "name":"message you",
          "message":"Hello who is it"
        }
      ]
    });
    console.log(this.state)
  }

  acceptCall(){
    $.connection.hub.start().done(function () {
         this.chat.server.acceptHome("Accepted");
         this.setState({callState:2})
         this.changeImage("img/delivery.jpg");

      }.bind(this));

  }


  updateMessage(message,type){
    var messageArray = this.state.messages;
    messageArray.push(  {
        "name":"message "+type,
        "message": message
      });

    this.setState({messages:messageArray});
    if(type==="you"){
      this.chat.server.respondMessage(message);
    }
  }

  render() {
    return (
      <div>
        <Video image={this.state.image}/>
        { this.state.callState === 1 ? <Accept acceptCall={this.acceptCall.bind(this)} rejectCall={this.rejectCall.bind(this)} />: null }
        { this.state.callState === 2 ? <Chat updateMessage={this.updateMessage.bind(this)} messages={this.state.messages} />: null }

        <ActionButtons rejectCall={this.rejectCall.bind(this)} openDoor={this.openDoor.bind(this)}/>




      </div>
    );
  }
}
