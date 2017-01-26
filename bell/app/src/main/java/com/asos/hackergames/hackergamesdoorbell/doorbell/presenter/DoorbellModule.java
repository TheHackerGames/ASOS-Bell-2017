package com.asos.hackergames.hackergamesdoorbell.doorbell.presenter;

import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.service.CustomServiceConnection;
import com.asos.hackergames.hackergamesdoorbell.service.SignalRService;

public class DoorbellModule {

    public static CustomServiceConnection serviceConnection(@NonNull final SignalRService service) {
        return new CustomServiceConnection(service);
    }
}
