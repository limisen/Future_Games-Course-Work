using System.Numerics;

// Defining the grid for the game grid[Y,X], where " K " is the key, " G " is the goal, and " - " are empty rooms
string[,] grid = new string[6, 6]
{
    { " - ", "RPS", " - ", " - ", " - ", " - " },
    { " G ", " K ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " }
};
// Defining the variables for the game
Vector2 userPosition = new Vector2(0, 0);

string currentRoom = grid[(int)userPosition.Y, (int)userPosition.X]; // Current room the user is in, based on their position in the grid

int turnsLeft = grid.GetUpperBound(0) * grid.GetUpperBound(1); // turns left before the game ends
bool isPlaying = true;

List<string> Log = new List<string>(); // Log of the user's commands

// Defining and setting where the special rooms are located in the grid
Vector2 keyPosition = new Vector2(0, 0);  // Temporary position until we find the key in the grid
Vector2 goalPosition = new Vector2(0, 0); // Temporary position until we find the goal in the grid
Vector2 RPS_Position = new Vector2(0, 0);  // Temporary position until we find the RPS room in the grid
for (int y = 0; y < grid.GetLength(0); y++)
{
    for (int x = 0; x < grid.GetLength(1); x++)
    {
        if (grid[y, x] == " K ")
        {
            keyPosition = new Vector2(x, y);
        }
        if (grid[y, x] == " G ")
        {
            goalPosition = new Vector2(x, y);
        }
        if (grid[y, x] == "RPS")
        {
            RPS_Position = new Vector2(x, y);
        }
    }
}

// Defining the variables that check if the user has found the key or reached the goal
bool userFoundGoal = false;
bool userHasFoundKey = false;
bool userHasKey = false;
bool userHasFoundRPS = false;

string userInput = "";

// Game starts here:
Console.WriteLine("Welcome to the land of ZORK!\n");
Console.WriteLine("You are currently in a mansion.\nYour goal is to find the 'inner great hall'.\nAnd open it with a key that is hidden ~somewhere~ in the mansion.\n");

Console.WriteLine("You are currently here:");
// Display the map at the start of the game, so the user can see where they are in the mansion
displayMap(userPosition);
Console.WriteLine("(X is your position)");

// Display the help menu at the start of the game, so the user can see what commands are available to them
commandSelector("help");
Log.RemoveAt(Log.Count - 1);

while (isPlaying && turnsLeft > 0)
{
    // Check what room the user is in and display a message accordingly
    if (currentRoom == " - ")
    {
        Console.WriteLine("You are in an empty room.");
    }
    if (currentRoom == " K ")
    {
        userHasFoundKey = true;
        if (userHasKey)
        {
            Console.WriteLine("You are in a room where the key *used* to be, (you picked it up)");
        }
        else
        {
            Console.WriteLine("You are in a room with a key.");
        }
    }
    if (currentRoom == " G ")
    {
        userFoundGoal = true;
        Console.WriteLine("You are in the 'inner great hall', standing before the great double door.");
        if (!userFoundGoal) { Console.WriteLine("(This is the room where you can win the game)"); }
    }
    if (currentRoom == "RPS")
    {
        userHasFoundRPS = true;
        Console.WriteLine("You are in a room with a strange table.\nOn it is a rock, a piece of paper and a pair of scissors.");
        Console.WriteLine("The table beckons you to choose one!\n");

        Console.WriteLine("Which do you choose?\n[rock], [paper], or [scissors]:");
        RPS(userInput = Console.ReadLine().Trim().ToLower());
    }

    // check if the user is at the goal position and if they have the key
    if (userPosition == goalPosition && userHasKey == true)
    {
        Console.WriteLine("\nCongratulation! you've reached the goal (the 'inner great hall') and aquired the key!\n\nDo you wish to use the key?\n[Yes] or [No]");
        userInput = Console.ReadLine().Trim().ToLower();
        // Check if the user wants to use the key or not
        if (userInput == "yes")
        {
            Log.Add(userInput); // Add the inputed command to the log
            Console.WriteLine("\nYou use the key to unlock the doors.\nAs you (obviously) open it, you see that beyond it lays...\n\n(Well we'll see. You'll have to stay tuned untill the next installment of ZORK\n\nYOU WIN!!!");
            isPlaying = false;
            break;
        }
        else if (userInput == "no")
        {
            Log.Add(userInput); // Add the inputed command to the log
            Console.WriteLine("\nstrange... There's not much else you can do besides finding this, the key and picking it up. But alright...");
            Console.WriteLine("\nPress 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
        }

        // Error handling for invalid input, keep asking until the user provides a valid response
        while (userInput != "yes" && userInput != "no")
        {
            Console.WriteLine("Invalid input. Please choose [Yes] or [No]");
            userInput = Console.ReadLine().Trim().ToLower();

            // Check if the user wants to use the key
            if (userInput == "yes")
            {
                Log.Add(userInput); // Add the inputed command to the log

                Console.WriteLine("\nYou use the key to unlock the heavy door and beyond it lays...\n(Well we'll see you'll have to stay tuned untill the next installment of ZORK\nYOU WIN!!!");
                isPlaying = false;
                break;
            }
            else if (userInput == "no")
            {
                Log.Add(userInput); // Add the inputed command to the log
                Console.WriteLine("\nstrange... There's not much else you can do besides this but alright...");
            }
        }
    }

    // IF they don't have the key then tell them to keep looking for it
    else if (userPosition == goalPosition && userHasKey == false)
    {
        Console.WriteLine("\nYou found the goal (the 'inner great hall'), but you don't have the key to open the great door.");
        if (!userHasFoundKey) { Console.WriteLine("Keep looking for the key!"); }
        Console.WriteLine("Press 'Enter' to continue");
        Console.ReadLine();
        Console.Clear();
    }

    // Check if the user is at the key position. Then prompt them to pick it up or not, and handle their response
    if (userPosition == keyPosition && userHasKey == false)
    {
        Console.WriteLine("\nWould you like to pick it up?\n\n[Yes] or [No]");
        userInput = Console.ReadLine().Trim().ToLower();

        // Prompt the user to pick up the key or not, and handle their response
        if (userInput == "yes")
        {
            Log.Add(userInput); // Add the inputed command to the log

            Console.WriteLine("\nYou pick up the key!");
            keyPosition = new Vector2(-1, -1); // just to make it dissappear
            userHasKey = true;
            userHasFoundKey = true;
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
        }
        else if (userInput == "no")
        {
            Log.Add(userInput); // Add the inputed command to the log

            userHasFoundKey = true;
            Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...\n");
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
        }

        // error handling for invalid input, keep asking until the user provides a valid response
        while (userInput != "yes" && userInput != "no")
        {
            Console.WriteLine("Invalid input. Please choose [Yes] or [No]");
            userInput = Console.ReadLine().Trim().ToLower();

            // Check (again) if the user wants to pick up the key
            if (userInput == "yes")
            {
                Log.Add(userInput); // Add the inputed command to the log

                Console.Clear();
                Console.WriteLine("You pick up the key!");
                keyPosition = new Vector2(-1, -1); // just to make the key none interactable
                userHasFoundKey = true;
                userHasKey = true;
                Console.WriteLine("Press 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
            }
            // or not
            else if (userInput == "no")
            {
                Log.Add(userInput); // Add the inputed command to the log

                userHasFoundKey = true;
                Console.Clear();
                Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...\n");
                Console.WriteLine("Press 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    // IF the user has "won" already, then don't proceed with (this) the loop and end the game
    if (isPlaying)
    {
        Console.WriteLine($"It's time to move, you've only got {turnsLeft} turns left until the mansion collapses!\n\nWhere are you headed?");
        userInput = Console.ReadLine().Trim().ToLower();

        if (turnsLeft == 0)
        {
            Console.WriteLine("You've run out of turns! Game Over.\nYOU LOSE!");
        }

        commandSelector(userInput);
    }
}
// Game loop ends here

// Function to display the map. Showing where the user is positioned and special rooms (like the goal, key, and RPS room) IF the user has found them
void displayMap(Vector2 userPosition)
{
    int gridMaxX = grid.GetUpperBound(0);
    int gridMaxY = grid.GetUpperBound(1);

    Vector2 currentPos = new Vector2(0, 0);

    // top border (walls) of the grid
    Console.Write("|");
    for (int i = 0; i < gridMaxX + 1; i++)
    {
        Console.Write("---");
    }
    Console.WriteLine("|");

    Console.Write("|");

    // map content
    foreach (var item in grid)
    {
        if (currentPos == userPosition) // user's position is marked with an "X" on the grid
        {
            Console.Write(" X ");
        }
        else
        {
            if (item == " G " && userFoundGoal == false)    // Goal position is marked with a "G" on the grid, but only shown IF the user has found it
            {
                Console.Write(" - ");
            }
            if (item == " K " && userHasFoundKey == false)  // Key position is marked with a "K" on the grid, but only shown IF the user has found it
            {
                Console.Write(" - ");
            }

            if (item == "RPS" && userHasFoundRPS == false) // The position/room where the Rock Papper Scissors minigame is located is marked with "RPS"
            {
                Console.Write(" - ");
            }

            // Otherwise show the item in the grid. 
            if ((item == " G " && userFoundGoal == false) == false && ((item == " K " && userHasFoundKey == false) == false) && ((item == "RPS" && userHasFoundRPS == false) == false))
            {
                Console.Write(item);
            }

        }
        if (currentPos.X == gridMaxX)   // If the current position is at the border of the grid, print a border, and move to the next line
        {
            Console.WriteLine("|");
        }
        if (currentPos.X + 1 > gridMaxX) // If the current position is greater/more than the max X value of the grid (i.e., we've reached the right border(/wall)), then reset the X position to 0 and move to the next line
        {
            currentPos.X = 0;
            currentPos.Y++;
            Console.Write("|");
        }
        else // Otherwise, just move to the next position in the grid
        {
            currentPos.X++;
        }
    }

    // bottom border (walls) of the grid
    for (int i = 0; i < gridMaxX + 1; i++)
    {
        Console.Write("---");
    }
    Console.WriteLine("|");
}

// Function to handle user input for movement and other commands (excluding RPS minigame)
void commandSelector(string userInput)
{
    // Handle user input for movement and other commands
    switch (userInput)
    {
        case "left":
            if ((userPosition.X - 1) >= 0 && userInput == "left")
            {
                userPosition.X += -1;
                turnsLeft--;
                Log.Add("left"); // Add the new command to the log
            }
            else
            {
                Console.WriteLine("\nPLEASE, try to stay inside the mansion and don't try to go through it's (outer) walls.");
                if (userPosition.X <= 0) { userPosition.X = 0; }
                Console.WriteLine("Press 'Enter' to acknowledge");
                Console.ReadLine();
            }
            break;

        case "right":
            if ((userPosition.X + 1) <= (grid.GetLength(0) - 1) && userInput == "right")
            {
                userPosition.X += 1;
                turnsLeft--;
                Log.Add("right"); // Add the inputed command to the log
            }
            else
            {
                Console.WriteLine("\nPLEASE, try to stay inside the mansion and don't try to go through it's (outer) walls.");
                if (userPosition.X >= grid.GetLength(0) - 1) { userPosition.X = grid.GetLength(0) - 1; }
                Console.WriteLine("Press 'Enter' to acknowledge");
                Console.ReadLine();
            }
            break;

        case "up":
            if ((userPosition.Y - 1) >= 0 && userInput == "up")
            {
                userPosition.Y -= 1;
                turnsLeft--;
                Log.Add("up"); // Add the inputed command to the log
            }
            else
            {
                Console.WriteLine("\nPLEASE, try to stay inside the mansion and don't try to go through it's (outer) walls.");
                if (userPosition.Y >= grid.GetLength(1) - 1) { userPosition.Y = grid.GetLength(1) - 1; }
                Console.WriteLine("Press 'Enter' to acknowledge");
                Console.ReadLine();
            }
            break;

        case "down":
            if ((userPosition.Y + 1) <= grid.GetLength(1) - 1 && userInput == "down")
            {
                userPosition.Y += 1;
                turnsLeft--;
                Log.Add("down"); // Add the inputed command to the log
            }
            else
            {
                Console.WriteLine("\nPLEASE, try to stay inside the mansion and don't try to go through it's (outer) walls.");
                if (userPosition.Y <= 0) { userPosition.Y = 0; }
                Console.WriteLine("Press 'Enter' to acknowledge");
                Console.ReadLine();
            }
            break;

        case "hint":
            Log.Add("hint"); // Add the inputed command to the log

            // IF the user has NOT found the goal point them in the right direction.
            if (!userFoundGoal)
            {
                Console.Write($"The goal is somewhere... ");
                if (userPosition.X < goalPosition.X)
                {
                    Console.WriteLine("to the right...");
                }
                else if (userPosition.X > goalPosition.X)
                {
                    Console.WriteLine("to the left...");
                }
                else if (userPosition.Y < goalPosition.Y)
                {
                    Console.WriteLine("below you...");
                }
                else if (userPosition.Y > goalPosition.Y)
                {
                    Console.WriteLine("above you...");
                }
                Console.WriteLine("\nPress 'Enter' to continue");
                Console.ReadLine();
                break;
            }
            // IF they've found the goal and don't have the key, tell the user to look at their map to locate the goal again. Also hint that something else may be needed
            else if (userFoundGoal && !userHasKey)
            {
                Console.WriteLine("You've already found the goal. Look at the map using [showmap] to see where it is");
                Console.WriteLine("(Did you need something else?...)");
                Console.WriteLine("\nPress 'Enter' to continue");
                Console.ReadLine();
                break;
            }
            // IF the user has found the goal and picked up the key, tell them to go there and use the key
            else if (userFoundGoal && userHasKey)
            {
                Console.WriteLine("Head to the 'inner great hall' and use the key!");
                Console.WriteLine("\nPress 'Enter' to continue");
                Console.ReadLine();
                break;
            }
            break;

        case "help":
            Log.Add("help"); // Add the inputed command to the log

            // Display the help menu to the user
            Console.WriteLine("\nPossible paths: 'Left', 'Right', 'Up' & 'Down'");
            Console.WriteLine("(Attempts to move in the specified direction)\t\t\t(Moving into a room consumes a turn)");
            Console.WriteLine("(not writing anything ('') will keep you in the same room)\t(Remaining in the same room does NOT consume a turn)\n");

            Console.WriteLine("Other commands: 'Hint', 'Show Map', 'Log', 'Redo' & 'Quit'\t(These commands dont consume a turn)\n");

            Console.WriteLine("'Hint': points you in the direction of the goal.\t\t(Prioritizing X-axis over Y)");
            Console.WriteLine("'Show Map': Displays the map of the mansion.\t\t\t(Also shows special rooms on the map, IF you've found them)");
            Console.WriteLine("'Log': Displays a log of all the commands you've inputed so far");
            Console.WriteLine("'Redo': Takes you back to the state prior to the last command");
            Console.WriteLine("'Quit': Quits the program");

            // Effectively just a pause so the user can read the help menu before continuing
            Console.WriteLine("\nPress 'Enter' to continue");
            Console.ReadLine();
            break;

        case "showmap":
            Log.Add("showmap"); // Add the inputed command to the log

            displayMap(userPosition);
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            break;
        case "show map":
            Log.Add("show map"); // Add the inputed command to the log

            displayMap(userPosition);
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            break;

        case "log":
            Log.Add("log"); // Add the inputed command to the log

            Console.WriteLine("Your log is as follows:");
            int nrOfcmds = 0;
            foreach (string? item in Log)
            {
                Console.Write($"{++nrOfcmds}. [{item}], ");
            }
            Console.WriteLine(""); // Just to add a new (empty) line after the log is printed

            Console.WriteLine("\nPress 'Enter' to continue");
            Console.ReadLine();
            break;

        case "redo":
            Log.Add("redo"); // Add the inputed command to the log

            // Redo the last command in the log, if there is one
            Console.WriteLine("not implimented yet.");

            break;

        case "quit":
            Log.Add("quit"); // Add the inputed command to the log
            isPlaying = false;
            break;

        case "": // IF nothing is typed, (usually when the game starts) then just continue
            break;
        default:
            Console.WriteLine("Invalid input.\nPlease choose a valid command (see help for possible commands).\nPress 'Enter' to acknowledge");
            Console.ReadLine();
            break;
    }
    currentRoom = grid[(int)userPosition.Y, (int)userPosition.X]; // Updating currentRoom to reflect the user's new position in the mansion(/grid) after their move

    // Clear the console for the next turn (and astethic reasons)
    Console.Clear();
}

// Function to handle the Rock Paper Scissors minigame
void RPS(string userInput)
{
pickRPS:
    // Error handling for invalid input, keep asking until the user provides a valid response
    while ((userInput != "rock") && (userInput != "paper") && (userInput != "scissors"))
    {
        Console.WriteLine("Invalid input. Please choose [rock], [paper], or [scissors]:");
        userInput = Console.ReadLine().Trim().ToLower();
    }

    Log.Add($"{userInput}"); // Add the inputed command to the log

    Console.WriteLine($"\nAs you pick up the {userInput} the two other items on the table vanish and a strange voice says:");
    Console.Write($"'Your choice is made~...'\n\nThe voice continues:\n'Now I will choose~...'");

    int opponentChoiceNumber = new Random().Next(0, 3); // randomly choose a number between 0 and 2 for the opponent's choice
    string opponentChoiceTxt = "";

    // Display the opponent's choice
    switch (opponentChoiceNumber)
    {
        case 0:
            Console.WriteLine("Rock!\n");
            opponentChoiceTxt = "Rock";
            break;
        case 1:
            Console.WriteLine("Paper!\n");
            opponentChoiceTxt = "Paper";
            break;
        case 2:
            Console.WriteLine("Scissors!\n");
            opponentChoiceTxt = "Scissors";
            break;
    }

    // Prepare a variable to hold the user's choice as a number
    int userRPSchoice = 0; // This is just a temporary value and will be set based on the user's input below
    switch (userInput) // Convert the user's choice to a number for comparison
    {
        case "rock":
            userRPSchoice = 0;
            break;
        case "paper":
            userRPSchoice = 1;
            break;
        case "scissors":
            userRPSchoice = 2;
            break;
    }

    // Now to determine the winner of the Rock Paper Scissors game,
    // by the following truth table:
    /*
     | ----------- | -------- | ---------- | ----------- |
     | user →      | rock (0) | papper (1) | scissor (2) |
     | Opp ↓       |          |            |             |
     | ----------- | -------- | ---------- | ----------- |
     | rock (0)    |    0     |     1      |     2       |
     | ----------- | -------- | ---------- | ----------- |
     | papper (1)  |    2     |     0      |     1       |
     | ----------- | -------- | ---------- | ----------- |
     | scissor (2) |    1     |     2      |     0       |
     | ----------- | -------- | ---------- | ----------- |
    */
    int result = (userRPSchoice - opponentChoiceNumber + 3) % 3;
    // switch statement to handle the result of the RPS game
    switch (result)
    {
        // It's a tie
        case 0:
            Console.WriteLine("After a pause. The voice continues:\n'You have chosen the same as I, so we are equal~'");
            Console.WriteLine("\nPress 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("The items on the table reappear and you are beckoned to choose again!\n");

            Console.WriteLine("Which do you choose?\n[rock], [paper], or [scissors]:");
            userInput = Console.ReadLine().Trim().ToLower();

            goto pickRPS; // Go the the label and effectively restart the RPS game, until a winner is determined (Best of 1)

        // The user wins
        case 1:
            Console.WriteLine($"After a pause. The voice continues:\n'You have chosen better than I. {userInput} beats {opponentChoiceTxt} therefore you win!'");
            if (userFoundGoal == false)
            {
                Console.WriteLine("'\nAs a reward I'll reveal where the goal of your exploration is located!'");
                userFoundGoal = true;
                displayMap(userPosition);
                Console.WriteLine("\nPress 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("'\nIt seems you've already found the goal....\nSo! I'll simply give you more time to explore as you like!'");
                Console.WriteLine("The mansion stabilizes slightly...");
                turnsLeft += 5;
                Console.WriteLine($"(You now have {turnsLeft} turns left)");
                Console.WriteLine("\nPress 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
            }
            break;

        // The opponent wins
        case 2:
            Console.WriteLine($"After a pause. The voice continues:\n'You have chosen worse than I. {opponentChoiceTxt} beats {userInput} therefore you lose.'");
            Console.WriteLine("'As a punishment I'll teleport you somewhere else in the mansion!'");
            while (userPosition != RPS_Position)
            {
                userPosition = new Vector2(new Random().Next(0, grid.GetLength(0)), new Random().Next(0, grid.GetLength(1)));
            }
            displayMap(userPosition);
            Console.WriteLine("\nPress 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
            break;
    }
}