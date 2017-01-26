package com.asos.hackergames.hackergamesdoorbell.doorbell.presenter;

import android.graphics.Bitmap;
import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;

import java.io.File;

import okhttp3.MediaType;
import okhttp3.RequestBody;
import retrofit2.Retrofit;

public class DoorbellPresenter {

    private final DoorbellView view;

    public DoorbellPresenter(@NonNull final DoorbellView view) {
        this.view = view;
    }

    public void pushDoorBell() {
        view.takePicture();
    }

//    public void sendImage(Bitmap imageBitmap) {
//        // send image bitmap
//
//        File file = new File(imageUri.getPath());
//        RequestBody fbody = RequestBody.create(MediaType.parse("image/*"), file);
//        RequestBody name = RequestBody.create(MediaType.parse("text/plain"), firstNameField.getText().toString());
//        RequestBody id = RequestBody.create(MediaType.parse("text/plain"), AZUtils.getUserId(this));
//        Call<User> call = client.editUser(AZUtils.getToken(this), fbody, name, id);
//        call.enqueue(new Callback<User>() {
//            @Override
//            public void onResponse(retrofit.Response<User> response, Retrofit retrofit) {
//                AZUtils.printObject(response.body());
//            }
//
//            @Override
//            public void onFailure(Throwable t) {
//                t.printStackTrace();
//            }
//        });
//    }
}
