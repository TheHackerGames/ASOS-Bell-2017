package com.asos.hackergames.hackergamesdoorbell.doorbell.model;

import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;

public interface DoorbellApi {

    String SERVER_URL = "http://hackergameshubazureapi.azurewebsites.net";

    @Multipart
    @POST("api/image")
    Call<String> sendImage(@Part("image") RequestBody photo, @Part("description") RequestBody description);
}
