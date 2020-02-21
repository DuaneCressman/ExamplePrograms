# Split Image
<b>Author:</b> Duane Cressman</br>
<b>Date:</b> February 20, 2020</br>
<b>Language:</b> C#</br></br>
This program can take any image format as an input and sperate it into the 3 different color channels.
It is a console application and uses command line arguments. </br>
<b>See the folder "Example Photos" for a demonstration of what these images look like.</b></br>

The program will produce bitmaps and save them to the same file location as the input file. The user can choose to create the following images:</br>
1. A bitmap with only the red color channel.</br>
2. A bitmap with only the green color channel.</br>
3. A bitmap with only the blue color channel.</br> 
4. A monochromatic in bitmap.</br></br>

The user can also create an image with a combination of 2 color channels. </br>
For example, a bitmap with only the red and blue colour channels turned on.

All of these options are selected using flags on the command line.

This program is more of a proof of concept. I plan to build an interactive version of this idea in the future. The next version will be built with WPF. The user will load an image into the application and it will be shown in gray scale. There will be red, green, and blue "filters" that the user can drag across the image. The overlapping filters will decide which color channels in the photo are active.
