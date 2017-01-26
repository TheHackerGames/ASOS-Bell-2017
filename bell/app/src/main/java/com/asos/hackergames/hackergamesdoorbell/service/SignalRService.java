package com.asos.hackergames.hackergamesdoorbell.service;

import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.Handler;
import android.os.IBinder;
import android.os.Looper;
import android.support.annotation.NonNull;

import com.asos.hackergames.hackergamesdoorbell.doorbell.view.DoorbellView;

import java.util.concurrent.ExecutionException;

import microsoft.aspnet.signalr.client.Platform;
import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.http.android.AndroidPlatformComponent;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;
import microsoft.aspnet.signalr.client.transport.ClientTransport;
import microsoft.aspnet.signalr.client.transport.LongPollingTransport;

import static com.asos.hackergames.hackergamesdoorbell.doorbell.model.DoorbellApi.SERVER_URL;

public class SignalRService extends Service {

    public static final String SERVER_HUB_CHAT = "HomeHub";
    public static final String PRESS_BELL = "PressBell";
    public static final String ACCEPT_HOME = "HomeAccepted";
    public static final String SEND_MESSAGE = "SendMessage";
    public static final String MESSAGE_RESPONDED = "MessageResponded";
    private DoorbellView view;

    private HubConnection hubConnection;
    private HubProxy hubProxy;
    private Handler handler; // to display Toast message
    private final IBinder binder = new LocalBinder(); // Binder given to clients

    public SignalRService() {
    }

    @Override
    public void onCreate() {
        super.onCreate();
        handler = new Handler(Looper.getMainLooper());
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        int result = super.onStartCommand(intent, flags, startId);
        startSignalR();
        return result;
    }

    @Override
    public void onDestroy() {
        hubConnection.stop();
        super.onDestroy();
    }

    @Override
    public IBinder onBind(Intent intent) {
        // Return the communication channel to the service.
        startSignalR();
        return binder;
    }

    public void setView(@NonNull final DoorbellView view) {
        this.view = view;
    }

    /**
     * Class used for the client Binder.  Because we know this service always
     * runs in the same process as its clients, we don't need to deal with IPC.
     */
    public class LocalBinder extends Binder {
        public SignalRService getService() {
            // Return this instance of SignalRService so clients can call public methods
            return SignalRService.this;
        }
    }

    public void pressBell(String message, String id) {
        hubProxy.invoke(PRESS_BELL, message, id);
    }

    public void sendText(String message, String id) {
        hubProxy.invoke(SEND_MESSAGE, message);
    }

    private void startSignalR() {
        Platform.loadPlatformComponent(new AndroidPlatformComponent());

        hubConnection = new HubConnection(SERVER_URL);
        hubProxy = hubConnection.createHubProxy(SERVER_HUB_CHAT);
        ClientTransport clientTransport = new LongPollingTransport(hubConnection.getLogger());
        SignalRFuture<Void> signalRFuture = hubConnection.start(clientTransport);

        try {
            signalRFuture.get();
        } catch (InterruptedException | ExecutionException e) {
            e.printStackTrace();
            return;
        }

        hubProxy.on(ACCEPT_HOME,
                new SubscriptionHandler1<String>() {
                    @Override
                    public void run(final String msg) {
                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                view.requestSpeech();
                            }
                        });
                    }
                }
                , String.class);

        hubProxy.on(MESSAGE_RESPONDED,
                new SubscriptionHandler1<String>() {
                    @Override
                    public void run(final String msg) {
                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                view.speakMessage(msg);
                            }
                        });
                    }
                }
                , String.class);

        hubProxy.invoke("RegisterBell");
    }
}
