using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 
public class SwitchScenes : MonoBehaviour {
    private Scene scene;
    public static string previousScreen;
    //private bool canShowAd = false;

    public void Start() {
        scene = SceneManager.GetActiveScene();
    }

    //Change the scene from the title screen
    public void ChangeTitleScreen(string buttonName) {
        //canShowAd = true;
        
        switch(buttonName) {
            case "PlayButton":
                SceneManager.LoadScene("GameList");
                //canShowAd = false;
                break;

            case "OptionsButton":
                SceneManager.LoadScene("Options");
                break;

            case "ShopButton":
                SceneManager.LoadScene("Shop");
                break;

            case "StatisticsButton":
                SceneManager.LoadScene("Statistics");
                break;

            case "AboutButton":
                SceneManager.LoadScene("About");
                break;
        }
    }

    IEnumerator HideAd() {
        yield return new WaitForSecondsRealtime(3);

        if (scene.name == "Options" || scene.name == "Shop" || scene.name == "Statistics" || scene.name == "GameExplanation" || scene.name == "About" || scene.name == "GameList") {
            AdsManager.Instance.bannerAds.HideBannerAd();
        }
    }

    //Change the scene from the game list
    public void ChangeGameList(string buttonName) {
        AdsManager.Instance.bannerAds.HideBannerAd();

        StartCoroutine(HideAd());

        switch(buttonName) {
            case "Baccarat":
                SceneManager.LoadScene("Baccarat");
                break;

            case "Slider":
                SceneManager.LoadScene("SliderGame");
                break;

            case "AllInOrFoldSinglePlayer":
                SceneManager.LoadScene("AllInOrFoldSinglePlayer");
                break;

            case "Blackjack":
                SceneManager.LoadScene("BlackJack");
                break;

            case "DoesPokerHandHoldUpSinglePlayer":
                SceneManager.LoadScene("ChoosePokerHandSinglePlayer");
                break;

            case "Coin Flip":
                SceneManager.LoadScene("CoinFlip");
                break;

            case "Guess Number":
                SceneManager.LoadScene("GuessTheNumber");
                break;

            case "Hi Lo":
                SceneManager.LoadScene("Hi_Lo");
                break;

            case "Mines":
                SceneManager.LoadScene("Mines");
                break;

            case "Multiplier":
                SceneManager.LoadScene("Multiplier");
                break;

            case "Ohio Keno":
                SceneManager.LoadScene("OhioKeno");
                break;

            case "Keno":
                SceneManager.LoadScene("RegularKeno");
                break;

            case "Shortdeck Poker Singleplayer":
                SceneManager.LoadScene("ShortDeckTexasHoldEmSinglePlayer");
                break;

            case "Solo Poker":
                SceneManager.LoadScene("TexasHoldEmSinglePlayer");
                break;

            case "Duo Poker":
                SceneManager.LoadScene("TexasHoldEmvsDealer");
                break;

            case "Towers":
                SceneManager.LoadScene("Towers");
                break;

            case "Video Poker":
                SceneManager.LoadScene("VideoPoker");
                break;

            case "BackToMainMenu":
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    //Method that deals with scene changes using a switch statement
    public void NextScene(GameObject button) {
        switch(scene.name) {
            case "MainMenu":
                ChangeTitleScreen(button.name);
                break;

            case "GameList":
                ChangeGameList(button.name);
                break;

            case "About":
                ChangeGameList(button.name);
                break;

            case "Options":
                ChangeGameList(button.name);
                break;

            case "Shop":
                ChangeGameList(button.name);
                break;

            case "Statistics":
                ChangeGameList(button.name);
                break;

            default:
                SceneManager.LoadScene("GameList");
                break;
        }

        if (scene.name == "Options" || scene.name == "Shop" || scene.name == "Statistics" || scene.name == "GameExplanation" || scene.name == "About" || scene.name == "GameList") {
            AdsManager.Instance.bannerAds.HideBannerAd();

            StartCoroutine(HideAd());
        }
    }

    public void GoToExplanationScene() {
        previousScreen = scene.name;
        SceneManager.LoadScene("GameExplanation");
        //canShowAd = true;
    }

    /*void Update() {
        if (Advertisement.Banner.isLoaded || Advertisement.IsReady(bannerPlacementName)) {
            Advertisement.Banner.Show(bannerPlacementName);
        }

        if(canShowAd) {
            try {
                AdsManager.InitializeAds.bannerAds.ShowBannerAd();
                canShowAd = false;
            } catch (Exception) {

            }
        }
    }*/
}