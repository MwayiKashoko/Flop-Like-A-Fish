using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class GlobalScript {
    //Random integer function
    public static System.Random _random = new System.Random();

    //Current amount of money owned
    public static decimal _money = 1000M;

    public static decimal moneyWagered = 0;
    public static decimal moneyWon = 0;
    public static decimal profit = 0;
    public static decimal moneyLost = 0;
    public static int betsPlaced = 0;
    public static int totalWins = 0;
    public static int totalLosses = 0;
    public static decimal largestWin = 0;
    public static decimal largestLoss = 0;
    public static float luckiestWin = 0;
    public static bool noAds = false;

    private static List<decimal> bets = new List<decimal>();

    //Unshuffled deck of cards
    public static Deck _deck = new Deck();

    public static Dictionary<int, string> rankNames = new Dictionary<int, string> {
        {11, "Jack"},
        {12, "Queen"},
        {13, "King"}
    };

    public static Dictionary<int, string> handRankings = new Dictionary<int, string> {
        {1, "Royal Flush"},
        {2, "Straight Flush"},
        {3, "Four of a Kind"},
        {4, "Full House"},
        {5, "Flush"},
        {6, "Straight"},
        {7, "Three of a Kind"},
        {8, "Two Pair"},
        {9, "Pair"},
        {10, "High"}
    };

    public static void UpdateMoney(string operation, params decimal[] inputs) {
        if (operation == "minus") {
            bets.Clear();
        }

        for (int i = 0; i < inputs.Length; i++) {
            switch (operation) {
                case "minus":
                    betsPlaced++;
                    money -= Math.Round(inputs[i], 2);
                    moneyWagered += Math.Round(inputs[i], 2);
                    profit -= Math.Round(inputs[i], 2);
                    moneyLost += Math.Round(inputs[i], 2);
                    bets.Add(Math.Round(inputs[i], 2));
                    break;

                case "plus":
                    money += Math.Round(inputs[i], 2);
                    moneyWon += Math.Round(inputs[i], 2);

                    decimal winAmount = Math.Round((Math.Round(inputs[i], 2) - bets[i]), 2);

                    moneyLost -= inputs[i];

                    float multiplier = 0;

                    try {
                        multiplier = (float) Math.Round(inputs[i] / bets[i], 2);
                    } catch (Exception) {

                    }

                    if (multiplier > luckiestWin) {
                        luckiestWin = multiplier;
                    }

                    if (winAmount > 0) {
                        totalWins++;

                        moneyLost -= winAmount;

                        if (winAmount > largestWin) {
                            profit += winAmount;
                            largestWin = winAmount;
                        }
                    } else {
                        if (bets[i] != 0) {
                            totalLosses++;
                        }

                        if (Math.Abs(winAmount) > largestLoss) {
                            largestLoss = Math.Abs(winAmount);
                        }
                    }

                    break;
                
            }
        }

        //When the user has lost all money
        if (operation == "plus" && money <= (decimal) 0.01) {
            Debug.Log("Lost all money");
        }

        moneyLost = Math.Abs(moneyLost);
    }

    //Shuffles baccaratDeck when the number of cards in the deck is too low
    public static void ShuffleCards(List<Card> newDeck, int decks, bool useNewDeck) {
        if (useNewDeck && decks < 100) {
            newDeck.Clear();
            //Adding all the cards to the deck
            for (int i = 0; i < deck.Count; i++) {
                for (int j = 0; j < decks; j++) {
                    newDeck.Add(deck.deck[i].Clone());
                }
            }
        }

        //Triggers short deck
        if (decks == 100) {
            newDeck.Clear();

            for (int i = 0; i < deck.Count; i++) {
                if ((i % 13) + 1 >= 6 || (i % 13) + 1 == 1) {
                    newDeck.Add(deck.deck[i].Clone());
                }
            }
        }

        //Fisher-Yates shuffle cards
        /*-- To shuffle an array a of n elements (indices 0..n-1):
        for i from 0 to n−2 do
            j ← random integer such that i ≤ j < n
            exchange a[i] and a[j]*/

        for (int i = 0; i < newDeck.Count; i++) {
            int j = random.Next(0, i+1);
            Card copy = newDeck[i];
            newDeck[i] = newDeck[j];
            newDeck[j] = copy;
        }
    }

    //For copying deck without shuffling it
    public static void CopyDeck(List<Card> newDeck) {
        newDeck.Clear();

        for (int i = 0; i < deck.Count; i++) {
            newDeck.Add(deck.deck[i].Clone());
        }
    }

    public static Card GetNextCard() {
        return deck.deck[random.Next(0, 52)].Clone();
    }

    //Draws card to parent]
    public static void DrawCard(float x, float y, float width, float height, Card card, Transform parent, List<Image> images, List<GameObject> cardImages, Sprite[] spriteList) {
        card.x = (int) x;
        card.y = (int) y;
        card.width = (int) width;
        card.height = (int) height;

        //Adding the image that shows the front of the card
        GameObject imgObject = new GameObject("Image");

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(parent.transform); // setting parent
        trans.localScale = Vector3.one;
        trans.anchoredPosition = new Vector2(card.x, card.y); // setting position, will be on center
        trans.sizeDelta = new Vector2(card.width, card.height); // custom size
            
        Image image = imgObject.AddComponent<Image>();
        imgObject.GetComponent<Image>().sprite = spriteList[card.pos];

        if (card.onBack) {
            imgObject.GetComponent<Image>().sprite = spriteList.Last();
        }

        imgObject.transform.SetParent(parent.transform);

        images.Add(image);
        cardImages.Add(imgObject);
    }

    //Removes all cards from the selected 
    public static void RemoveCards(List<Image> images, List<GameObject> cardImages) {
        if (images != null) {
            foreach(var item in images) {
                UnityEngine.Object.Destroy(item);
            }

            images.Clear();
        }

        if (cardImages != null) {
            foreach(var item in cardImages) {
                UnityEngine.Object.Destroy(item);
            }

            cardImages.Clear();
        }
    }

    //Gets string from input text and checks to see if it is empty or not and returns 0 if it is empty.
    public static decimal ReturnNum(string str) {
        if (str != "") {
            try {
                return Convert.ToDecimal(str);
            } catch (Exception) {
                
            }
        }

        return 0;
    }

    public static void ValidateInput(TMP_InputField input, Button button) {
        decimal num = ReturnNum(input.text);

        if (num > 1_000_000M || num < 0 || num > money) {
            button.interactable = false;
        } else {
            button.interactable = true;
        }
    }

    public static void RoundInput(TMP_InputField input) {
        input.text = $"{Math.Round(ReturnNum(input.text), 2)}";
    }

    //Removes last item from Card list passed and returns it
    public static T Pop<T>(List<T> list) {
        T item = list.Last();
        list.RemoveAt(list.Count-1);
        return item;
    }

    //Copies over an entire list
    public static List<T> CloneList<T>(List<T> list) {
        List<T> newList = new List<T>();

        for (int i = 0; i < list.Count; i++) {
            newList.Add(list[i]);
        }

        return newList;
    }

    //Smoothly moves card from one place to another on the screen
    public static bool MoveCard(Vector3 startPosition, Vector3 targetPosition, float percentageComplete, GameObject cardObject) {
        if (Vector3.Distance(cardObject.transform.localPosition, targetPosition) < 0.1f) {
            return true;
        } else {
            cardObject.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, percentageComplete));
        }

        return false;
    }

    //Smoothly flips card over on the screen
    public static void FlipCardOver(float percentageComplete, GameObject cardObject, Sprite[] spriteList, int pos) {
        cardObject.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 0, 0), Mathf.SmoothStep(0, 1, percentageComplete));

        if (cardObject.transform.localRotation.y <= 0.5) {
            cardObject.GetComponent<Image>().sprite = spriteList[pos];
        }
    }

    public static int[] DetermineRoyalFlush(string gameName, Card[] cards) {
        if (cards[0].rankString == "Ace" && cards.Last().rankString == "King" && handRankings[DetermineStraightFlush(gameName, cards)[0]] == "Straight Flush") {
            return new int[] {1};
        }

        return DetermineStraightFlush(gameName, cards);
    }

    //Returns int[] which has highest card straight the straight flush reaches, 2
    public static int[] DetermineStraightFlush(string gameName, Card[] cards) {
        if (handRankings[DetermineFlush(gameName, cards)[0]] == "Flush" && handRankings[DetermineStraight(gameName, cards)[0]] == "Straight") {
            return new int[] {2, cards.Last().rankInt};
        }

        return DetermineFourOfAKind(gameName, cards);
    }

    //Returns int[] showing the rank of the quads and the rank of the highest card on the board after the quads, 3
    public static int[] DetermineFourOfAKind(string gameName, Card[] cards) {
        int startIndex = 0;
        int highCard = cards.Length-1;

        bool nextRanking = false;

        if (cards[0].rankInt == cards[1].rankInt) {
            for (int i = 1; i < cards.Length-1; i++) {
                if (cards[0].rankInt != cards[i].rankInt) {
                    nextRanking = true;
                }
            }
        } else {
            startIndex = 1;
            highCard = 0;

            for (int i = 2; i < cards.Length; i++) {
                if (cards[1].rankInt != cards[i].rankInt) {
                    nextRanking = true;
                }
            }
        }

        if (nextRanking) {
            return DetermineFullHouse(gameName, cards);
        }

        int fourOfAKind = cards[startIndex].rankInt == 1 ? 14 : cards[startIndex].rankInt;
        int highCardNum = cards[highCard].rankInt == 1 ? 14 : cards[highCard].rankInt;

        return new int[] {3, fourOfAKind, highCardNum};
    }

    //Returns int[] showing the rank of the three of a kind, and then another int showing the rank of the pair, 3
    public static int[] DetermineFullHouse(string gameName, Card[] cards) {
        int threeOfAKind = 0;
        int pair = 2;

        bool nextRanking = false;

        if (cards[0].rankInt == cards[2].rankInt) {
            if (cards[0].rankInt != cards[1].rankInt || cards[3].rankInt != cards[4].rankInt) {
                nextRanking = true;
            }
        } else {
            threeOfAKind = 2;
            pair = 0;

            if (cards[0].rankInt != cards[1].rankInt) {
                nextRanking = true;
            }

            for (int i = 3; i < cards.Length; i++) {
                if (cards[2].rankInt != cards[i].rankInt) {
                    nextRanking = true;
                }
            }
        }

        if (nextRanking) {
            return DetermineFlush(gameName, cards);
        }

        int threeOfAKindNum = cards[threeOfAKind].rankInt == 1 ? 14 : cards[threeOfAKind].rankInt;
        int pairNum = cards[pair].rankInt == 1 ? 14 : cards[pair].rankInt;

        return new int[] {4, threeOfAKindNum, pairNum};
    }

    //Returns int[] for every value of the flush which will be used to compare other flushes in other player's hands, 6
    public static int[] DetermineFlush(string gameName, Card[] cards) {
        string suit = cards[0].suit;
        int[] ranks = new int[6];

        for (int i = 0; i < cards.Length; i++) {
            if (cards[i].suit != suit) {
                return DetermineStraight(gameName, cards);
            }

            ranks[i+1] = cards[i].rankInt == 1 ? 14 : cards[i].rankInt;
        }

        Array.Sort(ranks,
            delegate(int x, int y) { 
                return x.CompareTo(y); 
            }
        );

        ranks[0] = 5;

        return ranks;
    }

    //Returns int[] which has highest card straight the straight reaches, 2
    public static int[] DetermineStraight(string gameName, Card[] cards) {
        int highestCard = 1;

        bool nextRanking = false;

        if (cards[1].rankInt != 10) {
            for (int i = 1; i < cards.Length; i++) {
                if (cards[i].rankInt-1 != cards[i-1].rankInt) {
                    nextRanking = true;
                }
            }

            highestCard = cards.Last().rankInt;
        } else {
            if (cards[0].rankString != "Ace") {                        
                nextRanking = true;
            }

            for (int i = 2; i < cards.Length; i++) {
                if (cards[i].rankInt-1 != cards[i-1].rankInt) {
                    nextRanking = true;
                }
            }

            highestCard = 14;
        }

        if (nextRanking) {
            return DetermineThreeOfAKind(gameName, cards);
        }

        return new int[] {6, highestCard};
    }

    //Returns int[] showing the rank of the three of a kind, two ints showing the next highest two cards, 4
    public static int[] DetermineThreeOfAKind(string gameName, Card[] cards) {
        int count = 1;
        int cardPosition = 0;
                
        for (int i = 0; i < cards.Length; i++) {
            if (cards[cardPosition].rankInt != cards[i].rankInt) {
                cardPosition++;
                count = 1;
            } else {
                if (cardPosition != i) {
                    count++;
                }

                if (count == 3) {
                    int lowerInt = cards[3].rankInt;
                    int higherInt = cards[4].rankInt;

                    if (cardPosition == 1) {
                        lowerInt = cards[0].rankInt;
                    } else if (cardPosition == 2) {
                        lowerInt = cards[0].rankInt;
                        higherInt = cards[1].rankInt;
                    }

                    lowerInt = lowerInt == 1 ? 14 : lowerInt;
                    higherInt = higherInt == 1 ? 14 : higherInt;

                    if (lowerInt > higherInt) {
                        int temp = lowerInt;

                        lowerInt = higherInt;
                        higherInt = temp;
                    }

                    return new int[] {7, cards[cardPosition].rankInt == 1 ? 14 : cards[cardPosition].rankInt, higherInt, lowerInt};
                }
            }
        }

        return DetermineTwoPair(gameName, cards);
    }

    //Returns int[] showing the rank of the higher pair first, int showing the rank of the lower two pair, int showing the rank of the high card, 4
    public static int[] DetermineTwoPair(string gameName, Card[] cards) {
        //1, 2, 3, 4
        int divide = 0;

        if (cards[3].rankInt != cards[4].rankInt) {
            //0, 1, 2, 3
            divide = 4;
        } else if (cards[1].rankInt != cards[2].rankInt && cards[2].rankInt != cards[3].rankInt) {
            //0, 1, 3, 4
            divide = 2;
        }

        int pairAmount = 0;
        int[] pairsArr = new int[2];

        for (int i = 0; i < cards.Length-1; i++) {
            if (cards[i].rankInt == cards[i+1].rankInt) {
                pairsArr[pairAmount] = cards[i].rankInt == 1 ? 14 : cards[i].rankInt;
                pairAmount++;
                i++;
            }
        }

        if (pairsArr[0] == 14) {
            pairsArr[0] = pairsArr[1];
            pairsArr[1] = 14;
        }

        if (pairAmount == 2) {
            //This doesn't work with two aces because it registers it as the lowest pair WOW SO SMART
            //int higherNum = pairsArr[0] > pairsArr[1] ? pairsArr[0] : pairsArr[1];
            //int lowerNum = pairsArr[0] < pairsArr[1] ? pairsArr[0] : pairsArr[1];

            return new int[] {8, pairsArr[1], pairsArr[0], cards[divide].rankInt == 1 ? 14 : cards[divide].rankInt};
        }

        return DeterminePair(gameName, cards);
    }

    //Returns int[] showing the rank of the pair, three ints showing the next three highest cards, 5
    public static int[] DeterminePair(string gameName, Card[] cards) {
        for (int i = 1; i < cards.Length; i++) {
            if (cards[i-1].rankInt == cards[i].rankInt) {
                int firstRank = 0;
                int secondRank = 0;
                int thirdRank = 0;

                switch (i) {
                    case 1:
                        firstRank = cards[2].rankInt;
                        secondRank = cards[3].rankInt;
                        thirdRank = cards[4].rankInt;

                        break;

                    case 2:
                        firstRank = cards[0].rankInt;
                        secondRank = cards[3].rankInt;
                        thirdRank = cards[4].rankInt;
                        
                        break;

                    case 3:
                        firstRank = cards[0].rankInt;
                        secondRank = cards[1].rankInt;
                        thirdRank = cards[4].rankInt;
                        
                        break;

                    case 4:
                        firstRank = cards[0].rankInt;
                        secondRank = cards[1].rankInt;
                        thirdRank = cards[2].rankInt;
                        
                        break;
                }

                firstRank = firstRank == 1 ? 14 : firstRank;
                secondRank = secondRank == 1 ? 14 : secondRank;
                thirdRank = thirdRank == 1 ? 14 : thirdRank;

                int temp = 0;

                if (secondRank > thirdRank) {
                    temp = thirdRank;

                    thirdRank = secondRank;
                    secondRank = temp;
                }

                if (firstRank > thirdRank) {
                    temp = thirdRank;

                    thirdRank = firstRank;
                    firstRank = temp;
                }

                if (firstRank > secondRank) {
                    temp = secondRank;

                    secondRank = firstRank;
                    firstRank = temp;
                }

                return new int[] {9, cards[i].rankInt == 1 ? 14 : cards[i].rankInt, thirdRank, secondRank, firstRank};
            }
        }

        int[] hand = new int[6];
        int[] protoHand = new int[5];
        hand[0] = 10;

        for (int i = 0; i < cards.Length; i++) {
            protoHand[i] = cards[i].rankInt == 1 ? 14 : cards[i].rankInt;
        }

        Array.Sort(protoHand,
            delegate(int x, int y) {
                return x - y;
            }
        );

        for (int i = 0; i < protoHand.Length; i++) {
            hand[i+1] = protoHand[i];
        }

        //High Card - int for every value of the board which will be used to compare other high cards to other player's hands, 5
        return hand;
    }

    public static int[] FiveCardHand(string gameName, Card[] cards) {
        Array.Sort(cards,
            delegate(Card x, Card y) { 
                return x.rankInt.CompareTo(y.rankInt); 
            }
        );

        return DetermineRoyalFlush(gameName, cards);
    }

    //Determines whether two poker hands are a tie
    public static bool DeterminePokerTie(int[] hand1, int[] hand2) {
        for (int i = 0; i < hand1.Length; i++) {
            if (hand1[i] != hand2[i]) {
                return false;
            }
        }

        return true;
    }

    //Determines which of two hands has the best hand
    public static int[] DetermineBestHand(int[] hand1, int[] hand2) {
        if (hand1[0] < hand2[0]) {
            return hand1;
        } else if (hand1[0] > hand2[0]) {
            return hand2;
        } else if (hand1[0] == hand2[0] && hand1[0] > 1) {
            //Flush and High card is a special case
            if (hand1[0] == 5 || hand1[0] == 10) {
                for (int i = hand1.Length-1; i >= 1; i--) {
                    if (hand1[i] > hand2[i]) {
                        return hand1;
                    } else if (hand1[i] < hand2[i]) {
                        return hand2;
                    }
                }
            }

            for (int i = 1; i < hand1.Length; i++) {
                if (hand1[i] > hand2[i]) {
                    return hand1;
                } else if (hand1[i] < hand2[i]) {
                    return hand2;
                }
            }
        }

        //Make this a tie but I don't know how to yet. Return tie between multiple players, return tie between 2 players, do split pots
        //Right now I'll just return hand1 if there is a tie. (empty array to represent a tie later??)
        //Fixed the tie with the DeterminePokerTie method above
        return hand1;
    }

    //Derermines the type of poker hand
    public static int[] DeterminePokerHand(string gameName, Card[] communityCards, Card[] playerCards) {
        int cardCount = 0;

        if (communityCards == null) {
            cardCount = playerCards.Length;
        } else if (playerCards == null) {
            cardCount = communityCards.Length;
        } else {
            cardCount = communityCards.Length + playerCards.Length;
        }

        switch (cardCount) {
            case 5:
                return FiveCardHand(gameName, communityCards);

            case 7:
                int[] bestHand = FiveCardHand(gameName, communityCards);

                for (int i = 0; i < communityCards.Length; i++) {
                    Card[] builtHand = CloneCards(communityCards);

                    for (int j = 0; j < playerCards.Length; j++) {
                        builtHand[i] = playerCards[j];

                        int[] nextHand = FiveCardHand(gameName, builtHand);

                        bestHand = DetermineBestHand(bestHand, nextHand);
                    }
                }

                int dontUseCard1 = 0;
                int dontUseCard2 = 1;

                for (int i = 0; i < communityCards.Length; i++) {
                    for (int j = 0; j < playerCards.Length; j++ ) {
                        Card[] builtHand = CloneCards(communityCards);

                        builtHand[dontUseCard1] = playerCards[0];
                        builtHand[dontUseCard2] = playerCards[1];

                        int[] nextHand = FiveCardHand(gameName, builtHand);

                        bestHand = DetermineBestHand(bestHand, nextHand);

                        dontUseCard2++;

                        if (dontUseCard2 >= communityCards.Length) {
                            dontUseCard1++;
                            dontUseCard2 = dontUseCard1+1;
                        }
                    }
                }

                return bestHand;
        }

        return new int[] {0};
    }

    //Determines what 21+3 Bet has been hit in Blackjack
    public static string Determine21_3(Card card1, Card card2, Card card3) {
        Card[] arr = {card1.Clone(), card2.Clone(), card3.Clone()};

        Array.Sort(arr,
            delegate(Card x, Card y) { 
                return x.pokerRankInt.CompareTo(y.pokerRankInt); 
            }
        );

        string hand = "None";

        for (int i = 0; i < 2; i++) {
            if ((arr[0].suit == arr[1].suit && arr[0].suit == arr[2].suit) && (arr[0].pokerRankInt == arr[1].pokerRankInt && arr[0].pokerRankInt == arr[2].pokerRankInt)) {
                return "Suited Three of a Kind";
            } else if ((arr[0].suit == arr[1].suit && arr[0].suit == arr[2].suit) && (arr[0].pokerRankInt == arr[1].pokerRankInt-1 && arr[0].pokerRankInt == arr[2].pokerRankInt-2)) {
                return "Straight Flush";
            } else if (arr[0].pokerRankInt == arr[1].pokerRankInt && arr[0].pokerRankInt == arr[2].pokerRankInt) {
                return "Three of a Kind";
            } else if (arr[0].pokerRankInt == arr[1].pokerRankInt-1 && arr[0].pokerRankInt == arr[2].pokerRankInt-2) {
                hand = "Straight";
            } else if (arr[0].suit == arr[1].suit && arr[0].suit == arr[2].suit) {
                hand = "Flush";
            }

            for (int j = 0; j < arr.Length; j++) {
                if (arr[j].pokerRankInt == 14) {
                    arr[j].pokerRankInt = 1;
                } else if (arr[j].pokerRankInt == 1) {
                    arr[j].pokerRankInt = 14;
                }
            }

            Array.Sort(arr,
                delegate(Card x, Card y) { 
                    return x.pokerRankInt.CompareTo(y.pokerRankInt); 
                }
            );
        }

        return hand;
    }

    //Determines what  perfecrt pair bet has been hit in Blackjack
    public static string DeterminePerfectPair(Card card1, Card card2) {
        if (card1.suit == card2.suit && card1.pokerRankInt == card2.pokerRankInt) {
            return "Perfect Pair";
        } else if (card1.color == card2.color && card1.pokerRankInt == card2.pokerRankInt) {
            return "Colored Pair";
        } else if (card1.pokerRankInt == card2.pokerRankInt) {
            return "Mixed Pair";
        }

        return "None";
    }

    //Returns a deep copy of the array sent
    public static Card[] CloneCards(Card[] cards) {
        Card[] arr = new Card[5];

        for (int i = 0; i < cards.Length; i++) {
            arr[i] = cards[i].Clone();
        }

        return arr;
    }

    public static System.Random random {
        get {
            return _random;
        }
    }

    public static decimal money {
        get {
            return _money;
        }

        set {
            _money = Math.Round(value, 2);
        }
    }

    public static Deck deck {
        get {
            return _deck;
        }
    }
}
