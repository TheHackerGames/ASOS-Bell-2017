import React from "react";

import ActionButtons from "./ActionButtons";
import Chat from "./Chat";
import Video from "./Video"

export default class Layout extends React.Component {
  constructor() {
    super();
    this.state = {
      image: "img/avatar.jpg",
      chat:[]
    };

    $.connection.hub.url = "http://hackergameshubazureapi.azurewebsites.net/signalr";
    var chat = $.connection.homeHub;

    chat.client.bellPressed = function (message, imageId) {
          this.changeImage("img/delivery.jpg");
        }.bind(this);
           $.connection.hub.start().done(function () {
             chat.server.registerHome();

         });


  }

  changeImage(image) {
    this.setState({image});
  }
  openDoor(){
    alert("door open");
  }
  rejectCall(){
    alert("Rejected");
  }
  sendMessage(meesage){

  }

  render() {
    return (
      <div>
        <Video changeTitle={this.changeImage.bind(this)} image={this.state.image}/>
        <div onClick={this.changeImage.bind(this, "img/delivery.jpg")}>CLICK ME </div>

        <Chat />

      </div>
    );
  }
}
