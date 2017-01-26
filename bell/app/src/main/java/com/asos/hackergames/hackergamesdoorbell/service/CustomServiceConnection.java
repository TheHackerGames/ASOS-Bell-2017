package com.asos.hackergames.hackergamesdoorbell.service;

import android.content.ComponentName;
import android.content.ServiceConnection;
import android.os.IBinder;
import android.support.annotation.NonNull;

public class CustomServiceConnection implements ServiceConnection {

    public interface CustomServiceListener {
        void onServiceBound();

        void onServiceUnbound();
    }

    private SignalRService service;
    private CustomServiceListener listener;

    public CustomServiceConnection(@NonNull final SignalRService service) {
        this.service = service;
    }

    @Override
    public void onServiceConnected(ComponentName className,
                                   IBinder localBinder) {
        // We've bound to SignalRService, cast the IBinder and get SignalRService instance
        SignalRService.LocalBinder binder = (SignalRService.LocalBinder) localBinder;
        service = binder.getService();
        listener.onServiceBound();
    }

    @Override
    public void onServiceDisconnected(ComponentName arg0) {
        listener.onServiceUnbound();
    }

    public void setListener(CustomServiceListener listener) {
        this.listener = listener;
    }
}
