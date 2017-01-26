package com.asos.hackergames.hackergamesdoorbell.doorbell.model;

import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Part;

public interface DoorbellApi {

    String SERVER_URL = "http://hackergameshubazureapi.azurewebsites.net";

    @POST("whatever")
    Call<String> sendMessage(@Body String message);

    @Multipart
    @PUT("user/photo")
    Call<String> updateUser(@Part("photo") RequestBody photo, @Part("description") RequestBody description);
}
