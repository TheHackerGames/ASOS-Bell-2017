package com.asos.hackergames.hackergamesdoorbell.doorbell.view.ui.activity;

import android.content.ActivityNotFoundException;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.os.Vibrator;
import android.speech.RecognizerIntent;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.asos.hackergames.hackergamesdoorbell.R;
import com.asos.hackergames.hackergamesdoorbell.doorbell.presenter.DoorbellModule;
import com.asos.hackergames.hackergamesdoorbell.doorbell.presenter.DoorbellPresenter;
import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;
import com.asos.hackergames.hackergamesdoorbell.service.SignalRService;
import com.asos.hackergames.hackergamesdoorbell.view.ui.activity.BaseActivity;

import java.util.ArrayList;
import java.util.Locale;

import butterknife.BindView;
import butterknife.OnClick;

import static android.view.View.GONE;

public class DoorbellActivity extends BaseActivity implements DoorbellView {

    private static final int REQ_CODE_SPEECH_INPUT = 99;

    private Vibrator vibrator;
    private DoorbellPresenter presenter;

    private boolean bound = false;
    private SignalRService service;

    @BindView(R.id.user_voice_output)
    TextView userText;

    @Override
    protected int getLayoutResourceId() {
        return R.layout.activity_doorbell;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Intent intent = new Intent();
        intent.setClass(this, SignalRService.class);
        bindService(intent, serviceConnection, Context.BIND_AUTO_CREATE);

        vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
        presenter = DoorbellModule.presenter(this);
    }

    @Override
    public void displayMessage(String s) {
        Toast.makeText(this, s, Toast.LENGTH_SHORT).show();
    }

    @OnClick(R.id.doorbell)
    void onDoorbellClicked() {
        userText.setVisibility(GONE);
        vibrator.vibrate(400);
        presenter.pushDoorBell();
        service.pressBell("Ding dong", "image id");
    }

    @Override
    public void requestSpeech() {
        Intent intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,
                RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE, Locale.getDefault());
        intent.putExtra(RecognizerIntent.EXTRA_PROMPT,
                getString(R.string.speech_prompt));
        try {
            startActivityForResult(intent, REQ_CODE_SPEECH_INPUT);
        } catch (ActivityNotFoundException a) {
            Toast.makeText(getApplicationContext(),
                    getString(R.string.speech_not_supported),
                    Toast.LENGTH_SHORT).show();
        }
    }

    /**
     * Receiving speech input
     */
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        switch (requestCode) {
            case REQ_CODE_SPEECH_INPUT: {
                if (resultCode == RESULT_OK && null != data) {

                    ArrayList<String> result = data
                            .getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
                    userText.setVisibility(View.VISIBLE);
                    userText.setText(result.get(0));
                }
                break;
            }

        }
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
            service.setView(DoorbellActivity.this);
            bound = true;
        }

        @Override
        public void onServiceDisconnected(ComponentName arg0) {
            bound = false;
        }
    };
}
