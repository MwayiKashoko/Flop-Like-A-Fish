using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AddExplanation : MonoBehaviour {
    private TMP_Text GameText;
    private TMP_Text DescriptionText;
    private TMP_Text MoneyText;

    private string gameName = SwitchScenes.previousScreen;

    void Start() {
        GameText = GameObject.Find("GameText").GetComponent<TMP_Text>();
        DescriptionText = GameObject.Find("DescriptionText").GetComponent<TMP_Text>();
        MoneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();

        MoneyText.text = $"Money: ${GlobalScript.money}";

        switch (gameName) {
            case "AllInOrFoldSinglePlayer":
                GameText.text = "All In Or Fold";
                DescriptionText.text = "Upon betting, you will receive two cards from the deck which you will decide to continue playing by pressing the bet button or stop playing by folding. The second time the bet button is pressed, you will make a wager of 10x the initial bet. The game works based on the rules of Texas Hold 'Em and the better your hand is the higher the multiplier.";
                break;

            case "Baccarat":
                GameText.text = "Baccarat";
                DescriptionText.text = "Bet on one of two hands that you think will have a value of closer to nine. The highest hand is 9. Ace counts as 1, number cards are their number, and jack, queen, and king count as 10. The value of a hand is determined by adding up the values of each card and if the total value is greater than ten, then it will acquire the second digit of the number. A winning bet of the player receives a 2x payout, a tie results in either a push or a 9x payout depending on if a bet was placed, and a winning bet of the dealer results in a 1.95x payout. If either the player or dealer's first two cards total 8 or 9, the result is called a natural and the hand that totaled 8/9 will be declared the winner without any more cards being dealt. If both the dealer and player have a natural with different values, the 9 wins, and if the values of the two hands equal each other the result will be a push. The number of cards the player or dealer gets depends on the value after the first two cards are dealt for each hand.";
                break;

            case "BlackJack":
                GameText.text = "Blackjack";
                DescriptionText.text = "Blackjack is a card game where you compete against the dealer to get as close to 21 as possible without going over. Each card is worth its face value, with face cards (Jack, Queen, King) worth 10 and Aces worth either 1 or 11. When a hand goes over 21, it is called a bust, and the wager is automatically lost, even if the dealer also goes over 21. The dealer stands once their hand is greater than 16. When you receive your first two cards and the total is less than 21 you can stand (end hand), hit (take another card), or double (take one more card and end hand). Splitting is an option when your two cards have the same value, and it creates another hand. Insurance is for when the dealer shows an ace. There are a few sidebets that can be placed on the game with a great payout if hit. The 21+3 sidebet is hit when the value of the player's first 2 cards and the dealer's second card combine to make a flush (x6), straight (x11), three of-a-kind (x31), straight flush (x41), or suited three of a kind (x101). The perfect pair sidebet is hit when the first two player cards combine to make a Mixed Pair (x6), Colored Pair (x11), or a Perfect pair (x31).";
                break;

            case "ChoosePokerHandSinglePlayer":
                GameText.text = "Choose Poker Hand";
                DescriptionText.text = "Choose your poker hand from the dropdown and battle the dealer to see which hand wins. The rules follow from Texas Hold' Em.";
                break;

            case "CoinFlip":
                GameText.text = "Coin Flip";
                DescriptionText.text = "Choose to bet on either heads or tails for a 1.98x payout upon winning.";
                break;

            case "GuessTheNumber":
                GameText.text = "Guess The Number";
                DescriptionText.text = "Choose a min and max number and if you guess the correct number you will get a payout corresponding to the win percentage.";
                break;

            case "Hi_Lo":
                GameText.text = "Hi Lo";
                DescriptionText.text = "Choose whether the next card will be higher or lower than the current card. Ace is the lowest possible card and King is the highest possible card. Choose to skip the current card if you want a different card to bet on during the hand. Cashout at any point.";
                break;

            case "Mines":
                GameText.text = "Mines";
                DescriptionText.text = "Choose from 1 to up to 24 mines to avoid on the 5x5 grid. Cashout at any point.";
                break;

            case "Multiplier":
                GameText.text = "Multiplier";
                DescriptionText.text = "Choose a multiplier that the number on the screen has to go above for you to receive a payout.";
                break;

            case "OhioKeno":
                GameText.text = "Ohio Keno";
                DescriptionText.text = "Choose up to 10 spots on the board and then place your bet in the Ohio version of Keno. 20 spots will be randomly selected from the board and if a certain number of bets are hit, then you will receive a profit.";
                break;

            case "RegularKeno":
                GameText.text = "Regular Keno";
                DescriptionText.text = "Choose up to 10 spots on the board and then place your bet in Keno. 10 spots will be randomly selected from the board and if a certain number of bets are hit, then you will receive a profit.";
                break;

            case "ShortDeckTexasHoldEmSinglePlayer":
                GameText.text = "Short Deck Poker";
                DescriptionText.text = "The rules of this game are based on the Short Deck version of Texas Hold 'Em. All cards from 2-5 are removed from the deck. If your hand hits one of the selected hands, you will receive the corresponding payout.";
                break;

            case "SliderGame":
                GameText.text = "Slider";
                DescriptionText.text = "Choose to roll over or under a certain value in this game.";
                break;

            case "TexasHoldEmSinglePlayer":
                GameText.text = "Texas Hold 'Em Single Player";
                DescriptionText.text = "The rules of this game are based on Texas Hold 'Em. In this game, you try to make a hand that will give you a profit. Make an initial bet, and if you like your first two cards you can choose to bet the amount wagered again. If not you can decide to check which doesn't cost any more money. After the first 3 cards are dealt you are given the same option again. When the 4th card is dealt, you can either decide to bet the same wager or fold your cards thus not increasing your bet amount.";
                break;

            case "TexasHoldEmvsDealer":
                GameText.text = "Teaxs Hold 'Em vs Dealer";
                DescriptionText.text = "The rules of this game are based on Texas Hold 'Em. In this game you try to bet on what hand you think will win and/or if you think the player wins, the dealer wins, or a tie will happen.";
                break;

            case "Towers":
                GameText.text = "Towers";
                DescriptionText.text = "In Towers, you try to click as many safe tiles as possible in order to increase your profit. The highest payout for each difficulty follows this (x15, x40, x700, x60000, x1000000).";
                break;

            case "VideoPoker":
                GameText.text = "Video Poker";
                DescriptionText.text = "In Video Poker, you are given five cards from which you can choose to either hold or not upon dealing. All the cards that are held will not be taken from your hand, while the ones that are not will be replaced by new cards. Try and make one of the hands listed for a profit.";
                break;
        }
    }

    public void GoBackToGame() {
        Debug.Log("it worked");
        SceneManager.LoadScene(gameName);
        AdsManager.Instance.bannerAds.HideBannerAd();
    }
}
