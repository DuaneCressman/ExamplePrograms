/* HEADER FILE HEADER COMMENT ================================================================================

  PROJECT:		CreateGradient
  PROGRAMMER:	Duane Cressman
  DATE:			March 2, 2020
  DESCRIPTION:	This file holds the main function.

  This program will create and save bitmap images. The images can vary in size and will show a rainbow gradient
  with all the possible colors.
=========================================================================================================== */

using System;
using System.Drawing;

namespace CreateGradient
{
    class Program
    {
        //size of the image to be generated
        public static int SIZE = 500;

        static void Main(string[] args)
        {
            //the output bitmap to be created
            Bitmap Output = new Bitmap(SIZE, SIZE);

            //center of the image
            int centerX = SIZE / 2;
            int centerY = SIZE / 2;

            //the longest distance in the image
            double maxDistance = Gradient.GetDistance(0, 0, centerX, centerY);

            //define which type of image should be generated
            int imageMode = 2;
            switch (imageMode)
            {
                //generate a gradient based in a circle
                case 0:

                    //move the center to the bottom of the image
                    centerX = SIZE / 2;
                    centerY = SIZE - SIZE / 20;

                    maxDistance = Gradient.GetDistance(0, 0, centerX, centerY);

                    //for each bit in the image
                    for (int w = 0; w < Output.Width; w++)
                    {
                        for (int h = 0; h < Output.Height; h++)
                        {
                            //get the distance between the pixel and the center
                            double distance = Gradient.GetDistance(w, h, centerX, centerY);

                            //get the corresponding colour
                            Color ColColor = Gradient.GetGradient(distance, maxDistance);

                            //set the pixel
                            Output.SetPixel(w, h, ColColor);
                        }
                    }

                    //save the image
                    Output.Save("HalfMoon.bmp");

                    break;


                    //produce an image with the gradient that follows the edges of the image.
                case 1:

                    //middle of the image
                    int mid = SIZE / 2;

                    //for each pixel
                    for (int w = 0; w < Output.Width; w++)
                    {
                        for (int h = 0; h < Output.Height; h++)
                        {
                            //get the values needed to calculate the max value
                            double x = Math.Abs(w - mid);
                            double y = Math.Abs(h - mid);

                            double MaxValue = mid;

                            //find the max distance for this pixel
                            if (x > y)
                            {
                                if (x != 0)
                                {
                                    //using the slope formula, get the distance
                                    MaxValue = Math.Sqrt(Math.Pow(mid, 2) + Math.Pow((y / x) * mid, 2));
                                }
                            }
                            else if (x < y)
                            {
                                if (y != 0)
                                {
                                    //using the slope formula, get the distance
                                    MaxValue = Math.Sqrt(Math.Pow(mid, 2) + Math.Pow((x / y) * mid, 2));
                                }
                            }
                            else
                            {
                                //get default for a 45 degree angle
                                MaxValue = Math.Sqrt(Math.Pow(mid, 2) + Math.Pow(mid, 2));
                            }

                            //get the distance between the pixel and the center
                            double pixelDis = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

                            //get the corresponding colour
                            Color ColColor = Gradient.GetGradient(pixelDis, MaxValue);

                            //set the pixel
                            Output.SetPixel(w, h, ColColor);
                        }
                    }

                    Output.Save("Square.bmp");

                    break;

                //produce an gradient with 2 centers.
                case 2:

                    //get the 2 centers 
                    int c1x = SIZE / 5;
                    int c1y = SIZE / 2;
                    int c2x = 4 * SIZE / 5;
                    int c2y = SIZE / 2;

                    //for each pixel
                    for (int w = 0; w < Output.Width; w++)
                    {
                        for (int h = 0; h < Output.Height; h++)
                        {
                            //get the distance from the pixel to each center
                            double distance1 = Gradient.GetDistance(w, h, c1x, c1y);
                            double distance2 = Gradient.GetDistance(w, h, c2x, c2y);

                            //using the average of the 2 distances, get the corresponding colour.
                            Color ColColor = Gradient.GetGradient((distance1 + distance2) / 2, maxDistance + 50);

                            //set the pixel
                            Output.SetPixel(w, h, ColColor);
                        }
                    }

                    //save the image
                    Output.Save("DoubleCenter.bmp");

                    break;

                default:
                    break;
            }
        }
    }
}
