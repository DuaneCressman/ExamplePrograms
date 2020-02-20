/*
 Programmer: Duane Cressman
 Date Created: November 2019 
 */

using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SlidingTilesGame
{
    public static class TileHandler
    {
        private const int BoardSize = 4;

        //Check if the board is in a win state
        public static int CheckWin(List<myTile> inList)
        {
            int correctTiles = 0;

            foreach (myTile x in inList)
            {
                //check if the current coordinate is the same as the original
                if (x.chrdX == x.OriginalX && x.chrdY == x.OrginalY)
                {
                    correctTiles++;
                }
            }

            return correctTiles;
        }

        //update the list of tiles when one is moved. Pass in the coordinates of the one that is used
        public static List<myTile> UpdateTile(List<myTile> inList, int blankX, int blankY)
        {
            foreach (myTile x in inList)
            {
                //Check that the tile corresponds to an actual image
                if (x.TileImage != null)
                {
                    //set the coordinate of each tile based on its position on the canvas
                    x.chrdX = myTile.BoardToLogic(Canvas.GetLeft(x.TileImage), 'w');
                    x.chrdY = myTile.BoardToLogic(Canvas.GetTop(x.TileImage), 'h');
                    x.Move = 0;
                }
                else
                {
                    //this is the blank tile, set it equal to the old coordinates that were passed in.
                    x.chrdX = blankX;
                    x.chrdY = blankY;
                }
            }

            //update the movable blocks and return the list
            return UpdateMovableBlocks(inList, blankX, blankY);
        }

        //update all of the blocks that are able to move
        public static List<myTile> UpdateMovableBlocks(List<myTile> outList, int EmptyX, int EmptyY)
        {
            //get the coordinates of the tiles around the blank block
            int TileAbove = myTile.GetTile(EmptyX, EmptyY - 1, outList);
            int TileBelow = myTile.GetTile(EmptyX, EmptyY + 1, outList);
            int TileLeft = myTile.GetTile(EmptyX - 1, EmptyY, outList);
            int TileRight = myTile.GetTile(EmptyX + 1, EmptyY, outList);

            //check if the tile above is a valid tile, and update its move options
            if (TileAbove != -1)
            {
                outList[TileAbove].Move = 2;
                outList[TileAbove].Direct = false;
            }

            //check if the tile below is a valid tile, and update its move options
            if (TileBelow != -1)
            {
                outList[TileBelow].Move = 2;
                outList[TileBelow].Direct = true;
            }

            //check if the tile right is a valid tile, and update its move options
            if (TileRight != -1)
            {
                outList[TileRight].Move = 1;
                outList[TileRight].Direct = false;
            }

            //check if the tile left is a valid tile, and update its move options
            if (TileLeft != -1)
            {
                outList[TileLeft].Move = 1;
                outList[TileLeft].Direct = true;
            }

            //return the list
            return outList;
        }
    }
}
