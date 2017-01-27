package com.asos.hackergames.hackergamesdoorbell.doorbell.presenter;

import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;

public class DoorbellPresenter {

    private final DoorbellView view;

    public DoorbellPresenter(@NonNull final DoorbellView view) {
        this.view = view;
    }

    public void pushDoorBell() {
        view.takePicture();
    }
}
