package com.asos.hackergames.hackergamesdoorbell;

import android.app.Application;

import retrofit2.Retrofit;

import static com.asos.hackergames.hackergamesdoorbell.doorbell.model.DoorbellApi.SERVER_URL;

public class DoorbellApp extends Application {

    private static Retrofit retrofit;

    @Override
    public void onCreate() {
        super.onCreate();

        retrofit = new Retrofit.Builder()
                .baseUrl(SERVER_URL)
                .build();
    }
}
