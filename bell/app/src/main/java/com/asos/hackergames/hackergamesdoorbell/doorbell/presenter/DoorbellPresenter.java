package com.asos.hackergames.hackergamesdoorbell.doorbell.presenter;

import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;

public class DoorbellPresenter {

    private final DoorbellView view;
    private final DoorbellInteractor interactor;

    DoorbellPresenter(@NonNull final DoorbellView view,
                      @NonNull final DoorbellInteractor interactor) {
        this.view = view;
        this.interactor = interactor;
    }

    public void onDoorbellPressed() {
        interactor.sendDoorbellRequest();
        view.displayMessage("Doorbell pressed");
    }
}
