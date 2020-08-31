using System;

namespace DAN_LVIII_Milica_Karetic
{
    /// <summary>
    /// Class for game simulation
    /// </summary>
    class Game
    {

        public bool PlayerTurn { get; set; }
        public bool GameState { get; private set; }
        private int maxSize { get; set; } = 9;
        private bool foundWinPattern { get; set; }
        public short maxRowSize { get; private set; } = 3;
        public short maxcolSize { get; private set; } = 3;

        //win pattern (length 3)
        public short[] winSegments;

        //winner
        public IdentifyWinner winner;


        /// <summary>
        /// state of board spots - empty, x or o
        /// </summary>
        public enum SpotState
        {
            empty,
            X,
            O
        }

        /// <summary>
        /// winner options
        /// </summary>
        public enum IdentifyWinner
        {
            NULL,
            draw,
            player,
            computer
            
        }

        public SpotState[] spotValues;

        /// <summary>
        /// Game constructor
        /// </summary>
        public Game()
        {
            winSegments = new short[3] { 0, 0, 0 };
            spotValues = new SpotState[9];
            winner = new IdentifyWinner();
            foundWinPattern = false;
            PlayerTurn = true;
            GameState = true;
        }

        /// <summary>
        /// initialize board with all empty spots
        /// </summary>
        public void InitialBoard()
        {
            //all empty spots
            for (int i = 0; i < maxSize; ++i)
            {
                spotValues[i] = SpotState.empty;
            }
            //first turn is player's turn
            winner = IdentifyWinner.NULL;
            foundWinPattern = false;
            PlayerTurn = true;
            GameState = true;
        }


        /// <summary>
        /// computer playing
        /// </summary>
        /// <returns></returns>
        public int ComputerPlay()
        {

            int index = 0;

            short? val = CPUPlay();

            if (val != null)
            {
                index = (int)val;
                return index;
            }

            //getting random spot until it matches the empty one
            while (true)
            {
                Random random = new Random();
                index = random.Next(0, 8);

                if (spotValues[index] == SpotState.empty)
                    break;
            }

            return index;
        }


        /// <summary>
        /// Checks if someone won
        /// </summary>
        public void CheckGameState()
        {
            if (winner == IdentifyWinner.NULL)
            {
                //if there is empty spots game continues else game is over
                foreach (var spot in spotValues)
                {
                    if (spot == Game.SpotState.empty)
                    {
                        GameState = true;
                        break;
                    }
                    else
                        GameState = false;
                }

                //if there is no empty spots and winner is null -> it's draw
                if (GameState != true)
                    winner = IdentifyWinner.draw;

            }
        }

        /// <summary>
        /// Get winner of game
        /// </summary>
        /// <param name="tempstate"></param>
        public void GetWinner(SpotState spotstate)
        {
            if (GameState != false)
            {
                GetWin(spotstate);

                if (foundWinPattern)
                {
                    if (spotstate == SpotState.X)
                        winner = IdentifyWinner.player;

                    else if (spotstate == SpotState.O)
                        winner = IdentifyWinner.computer;
                }

                CheckGameState();
            }

        }

        /// <summary>
        /// method for computers turn in game
        /// </summary>
        /// <returns></returns>
        private short? CPUPlay()
        {

            short count = 0;
            short? CPUval = null;

            #region Horizontal

            short temp = maxRowSize;
            short temp2 = 0;

            for (short j = 0; j < maxRowSize; ++j)
            {
                for (short i = temp2; i < temp; ++i)
                {
                    if (spotValues[i] == SpotState.X)
                        ++count;
                    else
                        CPUval = i;
                }

                if (count == 2 && spotValues[(int)CPUval] != SpotState.O)
                    break;

                count = 0;
                temp += maxRowSize;
                temp2 += 2 + 1;
            }

            if (count == 2 && CPUval != null)
                return (short)CPUval;

            #endregion

            #region Vertical

            count = 0;
            temp = 6;
            temp2 = 0;
            short temp3 = temp2;

            for (short j = 0; j < maxcolSize; ++j)
            {
                for (short i = temp2; i <= temp; i += 3)
                {
                    if (spotValues[i] == SpotState.X)
                        ++count;
                    else
                        CPUval = i;
                }

                if (count == 2)
                    break;

                count = 0;
                temp += 1;
                temp3 += 1;
                temp2 = temp3;
            }

            if (count == 2 && CPUval != null)
                return (short)CPUval;

            #endregion

            #region diagonal

            count = 0;
            temp = 8;
            temp2 = 0;
            short incr = 4;

            for (short j = 0; j < maxcolSize - 1; ++j)
            {
                for (short i = temp2; i <= temp; i += incr)
                {
                    if (spotValues[i] == SpotState.X)
                        ++count;
                    else
                        CPUval = i;
                }

                if (count == 2)
                    break;

                count = 0;
                temp -= 2;
                temp2 += 2;
                incr -= 2;
            }

            if (count == 2 && CPUval != null) return (short)CPUval;

            #endregion

            return null;
        }

        /// <summary>
        /// chechs if someone win
        /// </summary>
        /// <param name="boxState"></param>
        private void GetWin(SpotState boxState)
        {

            #region Horizontalcheck

            short segindex = 0;
            short temp = maxRowSize;
            short temp2 = 0;

            for (short j = 0; j < maxRowSize; ++j)
            {
                for (short i = temp2; i < temp; ++i)
                {
                    if (spotValues[i] != boxState)
                    {
                        GameState = true;
                        foundWinPattern = false;
                        break;
                    }
                    else if (spotValues[i] == boxState)
                    {
                        winSegments[segindex++] = i;
                        GameState = false;
                        foundWinPattern = true;
                    }
                }

                if (foundWinPattern)
                    break;

                segindex = 0;
                temp += maxRowSize;
                temp2 += 2 + 1;
            }

            #endregion

            if (foundWinPattern)
                return;

            #region VerticalCheck

            segindex = 0;
            temp = 6;
            temp2 = 0;
            short temp3 = temp2;

            for (short j = 0; j < maxcolSize; ++j)
            {
                for (short i = temp2; i <= temp; i += 3)
                {
                    if (spotValues[i] != boxState)
                    {
                        GameState = true;
                        foundWinPattern = false;
                        break;
                    }

                    else if (spotValues[i] == boxState)
                    {
                        winSegments[segindex++] = i;
                        GameState = false;
                        foundWinPattern = true;
                    }
                }

                if (foundWinPattern)
                    break;

                segindex = 0;
                temp += 1;
                temp3 += 1;
                temp2 = temp3;
            }

            #endregion

            //if winner pattern for horizontal and vertical patterns is found return
            if (foundWinPattern)
                return;

            //chech diagonals
            #region diagonalCheck

            segindex = 0;
            temp = 8;
            temp2 = 0;
            short incr = 4;

            for (short j = 0; j < maxcolSize - 1; ++j)
            {
                for (short i = temp2; i <= temp; i += incr)
                {
                    if (spotValues[i] != boxState)
                    {
                        GameState = true;
                        foundWinPattern = false;
                        break;
                    }
                    else if (spotValues[i] == boxState)
                    {
                        winSegments[segindex++] = i;
                        GameState = false;
                        foundWinPattern = true;
                    }
                }

                //if pattern is found
                if (foundWinPattern)
                    break;

                segindex = 0;
                temp -= 2;
                temp2 += 2;
                incr -= 2;
            }
            #endregion
        }


    }
}
