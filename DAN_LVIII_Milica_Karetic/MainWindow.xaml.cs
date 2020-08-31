using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DAN_LVIII_Milica_Karetic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();

            newGame();
        }

        private void newGame()
        {
            game.InitialBoard();

            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
            });
        }

        /// <summary>
        /// event handler for button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            //get row and column from button clicked
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var gridIndex = column + (row * 3);

            //if it's players turn
            if (game.PlayerTurn && game.GameState)
            {
                //if spot isn't empty
                if (game.spotValues[gridIndex] != Game.SpotState.empty)
                {
                    MessageBox.Show("Spot is taken!");
                }
                //else if it is empty and its players turn 
                else
                {
                    game.spotValues[gridIndex] = Game.SpotState.X;
                    button.Content = 'X';
                    game.PlayerTurn = false;
                }
                game.getWinner(Game.SpotState.X);
            }

            //if it is computer's turn
            if (!game.PlayerTurn && game.GameState)
            {
                var btn = Container.Children.Cast<Button>().ToArray();
                var index = game.computerPlay();
                btn[index].Content = 'O';

                game.spotValues[index] = Game.SpotState.O;
                game.PlayerTurn = true;
                game.getWinner(Game.SpotState.O);
            }

            if (game.GameState != true)
            {
                //if someone won
                if (game.winner == Game.IdentifyWinner.player || game.winner == Game.IdentifyWinner.computer)
                {
                    var btn = Container.Children.Cast<Button>().ToArray();

                    for (int i = 0; i < game.maxRowSize; ++i)
                    {
                        btn[game.winSegments[i]].Background = Brushes.Green;
                    }
                    MessageBox.Show(game.winner.ToString() + " won!");
                    this.newGame();
                }

                if (game.winner == Game.IdentifyWinner.draw)
                {
                    Container.Children.Cast<Button>().ToList().ForEach(btn =>
                    {
                        button.Foreground = Brushes.Black;
                    });

                    MessageBox.Show("It's " + game.winner.ToString());
                    this.newGame();
                }


            }
        }
    }
}
