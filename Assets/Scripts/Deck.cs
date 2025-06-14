using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {
    private List<Card> _deck;
    private int _Count = 52;

    public Deck() {
        deck = new List<Card>();

        for (int i = 0; i < 4; i++) {
            string suit = "Spade";

            switch (i) {
                case 1:
                    suit = "Diamond";
                    break;

                case 2:
                    suit = "Club";
                    break;

                case 3:
                    suit = "Heart";
                    break;
            }

            for (int j = 1; j <= 13; j++) {
                string rank = j.ToString();

                switch (j) {
                    case 1:
                        rank = "Ace";
                        break;

                    case 11:
                        rank = "Jack";
                        break;

                    case 12:
                        rank = "Queen";
                        break;

                    case 13:
                        rank = "King";
                        break;
                }

                deck.Add(new Card(0, 0, 240, 336, suit, rank));
            }
        }
    }

    //Changes the ranks of the cards based the game
    public void ChangeCardRankings(string game) {
        foreach(Card card in deck) {
            try {
                card.rankInt = Int32.Parse(card.rankString);
                
            } catch (Exception) {

            }
        }

        switch (game) {
            case "Baccarat":
                foreach (Card card in deck) {
                    if (card.rankInt == 10) {
                        card.rankInt = 0;
                    }
                            
                    if (card.rankString == "Ace") {
                        card.rankInt = 1;
                    } else if (card.rankString == "Jack" || card.rankString == "Queen" || card.rankString == "King") {
                        card.rankInt = 0;
                    }

                    card.onBack = true;
                }

                break;

            case "HiLo":
                foreach (Card card in deck) {
                    if (card.rankString == "Ace") {
                        card.rankInt = 1;
                    } else if (card.rankString == "Jack") {
                        card.rankInt = 11;
                    } else if (card.rankString == "Queen") {
                        card.rankInt = 12;
                    } else if (card.rankString == "King") {
                        card.rankInt = 13;
                    }

                    card.onBack = false;
                }
                
                break;

            case "Video Poker":
                foreach (Card card in deck) {
                    if (card.rankString == "Ace") {
                        card.rankInt = 1;
                    } else if (card.rankString == "Jack") {
                        card.rankInt = 11;
                    } else if (card.rankString == "Queen") {
                        card.rankInt = 12;
                    } else if (card.rankString == "King") {
                        card.rankInt = 13;
                    }

                    card.onBack = true;
                }

                break;

            case "Blackjack":
                foreach (Card card in deck) {
                    if (card.rankString == "Ace") {
                        card.rankInt = 11;
                    } else if (card.rankString == "Jack" || card.rankString == "Queen" || card.rankString == "King") {
                        card.rankInt = 10;
                    }

                    card.onBack = true;
                }

                break;

            case "Solitaire":
                foreach (Card card in deck) {
                    if (card.rankString == "Ace") {
                        card.rankInt = 1;
                    } else if (card.rankString == "Jack") {
                        card.rankInt = 11;
                    } else if (card.rankString == "Queen") {
                        card.rankInt = 12;
                    } else if (card.rankString == "King") {
                        card.rankInt = 13;
                    }

                    card.onBack = true;
                }
                
                break;
        }
    }

    public List<Card> deck {
        get {
            return _deck;
        }

        set {
            _deck = value;
        }
    }

    public int Count {
        get {
            return _Count;
        }
    }
}