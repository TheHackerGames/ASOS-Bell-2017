package com.asos.hackergames.hackergamesdoorbell.doorbell.view;

public interface DoorbellView {

    void displayMessage(String s);

    void onServiceBound(final boolean bound);
}
