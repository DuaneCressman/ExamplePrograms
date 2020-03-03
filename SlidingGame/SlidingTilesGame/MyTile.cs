/*
 Programmer: Duane Cressman
 Date Created: November 2019 
 */

using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SlidingTilesGame
{
    public class myTile
    {
        public string ID;        //Name of the tile
        public Image TileImage;  //The image defined in the xaml
        public int Move;         //The direction that the tile can move
        public bool Direct;      //If the tile is moving in a positive or negative 
        public int chrdX;        //The current location of the block
        public int chrdY;        //The current location of the block
        public int OriginalX;    //The original location of the block (used for winning)
        public int OrginalY;     //The original location of the block (used for winning)

        //Constructor
        public myTile(string id, Image inborder, double inX, double inY)
        {
            ID = id;
            TileImage = inborder;

            Move = 0;
            Direct = false;

            chrdX = BoardToLogic(inX, 'w');
            chrdY = BoardToLogic(inY, 'h');

            OrginalY = chrdY;
            OriginalX = chrdX;
        }

        //Converts a coordinate in a tile to an actual point on the canvas
        public static double LogicToBoard(int input, char hw)
        {
            if (hw == 'h')
            {
                return input * Constants.BlockHeight;
            }
            else if (hw == 'w')
            {
                return input * Constants.BlockWidth;
            }

            return -1;
        }

        //Converts an actual point on the canvas to a coordinator that can be used by the logic
        public static int BoardToLogic(double input, char hw)
        {
            if (hw == 'h')
            {
                return (int)(input / Constants.BlockHeight);
            }
            else if (hw == 'w')
            {
                return (int)(input / Constants.BlockWidth);
            }

            return -1;
        }

        //Get the index of a tile in a list of tiles based on xy coordinates
        public static int GetTile(int cX, int cY, List<myTile> inList)
        {
            for (int i = 0; i < inList.Count(); i++)
            {
                if (inList[i].chrdX == cX && inList[i].chrdY == cY)
                {
                    return i;
                }
            }

            return -1;
        }

        //get a tile from a list of tiles based on an id.
        public static myTile GetFromName(string inName, List<myTile> inList)
        {
            foreach (myTile x in inList)
            {
                if (x.ID == inName)
                {
                    return x;
                }
            }

            return null;
        }
    }
}
