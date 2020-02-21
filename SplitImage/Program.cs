/* HEADER FILE HEADER COMMENT ================================================================================

  PROJECT:		Split Image
  PROGRAMMER:	Duane Cressman
  DATE:			Feb 20, 2020
  DESCRIPTION:	The program will take an image as an input. It will the produce bitmaps with only specific color channels.
                The path to the image should be the first command line argument.
                The which images to be produced is specified with a flag in the second argument.

                To produce an image with a single color channel, use lowercase letters in the flag.
                Multiple flags can be used together.
                -r : red color channel
                -b : blue color channel
                -g : green color channel
                -m : monochromatic
                If know flag is entered, all images will be produced.

                To produce an image that has multiple color channels, use CAPITAL letters in the flag.
                -R : the red channel will be included in the combined image
                -G : the green channel will be included in the combined image
                -B : the blue channel will be included in the combined image

                For example, the flag -rmGB would produce the following images:
                1. An image with only the red channel.
                2. An monochromatic image.
                3. An image with the green and blue channels combined.

=========================================================================================================== */

using System;
using System.Drawing;

namespace BitMapV2
{
    class Program
    {
        static int Main(string[] args)
        {
            //check for the help flag
            if (args[0].StartsWith("-h"))
            {
                PrintHelpStatment();
                return 0;
            }

            //check the correct amount of command line arguments is correct
            if (!(args.Length == 1 || args.Length == 2))
            {
                Console.WriteLine("Invalid command line arguments. Use flag -h for usage information.");
                return 1;
            }

            //used to control which images should be produced
            bool createRed = true;
            bool createBlue = true;
            bool createGreen = true;
            bool createMono = true;

            //used to control if and how the combined image is created
            bool CombineMode = false;
            bool combineRed = false;
            bool combineBlue = false;
            bool combineGreen = false;

            //if there is a second argument...
            if (args.Length == 2)
            {
                //and it is a flag, parse out the data
                if (args[1].StartsWith("-"))
                {
                    //check for the single color images that need to be produced
                    if (!args[1].Contains("r"))
                    {
                        createRed = false;
                    }

                    if (!args[1].Contains("g"))
                    {
                        createGreen = false;
                    }

                    if (!args[1].Contains("b"))
                    {
                        createBlue = false;
                    }

                    if (!args[1].Contains("m"))
                    {
                        createMono = false;
                    }

                    //check how the combined image should be created
                    if (args[1].Contains("R"))
                    {
                        combineRed = true;
                        CombineMode = true;
                    }

                    if (args[1].Contains("G"))
                    {
                        combineGreen = true;
                        CombineMode = true;
                    }

                    if (args[1].Contains("B"))
                    {
                        combineBlue = true;
                        CombineMode = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command line arguments. Use flag -h for usage information.");
                    return 1;
                }
            }

            //the original image that is being separated
            Bitmap originalImage;

            //see if the image is able to be opened
            try
            {
                originalImage = new Bitmap(args[0], true);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("The file could not be opened. Use flag -h for usage information.");
                return 1;
            }

            //get the height and width of the image
            int height = originalImage.Height;
            int width = originalImage.Width;

            //the specific color channel images 
            Bitmap BMR = new Bitmap(originalImage);
            Bitmap BMG = new Bitmap(originalImage);
            Bitmap BMB = new Bitmap(originalImage);
            Bitmap BMW = new Bitmap(originalImage);

            Console.WriteLine("Deconstructing the image...");

            //for the height and width of the image
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    //for each pixel in the original image

                    //get the pixel color
                    Color color = originalImage.GetPixel(w, h);

                    //if this color channel is needed, save it to the temp image
                    if (createRed)
                    {
                        Color colorR = Color.FromArgb(color.R, 0, 0);
                        BMR.SetPixel(w, h, colorR);
                    }

                    //if this color channel is needed, save it to the temp image
                    if (createBlue)
                    {
                        Color colorB = Color.FromArgb(0, 0, color.B);
                        BMB.SetPixel(w, h, colorB);
                    }

                    //if this color channel is needed, save it to the temp image
                    if (createGreen)
                    {
                        Color colorG = Color.FromArgb(0, color.G, 0);
                        BMG.SetPixel(w, h, colorG);
                    }

                    //if this color channel is needed, save it to the temp image
                    if (createMono)
                    {
                        //get the average of the 3 colors
                        int avrage = (color.R + color.B + color.G) / 3;

                        //set the all three channels to the average of all three
                        Color colorM = Color.FromArgb(avrage, avrage, avrage);
                        BMW.SetPixel(w, h, colorM);
                    }
                }
            }

            //get the path of the image without the file extension
            string[] filePath = args[0].Split('.');
            string fpNoExtension = filePath[0];

            //the total images that have been saved
            int imagesSaved = 0;

            //if this single color image is needed... 
            if (createRed)
            {
                //create a new file extension
                string redFP = fpNoExtension + "(r).bmp";

                //save the image
                BMR.Save(redFP);

                //increment the images saved
                imagesSaved++;
            }

            //if this single color image is needed... 
            if (createBlue)
            {
                //create a new file extension
                string blueFP = fpNoExtension + "(b).bmp";

                //save the image
                BMB.Save(blueFP);

                //increment the images saved
                imagesSaved++;
            }

            //if this single color image is needed... 
            if (createGreen)
            {
                //create a new file extension
                string greenFP = fpNoExtension + "(g).bmp";

                //save the image
                BMG.Save(greenFP);

                //increment the images saved
                imagesSaved++;
            }

            //if this single color image is needed... 
            if (createMono)
            {
                //create a new file extension
                string monoFP = fpNoExtension + "(m).bmp";

                //save the image
                BMW.Save(monoFP);

                //increment the images saved
                imagesSaved++;
            }

            //if a combined image is needed
            if (CombineMode)
            {
                Bitmap combineBM = new Bitmap(originalImage);

                //for each pixel in the image
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        //get the color of the pixel from the original image
                        Color color = originalImage.GetPixel(w, h);

                        //the color values for the new pixel
                        int rVal = 0;
                        int gVal = 0;
                        int bVal = 0;

                        //take the color value if this channel is needed
                        if (combineRed)
                        {
                            rVal = color.R;
                        }

                        //take the color value if this channel is needed
                        if (combineGreen)
                        {
                            gVal = color.G;
                        }

                        //take the color value if this channel is needed
                        if (combineBlue)
                        {
                            bVal = color.B;
                        }

                        //set the pixel color value in the new image 
                        combineBM.SetPixel(w, h, Color.FromArgb(rVal, gVal, bVal));
                    }
                }

                //create the new file extension
                string combineFP = fpNoExtension + "(";

                //add letters to the extension for the included color channels
                if (combineRed)
                {
                    combineFP += "R";
                }

                if (combineBlue)
                {
                    combineFP += "B";
                }

                if (combineGreen)
                {
                    combineFP += "G";
                }

                combineFP += ").bmp";

                //save the image
                combineBM.Save(combineFP);
                imagesSaved++;
            }

            //print out the number of saved images
            if (imagesSaved == 1)
            {
                Console.WriteLine("1 image was created.");
            }
            else
            {
                Console.WriteLine(imagesSaved.ToString() + " images were created.");
            }

            return 0;
        }

        //This method will print out the usage statement
        private static void PrintHelpStatment()
        {
            string helpStatment = "\nThis program will take an image and split it into its different color channels." +
                                  "\nUsage: \nSplitImage.exe \"Image File Path\" [-flags]\n" +
                                  "\nTo produce an image with a single color channel, use lowercase letters in the flag." +
                                  "\nMultiple flags can be used together." +
                                  "\n-r : red color channel\n-b : blue color channel\n-g : green color channel\n-m : monochromatic" +
                                  "\nIf know flag is entered, all images will be produced." +
                                  "\n\nTo produce an image that has multiple color channels, use CAPITAL letters in the flag." +
                                  "\n-R : the red channel will be included in the combined image" +
                                  "\n-G : the green channel will be included in the combined image" +
                                  "\n-B : the blue channel will be included in the combined image" +
                                  "\n\nFor example, the flag -rmGB would produce the following images:" +
                                  "\n1. An image with only the red channel.\n2. An monochromatic image.\n3. An image with the green and blue channels combined.\n";

            Console.WriteLine(helpStatment);
        }
    }
}
