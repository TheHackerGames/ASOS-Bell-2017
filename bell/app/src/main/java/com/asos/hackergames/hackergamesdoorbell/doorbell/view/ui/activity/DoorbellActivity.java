package com.asos.hackergames.hackergamesdoorbell.doorbell.view.ui.activity;

import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.os.Vibrator;
import android.widget.Toast;

import com.asos.hackergames.hackergamesdoorbell.R;
import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;
import com.asos.hackergames.hackergamesdoorbell.service.SignalRService;
import com.asos.hackergames.hackergamesdoorbell.view.ui.activity.BaseActivity;

import butterknife.OnClick;

public class DoorbellActivity extends BaseActivity implements DoorbellView {

    private Vibrator vibrator;

    private boolean bound = false;
    private SignalRService service;

    @Override
    protected int getLayoutResourceId() {
        return R.layout.activity_doorbell;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);

        Intent intent = new Intent();
        intent.setClass(this, SignalRService.class);
        bindService(intent, serviceConnection, Context.BIND_AUTO_CREATE);
    }

    @Override
    public void displayMessage(String s) {
        Toast.makeText(this, s, Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onServiceBound(boolean bound) {
        this.bound = bound;
    }

    @OnClick(R.id.doorbell)
    void onDoorbellClicked() {
        vibrator.vibrate(400);
        service.sendMessage("Hello World", "image id");
    }

    @Override
    protected void onStop() {
        // Unbind from the service
        if (bound) {
            unbindService(serviceConnection);
            bound = false;
        }
        super.onStop();
    }

    /**
     * Defines callbacks for service binding, passed to bindService()
     */
    private final ServiceConnection serviceConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName className,
                                       IBinder localBinder) {
            // We've bound to SignalRService, cast the IBinder and get SignalRService instance
            SignalRService.LocalBinder binder = (SignalRService.LocalBinder) localBinder;
            service = binder.getService();
            bound = true;
        }

        @Override
        public void onServiceDisconnected(ComponentName arg0) {
            bound = false;
        }
    };
}
