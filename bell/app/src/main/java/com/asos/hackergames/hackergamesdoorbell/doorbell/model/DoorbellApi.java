package com.asos.hackergames.hackergamesdoorbell.doorbell.model;

import retrofit.Callback;
import retrofit.http.Body;
import retrofit.http.POST;
import retrofit.mime.TypedInput;

public interface DoorbellApi {

    String SERVER_URL = "http://hackergameshubazureapi.azurewebsites.net";

    @POST("/api/image")
    void upload(@Body TypedInput bytes, Callback<String> cb);
}
