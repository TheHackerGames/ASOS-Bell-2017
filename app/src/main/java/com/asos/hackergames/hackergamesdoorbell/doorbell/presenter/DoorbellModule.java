package com.asos.hackergames.hackergamesdoorbell.doorbell.presenter;

import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;

public class DoorbellModule {

    public static DoorbellPresenter presenter(@NonNull final DoorbellView view) {
        return new DoorbellPresenter(view, interactor());
    }

    public static DoorbellInteractor interactor() {
        return new DoorbellInteractor();
    }
}
