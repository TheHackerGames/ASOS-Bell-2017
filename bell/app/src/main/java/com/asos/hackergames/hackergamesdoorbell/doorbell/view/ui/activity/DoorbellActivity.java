package com.asos.hackergames.hackergamesdoorbell.doorbell.view.ui.activity;

import android.content.Context;
import android.os.Bundle;
import android.os.Vibrator;
import android.widget.Toast;

import com.asos.hackergames.hackergamesdoorbell.R;
import com.asos.hackergames.hackergamesdoorbell.doorbell.presenter.DoorbellModule;
import com.asos.hackergames.hackergamesdoorbell.doorbell.presenter.DoorbellPresenter;
import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;
import com.asos.hackergames.hackergamesdoorbell.view.ui.activity.ServiceAwareActivity;

import butterknife.OnClick;

public class DoorbellActivity extends ServiceAwareActivity implements DoorbellView {

    private Vibrator vibrator;
//    private DoorbellPresenter presenter;

    @Override
    protected int getLayoutResourceId() {
        return R.layout.activity_doorbell;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
//        presenter = DoorbellModule.presenter(this, getService());
    }

    @Override
    public void displayMessage(String s) {
        Toast.makeText(this, s, Toast.LENGTH_SHORT).show();
    }

    @OnClick(R.id.doorbell)
    void onDoorbellClicked() {
        vibrator.vibrate(400);
        getService().sendMessage("Ding dong", "image id");
    }
}
