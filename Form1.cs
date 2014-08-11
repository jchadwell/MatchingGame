using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {

        #region Declarations
        // Declare fonts
        Font defaultFont = new System.Drawing.Font(new FontFamily("arial"), 10, FontStyle.Bold);
        Font lblFont = new System.Drawing.Font(new FontFamily("wingdings"), 22, FontStyle.Regular);

        // Gameplay vars
        int numAttempts = 0, numCorrect = 0, pieces = 0, cols = 2, rows = 2, pairs = 0, modifyBoard = 2;

        // Loop / random vars
        int i = 0, j = 0, count = 0, intial = 0, increment = 0, minRange = 33, maxRange = 88, incValue = 5;

        // Layout
        int form_width = 0, form_height = 0, piece_width = 45, piece_height = 45,
            lMargin = 25, rMargin = 25, tMargin = 25, bMargin = 25, vSpacer = 10,
            hSpacer = 10;

        // String array to hold the piece values
        string[] piece;

        // Labels used during game play
        Label lbl_title, lbl_attempts, lbl_correct, lbl_win, lbl_lose;

        // Labels used to check user matches
        Label firstSelection = null, secondSelection = null;

        // Labels array to hold the game labels that will be the "match cards"
        Label[,] gameLabels;

        // Timer used to delay action after 2 matches selected
        // so that the user can see the result of the second
        // match they selected
        Timer delay;

        // Timer used to delay creating or restarting the game board after
        // the player wins or loses
        Timer restart;

        // Random
        Random rand = new Random();

        // bool to tell if the game is over
        bool gameOver = false;

        #endregion

        #region Initialization

        public Form1()
        {
            // Initialize the timers
            delay = new Timer();
            delay.Enabled = false;
            delay.Interval = 550;
            delay.Tick += new System.EventHandler(this.delay_Tick);

            restart = new Timer();
            restart.Enabled = false;
            restart.Interval = 550;
            restart.Tick += new System.EventHandler(this.restart_Tick);
 
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void delay_Tick(object sender, EventArgs e)
        {
            // After the timer has ran a full interval have it stop
            delay.Stop();

            // check the first and second selection to see if their
            // text values match.
            if (firstSelection.Text == secondSelection.Text)
            {
                // clear the matches from view
                firstSelection.Visible = false;
                secondSelection.Visible = false;

                // increment the number of correct matches
                numCorrect++;
            }
            // else they do not match
            else
            {
                // reset text of both to null strings
                firstSelection.Text = "";
                secondSelection.Text = "";

                // decrement the number of attempts
                numAttempts--;
            }

            // set the first and second selection to null
            firstSelection = null;
            secondSelection = null;

            // update the attempts and correct labels
            lbl_attempts.Text = "Attempts: " + numAttempts.ToString();
            lbl_correct.Text = "Correct: " + numCorrect.ToString();

            // if all the pairs matched
            if (pairs == numCorrect)
            {
                //show the win label
                lbl_win.Visible = true;
                // set the game is over
                gameOver = true;
                // start the restart timer
                restart.Start();
            }

            // if the user runs out of attempts
            if (numAttempts <= 0)
            {
                // show the lose label
                lbl_lose.Visible = true;
                // set the game is over
                gameOver = true;
                // start the restart timer
                restart.Start();
            }
        }

        private void restart_Tick(object sender, EventArgs e)
        {
            // After the timer has ran a full interval have it stop
            restart.Stop();

            // if the game is over && the player has lost
            if (gameOver && lbl_lose.Visible)
            {
                // remove the labels from the board
                ClearBoard();

                // create a new board with the same rows and cols
                // as the previous board
                createBoard(cols, rows);
                return;
            }
            // else if the game is over && and the player has won
            else if (gameOver && lbl_win.Visible)
            {
                // remove the labels from the board
                ClearBoard();

                // create a new board with the new amound of pieces
                createBoard(cols * modifyBoard, rows * modifyBoard);
                return;
            }
        }

        private void ClearBoard()
        {
            // loop through the game labels
            foreach (Label l in gameLabels)
            {
                // remove the labels from the board.
                // if you do not remove the labls they
                // will just be set to not visible and you
                // will be adding new labels on top of invisible
                // labels if you win, or if you lose the old labels
                // both the invis and the ones that were left vis
                // will be under the new labels and the vis ones
                // will be seen and are still able to be clicked
                // like a game piece.
                Controls.Remove(l);
            }
        }

        #endregion
    }
}
