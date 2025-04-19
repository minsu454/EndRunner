using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using Firebase.Auth;
using System;

public class SDKManager : MonoBehaviour
{
    private FirebaseAuth fbAuth;

    // Start is called before the first frame update
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestEmail()
            .Build();

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        fbAuth = FirebaseAuth.DefaultInstance;

        TryGoogleLogin();
    }

    public void TryGoogleLogin()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (success) =>
        {
            if (success == SignInStatus.Success)
            {
                StartCoroutine(CoTryFirebaseLogin());
            }
            else
            {
                Debug.Log("Is Not GoogleLogin");
            }
        });

    }

    IEnumerator CoTryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        {
            yield return null;
        }

        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        fbAuth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        { 
            if (task.IsCanceled)
            {

            }
            else if (task.IsFaulted)
            {

            }
            else
            {

            }
        });

    }
}
