package com.asos.hackergames.hackergamesdoorbell.doorbell.model;

import android.support.annotation.StringDef;

import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;

import static com.asos.hackergames.hackergamesdoorbell.doorbell.model.DoorbellFunctions.PRESS_BELL;

@Retention(RetentionPolicy.SOURCE)
@StringDef({PRESS_BELL})
public @interface DoorbellFunctions {

    String PRESS_BELL = "PressBell";
}
