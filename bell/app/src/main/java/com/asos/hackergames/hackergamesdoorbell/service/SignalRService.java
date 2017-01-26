package com.asos.hackergames.hackergamesdoorbell.service;

import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.Handler;
import android.os.IBinder;
import android.os.Looper;
import android.widget.Toast;

import java.util.concurrent.ExecutionException;

import microsoft.aspnet.signalr.client.Platform;
import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.http.android.AndroidPlatformComponent;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;
import microsoft.aspnet.signalr.client.transport.ClientTransport;
import microsoft.aspnet.signalr.client.transport.LongPollingTransport;

public class SignalRService extends Service {

    private static final String SERVER_URL = "http://hackergameshubazureapi.azurewebsites.net";
    public static final String SERVER_HUB_CHAT = "HomeHub";
    public static final String PRESS_BELL = "PressBell";

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

    /**
     * method for clients (activities)
     */
    public void sendMessage(String message, String id) {
        hubProxy.invoke(PRESS_BELL, message, id);
    }

    private void startSignalR() {
        Platform.loadPlatformComponent(new AndroidPlatformComponent());

//        Credentials credentials = new Credentials() {
//            @Override
//            public void prepareRequest(Request request) {
//                request.addHeader("User-Name", "BNK");
//            }
//        };

        hubConnection = new HubConnection(SERVER_URL);
//        hubConnection.setCredentials(credentials);
        hubProxy = hubConnection.createHubProxy(SERVER_HUB_CHAT);
        ClientTransport clientTransport = new LongPollingTransport(hubConnection.getLogger());
        SignalRFuture<Void> signalRFuture = hubConnection.start(clientTransport);

        try {
            signalRFuture.get();
        } catch (InterruptedException | ExecutionException e) {
            e.printStackTrace();
            return;
        }

        sendMessage("Initialising", "id");

        String CLIENT_METHOD_BROADAST_MESSAGE = "broadcastMessage";
        hubProxy.on(CLIENT_METHOD_BROADAST_MESSAGE,
                new SubscriptionHandler1<CustomMessage>() {
                    @Override
                    public void run(final CustomMessage msg) {
                        final String finalMsg = msg.UserName + " says " + msg.Message;
                        // display Toast message
                        handler.post(new Runnable() {
                            @Override
                            public void run() {
                                Toast.makeText(getApplicationContext(), finalMsg, Toast.LENGTH_SHORT).show();
                            }
                        });
                    }
                }
                , CustomMessage.class);
    }
}
