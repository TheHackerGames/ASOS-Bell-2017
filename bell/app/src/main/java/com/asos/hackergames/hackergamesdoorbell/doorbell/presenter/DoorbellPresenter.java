package com.asos.hackergames.hackergamesdoorbell.doorbell.presenter;

import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.doorbell.model.DoorbellFunctions;
import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;
import com.asos.hackergames.hackergamesdoorbell.service.SignalRService;

public class DoorbellPresenter {

    private final DoorbellView view;
    private final SignalRService service;

    public DoorbellPresenter(@NonNull final DoorbellView view,
                             @NonNull final SignalRService service) {
        this.view = view;
        this.service = service;
    }

    public void pushDoorBell() {
        service.sendMessage(DoorbellFunctions.PRESS_BELL, "Boom!", "1234");
    }
}
