package com.asos.hackergames.hackergamesdoorbell.doorbell.view;

public interface DoorbellView {

    void displayMessage(String s);

    void requestSpeech();

    void takePicture();

    void speakMessage(String message);

    void onDoorOpened();

    void onRejected();
}
