package com.asos.hackergames.hackergamesdoorbell.doorbell.model;

import retrofit.RestAdapter;

public class ApiManager {

    private static DoorbellApi api = null;

    private ApiManager() {
        // Block instance creation
    }

    public static DoorbellApi getApi() {
        if (api == null) {
            api = new RestAdapter.Builder().setEndpoint(DoorbellApi.SERVER_URL)
                    .setLogLevel(RestAdapter.LogLevel.BASIC)
                    .build()
                    .create(DoorbellApi.class);
        }
        return api;
    }
}