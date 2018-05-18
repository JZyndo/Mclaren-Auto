using UnityEngine;
using System.Collections;
#if UNITY_STANDALONE_WIN
using XInputDotNetPure;
#endif

public class VibrationPageEvent : PageEventBase {
#if UNITY_STANDALONE_WIN
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    float timeSoFar = 0;
    bool vibrating = false;
#endif
    public override void ForceEvent()
    {
        base.ForceEvent();
        ProcessEvent();
    }

    public override void ProcessEvent()
    {

#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
#if UNITY_STANDALONE_WIN
        vibrating = true;
        timeSoFar = 0;
#endif
    }
#if UNITY_STANDALONE_WIN
    void FixedUpdate()
    {
        if (!vibrating || timeSoFar > duration)
        {
            if(vibrating)
            {
                vibrating = false;
                GamePad.SetVibration(playerIndex, 0, 0);
            }
            return;
        }
        
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        GamePad.SetVibration(playerIndex, curve.Evaluate(timeSoFar/duration), curve.Evaluate(timeSoFar / duration));
        timeSoFar += Time.deltaTime;
    }

    private void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected and use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
            GamePad.SetVibration(playerIndex, 0, 0);
    }

    private void OnApplicationQuit()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
            GamePad.SetVibration(playerIndex, 0, 0);
    }
#endif
}
