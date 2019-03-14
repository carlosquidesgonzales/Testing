using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texas_Holdem.Library.Classes;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.UI
{
   
    public partial class Form1 : Form
    {
        
        Table _table;     
        List<string> playerNames;

        private List<Label> _playerCardLabels;
        private List<Label> _dealerCardLabels;

        private Point[] _playerLblPos;
        private Point[] _dealerLblPos;
        


        public Form1()
        {
            InitializeComponent();
           
        }

        private void btnNewTable_Click(object sender, EventArgs e)
        {

            
            try
            {
                
                ClearPlayerNames();

                ClearPlayerCardsFromTable();
                ClearDealerCardsFromTable();
               
                playerNames = new List<string>();
               
                for (int i = 1; i <= 4; i++)
                {
                    TextBox playerTextBox = (TextBox)this.Controls.Find("txtPlayerName" + (i), true)[0];
                    Label currentLabel1 = (Label)this.Controls.Find("lblPlayerName" + (i), true)[0];
                    if (playerTextBox.Text != String.Empty)
                    {
                        currentLabel1.Text = playerTextBox.Text;
                        playerNames.Add(playerTextBox.Text);
                    }
                        
                }
                                      
                ClearHandLabels();
                ClearPlayerCardsFromTable();

                _table = new Table(playerNames.ToArray());
         
                ClearHandLabels();
                
            }
            catch (ArgumentException)
            {
                
            }
        }

        public void ClearHandLabels()
        {
            lblHand1.Text = String.Empty;
            lblHand2.Text = String.Empty;
            lblHand3.Text = String.Empty;
            lblHand4.Text = String.Empty;

            btnDrawCard.Enabled = false;
            btnNewHand.Enabled = true;
            lblWinner.Hide();
        }

        private void ClearPlayerNames()
        {
            
            lblPlayerName1.Text = String.Empty;
            lblPlayerName2.Text = String.Empty;
            lblPlayerName3.Text = String.Empty;
            lblPlayerName4.Text = String.Empty;

        }

        private void Form_Load(object sender, EventArgs e)
        {

            try
            {
                _playerCardLabels = new List<Label>();
                _dealerCardLabels = new List<Label>();

            }
            catch (ArgumentException)
            {
                
            }          
        }

        private Label CreateCard(int x, int y, Card card)
        {
            Label lbl = new Label();
            lbl.Text = card.Output;
            lbl.Size = new Size(45, 60);
            lbl.Location = new Point(x, y);
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Font = new Font("Consolas", 15);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.BackColor = Color.White;
            lbl.ForeColor = card.Suit.Equals(Suits.Hearts) || card.Suit.Equals(Suits.Diamonds) ? Color.Red : Color.Black;
            return lbl; 
        }

        private void btnNewHand_Click(object sender, EventArgs e)
        {
            if (_table.Equals(null)) return;

            ClearHandLabels();
            ClearPlayerCardsFromTable();

            _table.DealNewHand();
            DisplayPlayerHands();

            ClearDealerCardsFromTable();

            _table.EvaluatePlayerHands();
            FillHandValueLabels();

            btnDrawCard.Enabled = true;
            lblWinner.Hide();
            
        }

        private void btnDrawCard_Click(object sender, EventArgs e)
        {
            if (_table.Dealer.CardCount.Equals(0))
            {
                _table.DealerDrawsCard(3);
            }

            else if (_table.Dealer.CardCount >= 3)
            {
                _table.DealerDrawsCard();
                //_table.EvaluatePlayerHands(); //Card Leak
            }

            DisplayDealerCards();
            _table.EvaluatePlayerHands();


            if (_table.Dealer.CardCount.Equals(5))
            {
                _table.EvaluatePlayerHands();
                btnDrawCard.Enabled = false;
                FillHandValueLabels();
               
                DisplayWinner();
                btnNewHand.Enabled = true;
                btnNewTable.Enabled = true;
               
                
                
                for (int i = 0; i <= 3; i++)
                {
                    TextBox playerTextBox = (TextBox)this.Controls.Find("txtPlayerName" + (i + 1), true)[0];                  
                        playerTextBox.Enabled = true;
                }

                
                //btnNewGame.Visible = true;
                //btnNewGame.Enabled = true;
            }
               
        }

        private void DisplayDealerCards()
        {
            _dealerLblPos = new Point[]
            {
                new Point(320, 265), new Point(370, 265),
                new Point(420, 265), new Point(470, 265),
                new Point(520, 265)
            };


            ClearDealerCardsFromTable();
            var i = 0;
            foreach (var dealer in _table.Dealer.Cards)
            {
                _dealerCardLabels.Add(CreateCard(_dealerLblPos[i].X, _dealerLblPos[i].Y, dealer));
                i++;
            }
            foreach (var lbl in _dealerCardLabels)
            {
                Controls.Add(lbl);
            }
        }

        private void DisplayPlayerHands()
        {
            _playerLblPos = new Point[]
            {
                new Point(630, 265), new Point(680, 265),
                new Point(500, 365), new Point(550, 365),
                new Point(300, 365), new Point(350, 365),
                new Point(160, 265), new Point(210, 265)

            };

            var p = 0;
            var counter = 0;          
            for (int i = 1; i <= 4; i++)
            {
                TextBox playerTextBox = (TextBox)this.Controls.Find("txtPlayerName" + (i), true)[0];
                Label handLabel = (Label)this.Controls.Find("lblHand" + (i), true)[0];

                if (playerTextBox.Text != String.Empty)
                {
                    _table.Players[p].PlayerCards.Add(_table.Players[p].Cards.ElementAt(0));
                    _table.Players[p].PlayerCards.Add(_table.Players[p].Cards.ElementAt(1));                 
                    for (int l = 0; l <= 1; l++)
                    {
                        _playerCardLabels.Add(CreateCard(_playerLblPos[l+counter].X, _playerLblPos[l+counter].Y, _table.Players[p].PlayerCards[l]));                       
                    }
                    p++;
                }
                counter += 2;
            }
            
            foreach (Label lbl in _playerCardLabels)
            {
                this.Controls.Add(lbl);
            }
        }

        private void ClearPlayerCardsFromTable()
        {

            foreach (var control in _playerCardLabels)
            {
                Controls.Remove(control);
                //control.Controls.Clear();
            }
            _playerCardLabels.Clear();
        }

        private void ClearDealerCardsFromTable()
        {
            foreach (var control in _dealerCardLabels)
            {
                Controls.Remove(control);
                //control.Controls.Clear();
            }
            _dealerCardLabels.Clear();
        }

        private void FillHandValueLabels()
        {
            int player = 0;
                for (int i = 1; i <= 4; i++)
                {
                    TextBox playerTextBox = (TextBox)this.Controls.Find("txtPlayerName" + (i), true)[0];
                    Label handLabel = (Label)this.Controls.Find("lblHand" + (i), true)[0];
                    
                        if (playerTextBox.Text != String.Empty)
                        {
                            handLabel.Text = _table.Players[player].HandValue.ToString();
                            player++;
                        }
                }     
        }

        private void DisplayWinner()
        {
            var win = _table.DetermineWinner();
            lblWinner.Show();
            if (win.Count == 0 || win.Count > 1)
            {
                lblWinner.Text = "It's a Draw !";
            }
            else 
            {
                lblWinner.Text = "Congratiolations " + win.ElementAt(0).Name + " !" +"\n\n" +"You got " + win[0].HandValue;
            }
          
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
