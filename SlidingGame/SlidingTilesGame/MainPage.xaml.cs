/*
 Programmer: Duane Cressman
 Date Created: November 2019 
 */

using System;
using System.Collections.Generic;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SlidingTilesGame
{
    public sealed partial class MainPage : Page
    {
        public static List<Image> theBlocks = new List<Image>();
        public List<Image> ImageList = new List<Image>();
        public BitmapImage[,] blocks;
        public List<myTile> theTiles = new List<myTile>();


        public MainPage()
        {
            this.InitializeComponent();

            //reset the list
            theBlocks.Clear();

            //get all the images from the screen
            theBlocks.Add(IRow0Column0);
            theBlocks.Add(IRow0Column1);
            theBlocks.Add(IRow0Column2);
            theBlocks.Add(IRow0Column3);
            theBlocks.Add(IRow1Column0);
            theBlocks.Add(IRow1Column1);
            theBlocks.Add(IRow1Column2);
            theBlocks.Add(IRow1Column3);
            theBlocks.Add(IRow2Column0);
            theBlocks.Add(IRow2Column1);
            theBlocks.Add(IRow2Column2);
            theBlocks.Add(IRow2Column3);
            theBlocks.Add(IRow3Column0);
            theBlocks.Add(IRow3Column1);
            theBlocks.Add(IRow3Column2);

            //start splitting up the images
            GetImageAsync();
        }


        public async void GetImageAsync()
        {
            StorageFile ThePhotoFile = null;

            //open the image based on how the user choose
            if (Constants.CameraOrFile == 1)
            {
                //open the camera ui and load the photo
                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                ThePhotoFile = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            }
            else if (Constants.CameraOrFile == 2)
            {
                //open the file system
                FileOpenPicker FileDialog = new FileOpenPicker();
                FileDialog.ViewMode = PickerViewMode.Thumbnail;
                FileDialog.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                FileDialog.FileTypeFilter.Add(".jpeg");
                FileDialog.FileTypeFilter.Add(".jpg");
                FileDialog.FileTypeFilter.Add(".png");
                FileDialog.FileTypeFilter.Add(".bmp");
                ThePhotoFile = await FileDialog.PickSingleFileAsync();
            }
            else if (Constants.CameraOrFile == 3)
            {
                //open a default picture
                var myURI = new Uri("ms-appx:///Assets/DefaultNumbers.png");
                ThePhotoFile = await StorageFile.GetFileFromApplicationUriAsync(myURI);
            }

            if (ThePhotoFile != null)
            {
                //send the image to be split up
                DecodeImage(ThePhotoFile);
            }
            else
            {
                //if null, send the user back to make another selection
                GoToLaunchPage();
            }

        }

        //send the user back to the launch page
        public void GoToLaunchPage()
        {
            this.Frame.Navigate(typeof(Launch_Page));
        }

        //decode the image and send it to be divided into the smaller chunks
        public async void DecodeImage(StorageFile inputFile)
        {
            if (inputFile != null)
            {
                //using a stream, create a decoder from the image
                using (IRandomAccessStream fileStream = await inputFile.OpenAsync(FileAccessMode.Read))
                {
                    //create the decoder
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                    SplitImage(decoder);
                }
            }

            //set the image up om the canvas
            ConfigureImagesOnCanvas();
        }


        private async void SplitImage(BitmapDecoder decoder)
        {
            //get the size of the image
            var inHeight = decoder.PixelHeight / 4;
            var inWidth = decoder.PixelWidth / 4;

            //save these values to be used elsewhere in the code
            Constants.BlockHeight = inHeight;
            Constants.BlockWidth = inWidth;

            //Split the image 16 times
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //Set up streams and the encoder
                    InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
                    BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

                    //set the bounds for the part of the image
                    BitmapBounds bounds = new BitmapBounds();
                    bounds.Height = inHeight;
                    bounds.Width = inWidth;

                    //set the bounds to the part of the image
                    bounds.X = 0 + inWidth * (uint)j;
                    bounds.Y = 0 + inHeight * (uint)i;

                    enc.BitmapTransform.Bounds = bounds;

                    try
                    {
                        //Actually do the chopping
                        await enc.FlushAsync();
                    }
                    catch (Exception e)
                    {
                        string s = e.ToString();
                    }

                    //put the image section into a bitmap
                    BitmapImage bImg = new BitmapImage();
                    bImg.SetSource(ras);

                    //set the image section into a public string
                    if (!(i == 3 && j == 3))
                    {
                        theBlocks[i * 4 + j].Source = bImg;
                    }
                }
            }
        }

        //so now we have the image data from the system, and it is split into all the pieces.
        //we will now put it onto the canvas
        public void ConfigureImagesOnCanvas()
        {

            //set the hight and width so that they are valid sizes
            while (Constants.BlockHeight > 150 || Constants.BlockWidth > 150)
            {
                Constants.BlockHeight = Constants.BlockHeight * 0.8;
                Constants.BlockWidth = Constants.BlockWidth * 0.8;
            }

            //for each image on the canvas, set its position in the canvas, and its hight and width
            IRow0Column0.Height = Constants.BlockHeight;
            IRow0Column0.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow0Column0, 0 * Constants.BlockWidth);
            Canvas.SetTop(IRow0Column0, 0 * Constants.BlockHeight);

            IRow0Column1.Height = Constants.BlockHeight;
            IRow0Column1.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow0Column1, 1 * Constants.BlockWidth);
            Canvas.SetTop(IRow0Column1, 0 * Constants.BlockHeight);

            IRow0Column2.Height = Constants.BlockHeight;
            IRow0Column2.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow0Column2, 2 * Constants.BlockWidth);
            Canvas.SetTop(IRow0Column2, 0 * Constants.BlockHeight);

            IRow0Column3.Height = Constants.BlockHeight;
            IRow0Column3.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow0Column3, 3 * Constants.BlockWidth);
            Canvas.SetTop(IRow0Column3, 0 * Constants.BlockHeight);

            IRow1Column0.Height = Constants.BlockHeight;
            IRow1Column0.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow1Column0, 0 * Constants.BlockWidth);
            Canvas.SetTop(IRow1Column0, 1 * Constants.BlockHeight);

            IRow1Column1.Height = Constants.BlockHeight;
            IRow1Column1.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow1Column1, 1 * Constants.BlockWidth);
            Canvas.SetTop(IRow1Column1, 1 * Constants.BlockHeight);

            IRow1Column2.Height = Constants.BlockHeight;
            IRow1Column2.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow1Column2, 2 * Constants.BlockWidth);
            Canvas.SetTop(IRow1Column2, 1 * Constants.BlockHeight);

            IRow1Column3.Height = Constants.BlockHeight;
            IRow1Column3.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow1Column3, 3 * Constants.BlockWidth);
            Canvas.SetTop(IRow1Column3, 1 * Constants.BlockHeight);

            IRow2Column0.Height = Constants.BlockHeight;
            IRow2Column0.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow2Column0, 0 * Constants.BlockWidth);
            Canvas.SetTop(IRow2Column0, 2 * Constants.BlockHeight);

            IRow2Column1.Height = Constants.BlockHeight;
            IRow2Column1.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow2Column1, 1 * Constants.BlockWidth);
            Canvas.SetTop(IRow2Column1, 2 * Constants.BlockHeight);

            IRow2Column2.Height = Constants.BlockHeight;
            IRow2Column2.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow2Column2, 2 * Constants.BlockWidth);
            Canvas.SetTop(IRow2Column2, 2 * Constants.BlockHeight);

            IRow2Column3.Height = Constants.BlockHeight;
            IRow2Column3.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow2Column3, 3 * Constants.BlockWidth);
            Canvas.SetTop(IRow2Column3, 2 * Constants.BlockHeight);

            IRow3Column0.Height = Constants.BlockHeight;
            IRow3Column0.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow3Column0, 0 * Constants.BlockWidth);
            Canvas.SetTop(IRow3Column0, 3 * Constants.BlockHeight);

            IRow3Column1.Height = Constants.BlockHeight;
            IRow3Column1.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow3Column1, 1 * Constants.BlockWidth);
            Canvas.SetTop(IRow3Column1, 3 * Constants.BlockHeight);

            IRow3Column2.Height = Constants.BlockHeight;
            IRow3Column2.Width = Constants.BlockWidth;
            Canvas.SetLeft(IRow3Column2, 2 * Constants.BlockWidth);
            Canvas.SetTop(IRow3Column2, 3 * Constants.BlockHeight);

            //set up events for each image
            foreach (Image b in theBlocks)
            {
                //set up the events
                b.ManipulationDelta += new ManipulationDeltaEventHandler(ManipulateMe_ManipulationDelta);
                b.ManipulationCompleted += new ManipulationCompletedEventHandler(ManipulateMe_ManipulationCompleted);

                b.ManipulationMode =
                ManipulationModes.TranslateX |
                ManipulationModes.TranslateY;
            }

            //Create a list of myTile Objects from the list of images
            //each mytile will hold the image object as well as game information about this tile.
            for (int i = 0; i < 15; i++)
            {
                myTile temp = new myTile(theBlocks[i].Name.ToString(), theBlocks[i], Canvas.GetLeft(theBlocks[i]), Canvas.GetTop(theBlocks[i]));
                theTiles.Add(temp);
            }

            //add in the empty tile
            theTiles.Add(new myTile("E", null, Constants.BlockWidth, Constants.BlockHeight));

            //update all positions on canvas and the movement of the tiles
            theTiles = TileHandler.UpdateTile(theTiles, 3, 3);


            //randomize the board
            Random random = new Random();

            int randIndex = 0;

            while (randIndex < 100)
            {
                int ListIndex = random.Next(14);

                if (theTiles[ListIndex].Move != 0)
                {
                    //pointer released is called because it already is able to move a tile based on that tiles attributes
                    PointerReleased(theTiles[ListIndex].TileImage, null);
                    randIndex++;
                }
            }
        }

        //this event it used when the tile is being dragged
        void ManipulateMe_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

            Image tempImage = (Image)sender;

            //get the position of the element on the canvas plus the movement delta
            double left = Canvas.GetLeft(tempImage) + e.Delta.Translation.X;
            double top = Canvas.GetTop(tempImage) + e.Delta.Translation.Y;

            //get the mytile object tied to this image
            myTile temp = myTile.GetFromName(tempImage.Name.ToString(), theTiles);

            //if this tile is able to move (move != 0)
            if (temp.Move == 1)
            {
                //get if the movement in a positive or negative direction 
                if (temp.Direct == true)
                {
                    //get the allowable movement of the tile, then make sure that the new movement is with that range
                    if (left > myTile.LogicToBoard(temp.chrdX, 'w') && left < myTile.LogicToBoard(temp.chrdX + 1, 'w'))
                    {
                        Canvas.SetLeft(tempImage, left);
                    }
                }
                else
                {
                    //get the allowable movement of the tile, then make sure that the new movement is with that range
                    if (left < myTile.LogicToBoard(temp.chrdX, 'w') && left > myTile.LogicToBoard(temp.chrdX - 1, 'w'))
                    {
                        Canvas.SetLeft(tempImage, left);
                    }
                }


            }
            else if (temp.Move == 2)
            {
                //get if the movement in a positive or negative direction 
                if (temp.Direct == true)
                {
                    //get the allowable movement of the tile, then make sure that the new movement is with that range
                    if (top < myTile.LogicToBoard(temp.chrdY, 'h') && top > myTile.LogicToBoard(temp.chrdY - 1, 'h'))
                    {
                        Canvas.SetTop(tempImage, top);
                    }
                }
                else
                {
                    //get the allowable movement of the tile, then make sure that the new movement is with that range
                    if (top > myTile.LogicToBoard(temp.chrdY, 'h') && top < myTile.LogicToBoard(temp.chrdY + 1, 'h'))
                    {
                        Canvas.SetTop(tempImage, top);
                    }
                }
            }
        }

        //This bool is used so that only the ManipulateMe_ManipulationCompleted or PointerReleased
        bool DontClickSnap = false;

        //this event fires when a drag action is complete
        //it will be used to snap the tile into place, then call the methods used to update the game logic
        void ManipulateMe_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //set this true so that the PointerReleased will not activate
            DontClickSnap = true;

            Image tempImage = (Image)sender;

            //get the mytile object tied to this image
            myTile temp = myTile.GetFromName(tempImage.Name.ToString(), theTiles);

            //save its original position
            int oldX = temp.chrdX;
            int oldY = temp.chrdY;

            //get the actual position of the tile on the canvas
            double top = Canvas.GetTop(tempImage);
            double left = Canvas.GetLeft(tempImage);

            double DistanceOriginal = 0;
            double DistanceNew = 0;
            bool blockMoved = false;

            //get the direction that the tile is supposed to move in
            if (temp.Move == 2 && temp.Direct)
            {
                //get the distances of between where the tile was coming from, and where it was going 
                DistanceOriginal = myTile.LogicToBoard(temp.chrdY, 'h') - top;
                DistanceNew = top - myTile.LogicToBoard(temp.chrdY - 1, 'h');

                //determine if it is closer the new spot or the original one.
                if (DistanceOriginal <= DistanceNew)
                {
                    //put it back to were it came from
                    Canvas.SetTop(tempImage, myTile.LogicToBoard(temp.chrdY, 'h'));
                }
                else
                {
                    //snap it to the new position and set that it a block was moved
                    Canvas.SetTop(tempImage, myTile.LogicToBoard(temp.chrdY - 1, 'h'));
                    blockMoved = true;
                }
            }
            else if (temp.Move == 2 && !temp.Direct)
            {
                //get the distances of between where the tile was coming from, and where it was going 
                DistanceNew = myTile.LogicToBoard(temp.chrdY + 1, 'h') - top;
                DistanceOriginal = top - myTile.LogicToBoard(temp.chrdY, 'h');

                if (DistanceOriginal <= DistanceNew)
                {
                    //put it back to were it came from
                    Canvas.SetTop(tempImage, myTile.LogicToBoard(temp.chrdY, 'h'));
                }
                else
                {
                    //snap it to the new position and set that it a block was moved
                    Canvas.SetTop(tempImage, myTile.LogicToBoard(temp.chrdY + 1, 'h'));
                    blockMoved = true;
                }
            }
            else if (temp.Move == 1 && temp.Direct)
            {
                //get the distances of between where the tile was coming from, and where it was going 
                DistanceNew = myTile.LogicToBoard(temp.chrdX + 1, 'w') - left;
                DistanceOriginal = left - myTile.LogicToBoard(temp.chrdX, 'w');

                if (DistanceOriginal <= DistanceNew)
                {
                    //put it back to were it came from
                    Canvas.SetLeft(tempImage, myTile.LogicToBoard(temp.chrdX, 'w'));
                }
                else
                {
                    //snap it to the new position and set that it a block was moved
                    Canvas.SetLeft(tempImage, myTile.LogicToBoard(temp.chrdX + 1, 'w'));
                    blockMoved = true;
                }
            }
            else if (temp.Move == 1 && !temp.Direct)
            {
                //get the distances of between where the tile was coming from, and where it was going 
                DistanceNew = left - myTile.LogicToBoard(temp.chrdX - 1, 'w');
                DistanceOriginal = myTile.LogicToBoard(temp.chrdX, 'w') - left;

                if (DistanceOriginal <= DistanceNew)
                {
                    //put it back to were it came from
                    Canvas.SetLeft(tempImage, myTile.LogicToBoard(temp.chrdX, 'w'));
                }
                else
                {
                    //snap it to the new position and set that it a block was moved
                    Canvas.SetLeft(tempImage, myTile.LogicToBoard(temp.chrdX - 1, 'w'));
                    blockMoved = true;
                }
            }

            //if a block was moved to a new position
            if (blockMoved)
            {
                //update the list of tile 
                theTiles = TileHandler.UpdateTile(theTiles, oldX, oldY);

                //update the win sate
                int correctTiles = TileHandler.CheckWin(theTiles);
                if (correctTiles == 15)
                {
                    StatusBar.Text = "You Win!";
                }
                else
                {
                    StatusBar.Text = "You have " + correctTiles + " in the correct place.";
                }
            }
        }

        //This event will fire when the user releases the tile.
        //It will be used when the user clicks on the block to move it automatically.
        //It will be blocked if the ManipulateMe_ManipulationCompleted happened because the user was dragging the tile.
        private new void PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!DontClickSnap)
            {
                //stop any storyboard actions that are happening
                theStoryBoard.Stop();

                //set the RepositionThemeAnimation to the current 
                Storyboard.SetTarget(RTA, (UIElement)sender);

                //get the tile info
                Image tempBoarder = (Image)sender;
                myTile temp = myTile.GetFromName(tempBoarder.Name.ToString(), theTiles);

                //save the original position of the tile
                int oldX = temp.chrdX;
                int oldY = temp.chrdY;

                bool blockMoved = false;

                //check if the tile is able to move and in what direction
                if (temp.Move == 2 && temp.Direct)
                {
                    //set the RepositionThemeAnimation movement for the tile 
                    RTA.FromVerticalOffset = Constants.BlockHeight;
                    RTA.FromHorizontalOffset = 0;

                    //begin the animation
                    theStoryBoard.Begin();
                    Canvas.SetTop(tempBoarder, myTile.LogicToBoard(temp.chrdY - 1, 'h'));

                    //set that the block moved
                    blockMoved = true;
                }
                else if (temp.Move == 2 && !temp.Direct)
                {
                    //set the RepositionThemeAnimation movement for the tile 
                    RTA.FromVerticalOffset = Constants.BlockHeight * -1;
                    RTA.FromHorizontalOffset = 0;

                    //begin the animation
                    theStoryBoard.Begin();
                    Canvas.SetTop(tempBoarder, myTile.LogicToBoard(temp.chrdY + 1, 'h'));

                    //set that the block moved
                    blockMoved = true;
                }
                else if (temp.Move == 1 && temp.Direct)
                {
                    //set the RepositionThemeAnimation movement for the tile 
                    RTA.FromVerticalOffset = 0;
                    RTA.FromHorizontalOffset = Constants.BlockWidth * -1;

                    //begin the animation
                    theStoryBoard.Begin();
                    Canvas.SetLeft(tempBoarder, myTile.LogicToBoard(temp.chrdX + 1, 'w'));

                    //set that the block moved
                    blockMoved = true;
                }
                else if (temp.Move == 1 && !temp.Direct)
                {
                    //set the RepositionThemeAnimation movement for the tile 
                    RTA.FromVerticalOffset = 0;
                    RTA.FromHorizontalOffset = Constants.BlockWidth;

                    //begin the animation
                    theStoryBoard.Begin();
                    Canvas.SetLeft(tempBoarder, myTile.LogicToBoard(temp.chrdX - 1, 'w'));

                    //set that the block moved
                    blockMoved = true;
                }

                //if the block was actually moved 
                if (blockMoved)
                {
                    //update the logic of the tile
                    theTiles = TileHandler.UpdateTile(theTiles, oldX, oldY);

                    //check how many tiles are in the correct position
                    int correctTiles = TileHandler.CheckWin(theTiles);

                    //update the status bar
                    if (correctTiles == 15)
                    {
                        StatusBar.Text = "You Win";
                    }
                    else
                    {
                        StatusBar.Text = "You have " + correctTiles + " in the correct place";
                    }
                }
            }

            DontClickSnap = false;
        }

        //reset the page
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            GoToLaunchPage();
        }
    }
}
