package com.asos.hackergames.hackergamesdoorbell.doorbell.presenter;

import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;
import com.asos.hackergames.hackergamesdoorbell.service.SignalRService;

public class DoorbellModule {

    public static DoorbellPresenter presenter(@NonNull final DoorbellView view,
                                              @NonNull final SignalRService service) {
        return new DoorbellPresenter(view, service);
    }
}
