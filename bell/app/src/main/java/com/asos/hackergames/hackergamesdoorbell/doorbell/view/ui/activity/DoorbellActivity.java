package com.asos.hackergames.hackergamesdoorbell.doorbell.view.ui.activity;

import android.Manifest;
import android.content.ActivityNotFoundException;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.content.pm.ActivityInfo;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.os.IBinder;
import android.os.Vibrator;
import android.provider.MediaStore;
import android.speech.RecognizerIntent;
import android.speech.tts.TextToSpeech;
import android.speech.tts.UtteranceProgressListener;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.ViewFlipper;

import com.asos.hackergames.hackergamesdoorbell.R;
import com.asos.hackergames.hackergamesdoorbell.doorbell.model.ApiManager;
import com.asos.hackergames.hackergamesdoorbell.doorbell.presenter.DoorbellModule;
import com.asos.hackergames.hackergamesdoorbell.doorbell.presenter.DoorbellPresenter;
import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;
import com.asos.hackergames.hackergamesdoorbell.service.SignalRService;
import com.asos.hackergames.hackergamesdoorbell.view.ui.activity.BaseActivity;

import org.apache.commons.io.FileUtils;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Locale;

import butterknife.BindView;
import butterknife.OnClick;
import retrofit.RetrofitError;
import retrofit.client.Response;
import retrofit.mime.TypedByteArray;
import retrofit.mime.TypedInput;

import static android.view.View.GONE;

public class DoorbellActivity extends BaseActivity implements DoorbellView, TextToSpeech.OnInitListener {

    private static final int REQ_CODE_SPEECH_INPUT = 99;
    private static final int REQUEST_IMAGE_CAPTURE = 11;

    private Vibrator vibrator;
    private DoorbellPresenter presenter;

    private boolean bound = false;
    private SignalRService service;

    TextToSpeech textToSpeech;

    @BindView(R.id.view_flipper)
    ViewFlipper viewFlipper;

    @BindView(R.id.user_voice_output)
    TextView userText;

    @BindView(R.id.image_preview)
    ImageView imageView;

    @Override
    protected int getLayoutResourceId() {
        return R.layout.activity_doorbell;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        ActivityCompat.requestPermissions(this,
                new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE},
                1);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_PORTRAIT);
        Intent intent = new Intent();
        intent.setClass(this, SignalRService.class);
        bindService(intent, serviceConnection, Context.BIND_AUTO_CREATE);

        vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
        textToSpeech = new TextToSpeech(this, this);
        textToSpeech.setOnUtteranceProgressListener(new UtteranceProgressListener() {
            @Override
            public void onStart(String utteranceId) {
            }

            @Override
            public void onDone(String utteranceId) {
                requestSpeech();
            }

            @Override
            public void onError(String utteranceId) {

            }
        });
        presenter = DoorbellModule.presenter(this);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
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
    }

    @OnClick({R.id.access_granted, R.id.access_denied})
    void onTapped() {
        viewFlipper.setDisplayedChild(0);
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

    @Override
    public void takePicture() {
        Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
            startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
        }
    }

    @Override
    public void speakMessage(String message) {
        textToSpeech.speak(message, TextToSpeech.QUEUE_FLUSH, null, "Boo");
    }

    @Override
    public void onDoorOpened() {
        viewFlipper.setDisplayedChild(1);
    }

    @Override
    public void onRejected() {
        viewFlipper.setDisplayedChild(2);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == RESULT_OK) {
            Bundle extras = data.getExtras();
            Bitmap imageBitmap = (Bitmap) extras.get("data");
            imageView.setVisibility(View.VISIBLE);
            imageView.setImageBitmap(imageBitmap);

            // CALL THIS METHOD TO GET THE URI FROM THE BITMAP
            Uri tempUri = getImageUri(getApplicationContext(), imageBitmap);

            // CALL THIS METHOD TO GET THE ACTUAL PATH
            File finalFile = new File(getRealPathFromURI(tempUri));
            sendPhotoR1(finalFile);
        }
        switch (requestCode) {
            case REQ_CODE_SPEECH_INPUT: {
                if (resultCode == RESULT_OK && null != data) {

                    ArrayList<String> result = data
                            .getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
                    userText.setVisibility(View.VISIBLE);
                    final String text = result.get(0);
                    userText.setText(text);
                    service.sendText(text, "");
//                    presenter.sendImage(imageBitmap);
                }
                break;
            }
        }
    }

    private void sendPhotoR1(File finalFile) {
        byte[] byteArray = new byte[0];
        try {
            byteArray = FileUtils.readFileToByteArray(finalFile);
        } catch (IOException e) {
            e.printStackTrace();
        }


        TypedInput typedBytes = new TypedByteArray("application/octet-stream", byteArray);
        ApiManager.getApi().upload(typedBytes, new retrofit.Callback<String>() {
            @Override
            public void success(String s, Response response) {
                Toast.makeText(DoorbellActivity.this, "Success", Toast.LENGTH_SHORT).show();
                service.pressBell("Ding dong", response.getHeaders().get(5).getValue());
            }

            @Override
            public void failure(RetrofitError error) {
                Toast.makeText(DoorbellActivity.this, "Fail", Toast.LENGTH_SHORT).show();
            }
        });
    }

    public Uri getImageUri(Context inContext, Bitmap inImage) {
        ByteArrayOutputStream bytes = new ByteArrayOutputStream();
        inImage.compress(Bitmap.CompressFormat.JPEG, 100, bytes);
        String path = MediaStore.Images.Media.insertImage(inContext.getContentResolver(), inImage, "Title", null);
        return Uri.parse(path);
    }

    public String getRealPathFromURI(Uri uri) {
        Cursor cursor = getContentResolver().query(uri, null, null, null, null);
        cursor.moveToFirst();
        int idx = cursor.getColumnIndex(MediaStore.Images.ImageColumns.DATA);
        return cursor.getString(idx);
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

    @Override
    public void onInit(int status) {
        if (status == TextToSpeech.SUCCESS) {

            textToSpeech.setLanguage(Locale.ENGLISH);
        }
    }
}
