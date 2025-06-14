using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {
    private int _x;
    private int _y;
	private int _width;
	private int _height;
	private string _suit;

    //Rank of a card using a string
	private string _rankString;
    //Rank of a card using an int (rank varies between games like baccarat/blackjack/poker)
    private int _rankInt;

    private string _cardValue;

	private string _color;
	private bool _onBack = false;

    private int _pokerRankInt;

    //Position in the spriteList array the image is at
	private int _pos;

    public Card(int x, int y, int width, int height, string suit, string rank) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.suit = suit;
        rankString = rank;

        this.color = (suit.Contains("Diamond") || suit.Contains("Heart")) ? "Red" : "Black";

        int suitInt = 0;

        switch (suit) {
            case "Diamond":
                suitInt = 1;
                break;

            case "Club":
                suitInt = 2;
                break;

            case "Heart":
                suitInt = 3;
                break;
        }

        int rankInt = 0;

        try {
            rankInt = Int32.Parse(rankString)-1;
            pokerRankInt = Int32.Parse(rankString);
        } catch (Exception) {
            switch (rankString) {
                case "Ace":
                    rankInt = 0;
                    pokerRankInt = 1;
                    break;
                
                case "Jack":
                    rankInt = 10;
                    pokerRankInt = 11;
                    break;

                case "Queen":
                    rankInt = 11;
                    pokerRankInt = 12;
                    break;

                case "King":
                    rankInt = 12;
                    pokerRankInt = 13;
                    break;
            }
        }

        cardValue = $"{rankInt}{suit}";

        pos = suitInt*13 + rankInt;
    }

    public Card Clone() {
        int rank = rankInt;
        bool OnBack = onBack;
        int pokerRank = pokerRankInt;

        Card card = new Card(this.x, this.y, this.width, this.height, this.suit, this.rankString);

        card.rankInt = rank;
        card.pokerRankInt = pokerRankInt;

        card.onBack = OnBack;

        return card;
    }

    public int x {
        get {
            return _x;
        }

        set {
            _x = value;
        }
    }

    public int y {
        get {
            return _y;
        }

        set {
            _y = value;
        }
    }

    public int width {
        get {
            return _width;
        }

        set {
            _width = value;
        }
    }

    public int height {
        get {
            return _height;
        }

        set {
            _height = value;
        }
    }

    public string suit {
        get {
            return _suit;
        }

        set {
            _suit = value;
        }
    }

    public string rankString {
        get {
            return _rankString;
        }

        set {
            _rankString = value;
        }
    }

    public int rankInt {
        get {
            return _rankInt;
        }

        set {
            _rankInt = value;
        }
    }

    public string color {
        get {
            return _color;
        }

        set {
            _color = value;
        }
    }

    public bool onBack {
        get {
            return _onBack;
        }

        set {
            _onBack = value;
        }
    }

    public string cardValue {
        get {
            return _cardValue;
        }

        set {
            _cardValue = value;
        }
    }

    public int pos {
        get {
            return _pos;
        }

        set {
            _pos = value;
        }
    }

    public int pokerRankInt {
        get {
            return _pokerRankInt;
        }

        set {
            _pokerRankInt = value;
        }
    }
}
