//Project: MorseCodeTree
//Programmer: Duane Cressman (Student ID: 8431702)
//First Version: March 13, 2019
//Description: The Morse code for each letter / digit is stored in code.dat. 
//			   The program is written in C++ and uses the PD Curses library to control the output to the screen.
//			   The code for each letter / digit can be read by beginning at “Start” and following the tree down to that letter / digit.
//			   Each left turn represents a ‘.’ and each right turn represents a ‘ - ‘.


#pragma warning(disable: 4996)

#define FILENAME "code.dat"

#include <stdio.h>
#include <string.h>
#include <curses.h>
#include <windows.h>
#include <list>
using namespace std;


#define DOT 0
#define DASH 1
#define END 2
#define PAUSERATE 10
#define DEFAULTHIGHT 1

//The List STL will use this struct to hold the data for each character
struct dataElement
{
	char letter;
	char dotDash[7];
	dataElement* pDot;
	dataElement* pDash;
};

//the class that draws the path
class DrawPath
{
private:
	const int levLen[5] = { 36, 18, 8, 4, 2 }; //The length of the lines for each level of the tree.
	char path[7]; //the dots and dashes
	int level; //0 - 4 the level on the tree
	int xPos; //X position of the cursor
	int yPos; //y position of the cursor
	int numOfIterations; //how far into the path the code is
	char letter; //the letter to be printed.
	WINDOW* screen; //a pointer to the screen curses will be using, used for some curses functions
	int nextIteration; //an instruction for if a dot or dash line should be printed out next.
	bool pause; //if the object should pause when doing some refresh, used to watch the lines being drawn. 

	void GoDot(void); //print a line in the "Dot" direction
	void GoDash(void); //print a line in the "Dash" direction
	void PrintChar(void); //print the letter in the tree.

	void SlowRefresh(void); //a version of the refresh command that pauses after the refresh 


public:
	DrawPath(void); //constructor - default, should not be used.
	DrawPath(WINDOW* inScreen); //constructor - sets the window data member in the object
	~DrawPath(); //Destructor
	void Print(dataElement* inData); //prints out the path that a mores code letter would make on the tree
	void SetPause(bool shouldPause = false); //set if the object should pause after some refresh

};




FILE* validateFile(void);
list<dataElement> createList(FILE* inputFile);
FILE* closeFile(FILE* input);
//void printBluePrint(void);


int main(void)
{

	//check that the file can be 
	FILE* inputFile = validateFile();
	if (inputFile == NULL)
	{
		printf("File could not be opened\n");
		getc(stdin);
		return 0;
	}

	//create the list
	std::list<dataElement> dataList = createList(inputFile);

	//set an iterator for the list
	std::list<dataElement> ::iterator position;
	position = dataList.begin();

	//set pointer to hold the data.
	dataElement* current = &(dataElement)*position;

	//start the curses window
	WINDOW* theScreen = initscr();
	start_color();
	//make the cursor not appear.
	curs_set(0);

	//resize the window? (not sure if working)
	resize_term(25, 250);
	resize_window(theScreen, 25, 250);

	//define colors
	init_pair(1, COLOR_GREEN, COLOR_BLACK);
	init_pair(2, COLOR_RED, COLOR_BLACK);
	init_pair(3, COLOR_WHITE, COLOR_BLACK);


	//print out the labels for the tree.
	attron(COLOR_PAIR(3));
	mvprintw(DEFAULTHIGHT + 1, 70, "Start");
	mvprintw(DEFAULTHIGHT, 34, "<<Dot");
	mvprintw(DEFAULTHIGHT, 106, "Dash>>");
	attroff(COLOR_PAIR(3));

	refresh();

	//instantiate the object.
	DrawPath ThePrinter(theScreen);

	//run through each character
	for (int i = 0; i < 36; i++)
	{
		//Have the object print slowly
		ThePrinter.SetPause(true);
		//change colour to red for the first print
		attron(COLOR_PAIR(2));
		//get the data from the list. (letter and path)
		current = &(dataElement)*position;
		//Call the path method
		ThePrinter.Print(current);
		//pause after the path has been printed
		Sleep(PAUSERATE * 20);
		//change colour to green for the second pass of the path.
		attroff(COLOR_PAIR(2));
		attron(COLOR_PAIR(1));
		//Have the object print fast.
		ThePrinter.SetPause(false);
		//Call the path method
		ThePrinter.Print(current);
		//Set colour back
		attroff(COLOR_PAIR(1));
		//Step through the list of the characters
		position++;
	}

	//print the exit statement
	attron(COLOR_PAIR(3));
	mvprintw(21, 61, "Press any key to exit...");
	attroff(COLOR_PAIR(3));

	//wait for a key press to close the window
	getch();
	endwin();

	closeFile(inputFile);

	return 0;
}

/*Function: validateFile
Parameters:
void
Return Value:
FILE*: A pointer to the file that has been opened.
Description: This function will check and open the file that has been hard coded into the program. The file pointer will be returned.
*/
FILE* validateFile(void)
{
	FILE* inputFile = NULL;

	//open the file. Check to see if it opened correctly.
	inputFile = fopen(FILENAME, "r");
	if (inputFile == NULL)
	{
		printf("Error opening %s\n", FILENAME);
		return NULL;
	}

	return inputFile;
}

/*Function: createList
Parameters:
FILE* inputFile: the file to be read.
Return Value:
list<dataElement> : the list that was created.
Description: This function will read a file line by line and create an STL list based on the input.
*/
list<dataElement> createList(FILE* inputFile)
{
	//create the list
	std::list<dataElement> tempList;
	dataElement tempStruct = {};

	//for all 36 characters
	for (int i = 0; i < 36; i++)
	{
		char buffer[15] = { '\0' };
		fgets(buffer, sizeof(buffer), inputFile);
		sscanf(buffer, "%c%*c%s", &tempStruct.letter, tempStruct.dotDash);
		strcat(tempStruct.dotDash, "\n");


		tempList.push_back(tempStruct);
	}
	return tempList;
}


/*Function: closeFile
Parameters:
FILE* fArgv: The file to be closed.
Return Value:
FILE*: If the file fails to close, the original file will be returned, else null will be returned.
Description: This function will close a file and make sure it closes correctly.
*/
FILE* closeFile(FILE* input)
{
	//check if there is a file.
	if (input != NULL)
	{
		if (fclose(input) != 0)
		{
			//if the file doesn't close correctly, print an error.
			printf("Error closing the file\n");
			return input;
		}
		else
		{
			return NULL;
		}
	}
	return NULL;
}




/*Function: DrawPath - constructor
Parameters:
WINDOW* inScreen: the window that the object will be using.
Return Value:
N/A
Description: This method will create the object and set the default values. The window pointer will also be set.
*/
DrawPath::DrawPath(WINDOW* inScreen)
{
	for (int i = 0; i < 7; i++)
	{
		path[i] = 0;
	}

	level = 0;
	yPos = DEFAULTHIGHT + 1;
	xPos = 72;
	numOfIterations = 0;
	nextIteration = 0;
	screen = inScreen;
	pause = false;
}

/*Function: DrawPath - constructor
Parameters:
void
Return Value:
N/A
Description: This method will create the object and set the default values. Because this version does not
				set the window pointer, it should never be used.
*/
DrawPath::DrawPath(void)
{
	for (int i = 0; i < 7; i++)
	{
		path[i] = 0;
	}

	level = 0;
	xPos = 0;
	yPos = 0;
	nextIteration = 0;
	numOfIterations = 0;
	screen = NULL;
	pause = false;
}

/*Function: Print
Parameters:
dataElement* inData: the data that will be used to construct the path of the tree.
Return Value:
void
Description: This method will first save the data from the data struct and save it into the object so it can
			 be used elsewhere in the object. It will the facilitate the calling of the GoDot and GoDash methods
			 that will be printing the tree.
*/
void DrawPath::Print(dataElement* inData)
{
	//copy the path (the dots and dashes) and the character into the object.
	strcpy_s(path, inData->dotDash);
	letter = inData->letter;

	//reset default values
	level = 0;
	yPos = DEFAULTHIGHT + 1;
	xPos = 72;
	numOfIterations = 0;
	move(yPos, xPos);

	//check if the first part of the path is a dot or dash.
	if (path[0] == '.')
	{
		nextIteration = DOT;
	}
	else if (path[0] == '-')
	{
		nextIteration = DASH;
	}

	//this loop will continue until the end of the path is reached. 
	//The "nextIteration" data member will be set after each line is drawn.
	while (nextIteration != END)
	{

		if (nextIteration == DOT)
		{
			GoDot();
		}
		else if (nextIteration == DASH)
		{
			GoDash();
		}
	}

	refresh();
}

/*Function: GoDash
Parameters:
void
Return Value:
void
Description: This method will print the line of the tree in the dash (right) direction. It will determine the length of the line
			 using the "level" and "levLen" data members. If the "pause" data member is true, it will print in a way that lets
			 you see what is happening.
*/
void DrawPath::GoDash(void)
{
	//Print the "|" that will go below the last character
	yPos++;
	mvprintw(yPos, xPos, "|");
	xPos++;
	SlowRefresh();

	//print the line towards the next character
	for (int i = 0; i < levLen[level]; i++)
	{
		mvprintw(yPos, xPos + i, "_");
		SlowRefresh();
	}
	//After printing, get the position of the cursor
	getyx(screen, yPos, xPos);

	//print the "|" that will go above the next character.
	yPos++;
	xPos--;
	mvprintw(yPos, xPos, "|");
	SlowRefresh();

	//call the next method to print the character.
	PrintChar();
}

/*Function: GoDot
Parameters:
void
Return Value:
void
Description: This method will print the line of the tree in the dash (left) direction. It will determine the length of the line
			 using the "level" and "levLen" data members. If the "pause" data member is ture, it will print in a way that lets
			 you see what is happening.
*/
void DrawPath::GoDot(void)
{
	//Print the "|" that will go below the last character
	yPos++;
	mvprintw(yPos, xPos, "|");
	xPos--;
	SlowRefresh();

	//print the line towards the next character
	for (int i = 0; i < levLen[level]; i++)
	{
		mvprintw(yPos, xPos - i, "_");
		SlowRefresh();
	}
	//After printing, get the position of the cursor
	getyx(screen, yPos, xPos);

	//print the "|" that will go above the next character.
	yPos++;
	xPos--;
	mvprintw(yPos, xPos, "|");
	SlowRefresh();

	//call the next method to print the character.
	PrintChar();
}

/*Function: PrintChar
Parameters:
void
Return Value:
void
Description: This method will check if the end of the path has been reached. It will print the character if it is. If not it will
			 determine if the next line is going in the dot or dash direction. In theory the GoDot or GoDash methods could just be called
			 from this function again, but this would mean you have to go all the way back up a long call stack.
*/
void DrawPath::PrintChar(void)
{
	yPos++;

	//check if we are at the end of the path, If so print out the letter.
	if (path[numOfIterations + 1] == '\n')
	{
		attron(COLOR_PAIR(3));
		mvprintw(yPos, xPos, "%c", letter);
		attroff(COLOR_PAIR(3));
	}

	SlowRefresh();

	//set that we have moved down a level in the tree
	numOfIterations++;
	level++;

	//determine what action should happen next. Either we have reached the end so exit the object, or draw another line.
	nextIteration = END;
	if (path[numOfIterations] == '.')
	{
		nextIteration = DOT;
	}
	else if (path[numOfIterations] == '-')
	{
		nextIteration = DASH;
	}
}

/*Function: PrintChar
Parameters:
void
Return Value:
void
Description: This method will be used to replace the "refresh()" function in some areas of the code. It pauses after the screen it refreshed if the
			 "pause" data member is true. The reason for this is so that the user can see each time the screen changes.
*/
void DrawPath::SlowRefresh(void)
{
	refresh();
	if (pause)
	{
		Sleep(PAUSERATE);
	}

}

/*Function: SetPause
Parameters:
bool shouldPause: set the "pause" data member
Return Value:
void
Description: This method will set if the object should pause after a refresh in some parts of the code.
*/
void DrawPath::SetPause(bool shouldPause)
{
	pause = shouldPause;
}

/*Function: ~DrawPath
Parameters:
N/A
Return Value:
N/A
Description: Destructor for the DrawPath class.
*/
DrawPath::~DrawPath()
{

}
