﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;

/// <summary>
/// Keeps track of the global state of the game.
/// </summary>
public class GameController : MonoBehaviour
{
    private int score;

    /// <summary>
    /// Current score value. In this case, the number of cookies that have
    /// been collected.
    /// </summary>
    public int Score
    {
        get
        {
            return score;
        }
        private set
        {
            this.score = value;

            // Notify score changed
            if (this.OnScoreChanged != null)
            {
                this.OnScoreChanged(this, new ScoreChangedEventArgs(value));
            }
        }
    }

    public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

    public class ScoreChangedEventArgs : EventArgs
    {
        public readonly int score;

        public ScoreChangedEventArgs(int newScore)
        {
            this.score = newScore;
        }
    }

    private ItemShop itemShop;
    public ItemShop ItemShop
    {
        get
        {
            if (this.itemShop == null)
            {
                this.itemShop = new ItemShop(this);
            }
            return this.itemShop;
        }
    }

    public ObservableList<IItem> Items { get; private set; }
        = new ObservableList<IItem>();

    private void Update()
    {
        foreach (var item in this.Items)
        {
            this.Score += item.Update(Time.time);
        }
    }

    /// <summary>
    /// "Click" to collect a cookie.
    /// </summary>
    public void Click()
    {
        // Add one to score.
        this.Score++;

        // Add to score based on current items.
        foreach (var item in this.Items)
        {
            this.Score += item.HandleClick();
        }
    }

    public void BuyItem(int price, IItem newItem)
    {
        this.Items.Add(newItem);
        this.Score -= price;
    }
}
