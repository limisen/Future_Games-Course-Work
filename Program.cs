using System.Numerics;

// Defining the variables for the game

// Defining(Generating) the grid and placing the special rooms (key, goal, and RPS) in the grid
List<List<string>> grid = new List<List<string>>();
/* 
 * Elements accessed like (grid[Y][X]).
 * where:
 *    " K ": The room containing the key which is used to unlock the goal, 
 *    " G ": The room containing the goal, 
 *    "RPS": The room containing the Rock Paper Scissors game,
 *    " - ": The empty rooms/space
*/
bool room_K_exists = false;                 // Whether the key room exists in the grid or not
Vector2 keyPosition = new Vector2(0, 0);    // Temporary position until we decide where the key room is in the grid
bool room_G_exists = false;                 // Whether the goal room exists in the grid or not
Vector2 goalPosition = new Vector2(0, 0);   // Temporary position until we decide where the goal room is in the grid
bool room_RPS_exists = false;               // Whether the Rock Paper Scissors room exists in the grid or not
Vector2 RPS_Position = new Vector2(0, 0);   // Temporary position until we decide where the RPS room is in the grid

int Random_size_of_grid = Random.Shared.Next(1, 25); // Generate the size of the grid (i.e., the number of rows and columns) (minimum 1 to make sure all the rooms fit)
// Create the grid with the specified number of rows and columns, filling the grid with empty rooms
// and add all the rooms Coordinates to a HashSet so that we can keep track of which coordinates have already been used
HashSet<Vector2> UnUsedPositions = new HashSet<Vector2>();
for (int y = 0; y <= Random_size_of_grid; y++)
{
    grid.Add(new List<string>());
    for (int x = 0; x <= Random_size_of_grid; x++)
    {
        grid[y].Add(" - ");
        UnUsedPositions.Add(new Vector2(y, x)); // Add the coordinates of the room to the HashSet so that we can use those to get the random positions for the special rooms (key, goal, and RPS) in the grid
    }
}
HashSet<Vector2> usedPositions = new HashSet<Vector2>(); // This is basically just a list of all the positions that have already been used, so that we don't try to use them again when placing the special rooms in the grid

Vector2 RandomPosition = new Vector2(Random.Shared.Next(0, Random_size_of_grid + 1), Random.Shared.Next(0, Random_size_of_grid + 1)); // Get a random position in the grid out of the possible UnUsedPositions, to place the special rooms (key, goal, and RPS) in the grid
usedPositions.Add(RandomPosition);

// Setting where the special rooms are located in the grid by:
// Trying to add the special rooms (key, goal, and RPS) to the grid, until they are all successfully added
while (!room_K_exists || !room_G_exists || !room_RPS_exists) // whilst any of the special rooms (key, goal, and RPS) do not exist in the grid, keep trying to add them to the grid
{
    if (!room_G_exists)
    {
        grid[(int)RandomPosition.Y][(int)RandomPosition.X] = " G ";
        goalPosition = RandomPosition;
        UnUsedPositions.Remove(RandomPosition);
        room_G_exists = true;
    }
    else if (!room_K_exists)
    {
        grid[(int)RandomPosition.Y][(int)RandomPosition.X] = " K ";
        keyPosition = RandomPosition;
        UnUsedPositions.Remove(RandomPosition);
        room_K_exists = true;
    }
    else if (!room_RPS_exists)
    {
        grid[(int)RandomPosition.Y][(int)RandomPosition.X] = "RPS";
        RPS_Position = RandomPosition;
        UnUsedPositions.Remove(RandomPosition);
        room_RPS_exists = true;
    }
    RandomPosition = UnUsedPositions.ElementAt(Random.Shared.Next(UnUsedPositions.Count)); // Get a new random position for the next iteration of the loop, so that we don't just keep checking the same position over and over again
    while (usedPositions.Contains(RandomPosition)) // If the random position is already used, then get a new random position until we find one that is not used yet
    {
        RandomPosition = new Vector2(Random.Shared.Next(0, Random_size_of_grid), Random.Shared.Next(0, Random_size_of_grid));
    }
    usedPositions.Add(RandomPosition); // Add the new random position to the used positions so that we don't try to use it again in the next iteration of the loop
}

Vector2 userPosition = new Vector2(0, 0); // The user's starting position is always (0, 0) in the grid, which is the top left corner of the map/grid/mansion

string currentRoom = grid[(int)userPosition.Y][(int)userPosition.X]; // Current room the user is in, based on their position in the grid

int turnsLeft = grid.Count * grid[0].Count; // turns left before the game ends as determined by the size of the grid (i.e., the number of rooms in the mansion)
bool isPlaying = true;

List<string> Log = new List<string>(); // Log of the user's commands

// Defining the variables that check if the user has found X thing, or has Y item, etc. (to be used in the game loop)
bool userFoundGoal = false;
bool userHasFoundKey = false;
bool userHasKey = false;
bool userHasFoundRPS = false;

string userInput = "";


//------ Game starts here ->
Console.WriteLine("Welcome to the land of ZORK!\n");
mainMenu();

if (isPlaying)
{
    // Welcome the user to the game and explain the goal of the game, how to play it, etc
    Console.WriteLine("You are currently in a mansion.\nYour goal is to find the 'inner great hall'.\nAnd open it with a key that is hidden ~somewhere~ in the mansion.\n");

    Console.WriteLine("You are currently here:");
    // Display the map at the start of the game, so the user can see where they are in the mansion
    displayMap(userPosition);
    Console.WriteLine("(X is your position)\n");
    Console.WriteLine("Press 'Enter' to continue");
    Console.ReadLine();
    Console.Clear();
}


// Main game loop, which continues until the user has 'won', 'lost' or 'quit' the game
while (isPlaying && turnsLeft > 0)
{
    // Check what room the user is in and display a message (handle it) accordingly
    if (currentRoom == " - ") // Empty room
    {
        Console.WriteLine("You are in an empty room.");
    }
    if (currentRoom == " K ") // Key room
    {
        userHasFoundKey = true;

        // Check if the user has already picked up the key or not, and display a message accordingly
        if (userHasKey)
        {
            Console.WriteLine("You are in a room where the key *used* to be, (you picked it up)");
        }
        else
        {
            Console.WriteLine("You are in a room with a key.");
        }
    }
    if (currentRoom == " G ") // Goal room (Where the user can win the game)
    {
        userFoundGoal = true;
        Console.WriteLine("You are in the 'inner great hall', standing before the great double door.");
        if (!userFoundGoal) { Console.WriteLine("(This is the room where you can win the game)"); }
    }
    if (currentRoom == "RPS") // Rock Paper Scissors room (Where the user can play a Rock Paper Scissors minigame)
    {
        userHasFoundRPS = true;
        Console.WriteLine("You are in a room with a strange table.\nOn it is a rock, a piece of paper and a pair of scissors.");
        Console.WriteLine("The table beckons you to choose one!\n");

        Console.WriteLine("Which do you choose?\n[rock], [paper], or [scissors]:");
        userInput = Console.ReadLine().Trim().ToLower();
        Console.Clear();
        RPS(userInput);
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
            Console.Clear();
            Console.WriteLine("You use the key to unlock the doors.\nAs you (obviously) open it, you see that beyond it lays...\n\nWell we'll see. You'll have to stay tuned untill the next installment of ZORK\n\nYOU WIN!!!\n");
            isPlaying = false;
        }
        else if (userInput == "no")
        {
            Log.Add(userInput); // Add the inputed command to the log
            Console.WriteLine("\nstrange... There's not much else you can do besides finding this, the key and picking it up. But alright...");
            Console.WriteLine("\nPress 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
        }

        // Error handling for invalid input. Simply keep asking until the user provides a valid response
        while (userInput != "yes" && userInput != "no")
        {
            Console.Clear();
            Console.WriteLine("Invalid input. Please choose [Yes] or [No]");
            userInput = Console.ReadLine().Trim().ToLower();

            // Check if the user wants to use the key
            if (userInput == "yes")
            {
                Log.Add(userInput); // Add the inputed command to the log
                Console.Clear();
                Console.WriteLine("You use the key to unlock the doors.\nAs you (obviously) open it, you see that beyond it lays...\n\nWell we'll see. You'll have to stay tuned untill the next installment of ZORK\n\nYOU WIN!!!\n");
                isPlaying = false;
            }
            else if (userInput == "no")
            {
                Log.Add(userInput); // Add the inputed command to the log
                Console.WriteLine("\nstrange... There's not much else you can do besides this but alright...");
            }
        }
    }

    // IF the user does not have the key then tell them to keep looking for it
    else if (userPosition == goalPosition && userHasKey == false)
    {
        Console.WriteLine("\nYou found the goal (the 'inner great hall'), but you don't have the key to open the great door.");
        if (!userHasFoundKey) { Console.WriteLine("Keep looking for the key!"); }
        Console.WriteLine("\nPress 'Enter' to continue");
        Console.ReadLine();
        Console.Clear();
    }

    // Simply check if the user is at the key position, and if so, set the userHasFoundKey variable to true
    if (userPosition == keyPosition) { userHasFoundKey = true; }

    // Check if the user is at the key position, and whether they have the key yet. Then prompt them accordingly and handle their response
    if (userPosition == keyPosition && userHasKey == false)
    {
        Console.WriteLine("\nWould you like to pick it up?\n\n[Yes] or [No]");
        userInput = Console.ReadLine().Trim().ToLower();

        // Prompt the user to pick up the key or not, and handle their response
        if (userInput == "yes")
        {
            Log.Add(userInput); // Add the inputed command to the log

            Console.WriteLine("\nYou pick up the key!");
            keyPosition = new Vector2(-1, -1); // just to make it none interactable (as the key is only interactable based on position and should only be interactable once)
            userHasKey = true;

            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
        }
        else if (userInput == "no")
        {
            Log.Add(userInput); // Add the inputed command to the log

            Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...\n");
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
        }

        // Error handling for invalid input, keep asking until the user provides a valid response
        while (userInput != "yes" && userInput != "no")
        {
            Console.Clear();
            Console.WriteLine("Invalid input. Please choose [Yes] or [No]");
            userInput = Console.ReadLine().Trim().ToLower();

            // Check (again) if the user wants to pick up the key...
            if (userInput == "yes")
            {
                Log.Add(userInput); // Add the inputed command to the log

                Console.Clear();
                Console.WriteLine("You pick up the key!");
                keyPosition = new Vector2(-1, -1); // just to make the key none interactable (as the key is only interactable based on position and should only be interactable once)
                userHasKey = true;
                Console.WriteLine("Press 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
            }
            // ..or not
            else if (userInput == "no")
            {
                Log.Add(userInput); // Add the inputed command to the log

                Console.Clear();
                Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...\n");
                Console.WriteLine("Press 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    // IF the user has 'won', 'lost' or 'quit', then don't proceed with (this) the loop
    if (isPlaying)
    {
        Console.WriteLine($"It's time to move, you've only got {turnsLeft} turns left until the mansion collapses!\n\nWhere are you headed?");
        userInput = Console.ReadLine().Trim().ToLower();

        if (turnsLeft == 0)
        {
            Console.WriteLine("You've run out of turns! Game Over.\nYOU LOSE!");
        }

        commandSelector(userInput);

        // IF the user has 'won', 'lost' or 'quit', then proceed with (this) the loop
        if (!isPlaying)
        {
            Console.WriteLine("Would you like to play again?\n[Yes] or [No]");
            userInput = Console.ReadLine().Trim().ToLower();

            // Error handling for invalid input, keep asking until the user provides a valid response
            while (userInput != "yes" && userInput != "no")
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Please choose [Yes] or [No]");
                userInput = Console.ReadLine().Trim().ToLower();

            }

            // Depending on the user's response, either restart the game or exit the program
            if (userInput == "yes")
            {
                Console.Clear();
                Log.Add(userInput); // Add the inputed command to the log
                mainMenu();
            }
            else if (userInput != "no")
            {
                Console.Clear();
                Log.Add(userInput); // Add the inputed command to the log
            }
        }
    }
    // IF the user has 'won', 'lost' or 'quit', then proceed with (this) loop and prompt the user to see if they want to play again. Also and handle their response accordingly
    else
    {
        Console.WriteLine("Would you like to play again?\n[Yes] or [No]");
        userInput = Console.ReadLine().Trim().ToLower();

        // Error handling for invalid input, keep asking until the user provides a valid response
        while (userInput != "yes" && userInput != "no")
        {
            Console.Clear();
            Console.WriteLine("Invalid input. Please choose [Yes] or [No]");
            userInput = Console.ReadLine().Trim().ToLower();
        }

        // Depending on the user's response, either restart the game or exit the program
        if (userInput == "yes")
        {
            Console.Clear();
            Log.Add(userInput); // Add the inputed command to the log
            mainMenu();
        }
        else if (userInput != "no")
        {
            Console.Clear();
            Log.Add(userInput); // Add the inputed command to the log
        }
    }
}
Console.WriteLine("Thanks for playing!");
//<------ Game loop ends here



// Functions and methods:

// Display the main menu to the user, and handle their response
void mainMenu()
{
    bool showMenu = true;

    while (showMenu)
    {
        Console.WriteLine("Main Menu:\n[Start] - Start the game\n[Help] - Show the help menu\n[Quit] - Quit the game\n");
        userInput = Console.ReadLine().Trim().ToLower();

        // Error handling for invalid input, keep asking until the user provides a valid response
        while (userInput != "start" && userInput != "help" && userInput != "quit")
        {
            Console.Clear();
            Console.WriteLine("Invalid input. Please choose [Start], [Help], or [Quit].");

            Console.WriteLine("Explaination:\n[Start] - Start the game\n[Help] - Show the help menu\n[Quit] - Quit the game\n");
            userInput = Console.ReadLine().Trim().ToLower();
        }

        switch (userInput)
        {
            // Exit the main menu and start the game
            case "start":
                Log.Add(userInput);
                showMenu = false;
                isPlaying = true;
                Console.Clear();
                break;

            // Show the help menu
            case "help":
                Log.Add(userInput);
                Console.Clear();
                commandSelector("help");
                break;

            // Set the game to be in a playing state
            case "quit":
                Log.Add(userInput);
                showMenu = false;
                isPlaying = false;
                break;
        }
    }
}

// Function to display the map. Showing where the user is positioned and special rooms (like the goal, key, and RPS room) IF the user has found them
void displayMap(Vector2 userPosition)
{
    int gridMaxX = grid[0].Count - 1;
    int gridMaxY = grid.Count - 1;

    Vector2 currentPos = new Vector2(0, 0);

    // top border (walls) of the grid
    Console.Write("|");
    for (int i = 0; i < gridMaxX + 1; i++)
    {
        Console.Write("---");
    }
    Console.WriteLine("|");

    Console.Write("|");

    // map the grids content
    foreach (string? item in grid.SelectMany(row => row))
    {
        if (currentPos == userPosition) // user's position is marked with an "X" on the grid
        {
            Console.Write(" X ");
        }
        else
        {
            if (grid[(int)currentPos.Y][(int)currentPos.X] == " G " && userFoundGoal == false)    // Goal position is marked with a "G" on the grid, but only shown IF the user has found it
            {
                Console.Write(" - ");
            }
            else if (grid[(int)currentPos.Y][(int)currentPos.X] == " K " && userHasFoundKey == false)  // Key position is marked with a "K" on the grid, but only shown IF the user has found it
            {
                Console.Write(" - ");
            }
            else if (grid[(int)currentPos.Y][(int)currentPos.X] == "RPS" && userHasFoundRPS == false) // The position/room where the Rock Papper Scissors minigame is located is marked with "RPS"
            {
                Console.Write(" - ");
            }

            // Otherwise show the item in the grid. 
            else
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
            if ((userPosition.X + 1) <= (grid[0].Count - 1) && userInput == "right")
            {
                userPosition.X += 1;
                turnsLeft--;
                Log.Add("right"); // Add the inputed command to the log
            }
            else
            {
                Console.WriteLine("\nPLEASE, try to stay inside the mansion and don't try to go through it's (outer) walls.");
                if (userPosition.X >= grid[0].Count - 1) { userPosition.X = grid[0].Count - 1; }
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
                if (userPosition.Y >= grid.Count - 1) { userPosition.Y = grid.Count - 1; }
                Console.WriteLine("Press 'Enter' to acknowledge");
                Console.ReadLine();
            }
            break;

        case "down":
            if ((userPosition.Y + 1) <= grid.Count - 1 && userInput == "down")
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
            Console.WriteLine("Movement Commands:");
            Console.WriteLine("[Left], [Right], [Up], [Down] : Attempts to move the user in the specified direction.\n");
            Console.WriteLine("Moving into a room consumes a turn");
            Console.WriteLine("Remaining in the same room does NOT consume a turn\n");

            Console.WriteLine("Other commands:");
            Console.WriteLine("[Hint]: points you in the direction of the goal.\t\t(Prioritizing X-axis over Y)");
            Console.WriteLine("[Show Map]: Displays the map of the mansion.\t\t\t(Also shows special rooms on the map, IF they've been found)");
            Console.WriteLine("[Log]: Displays a log of all the commands you've inputed so far.");
            Console.WriteLine("[Redo]: Takes you back to the state prior to the last command.");
            Console.WriteLine("[Quit]: Quits the program.");
            Console.WriteLine("(These commands dont consume a turn)");

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

            Console.Clear();
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
            Log.Add("quit"); // Add the inputed command to the log (may be unnecessary since the game is ending, but just in case)
            isPlaying = false;
            break;

        case "": // IF nothing is typed, (usually when the game starts) then just continue
            break;
        default:
            Console.WriteLine("Invalid input.\nPlease choose a valid command (see help for possible commands).\nPress 'Enter' to acknowledge");
            Console.ReadLine();
            break;
    }
    currentRoom = grid[(int)userPosition.Y][(int)userPosition.X]; // Updating currentRoom to reflect the user's new position in the mansion(/grid) after their move

    // Clear the console for the next turn (and astethic reasons)
    Console.Clear();
}

// Function to handle the Rock Paper Scissors minigame
void RPS(string userInput)
{
    bool RPS_Tie = true;
    // In the event of a tie repeat the RPS game until there is a winner or loser
    while (RPS_Tie == true)
    {
        // Error handling for invalid input, keep asking until the user provides a valid response
        while ((userInput != "rock") && (userInput != "paper") && (userInput != "scissors"))
        {
            Console.WriteLine("Invalid input. Please choose [rock], [paper], or [scissors]:");
            userInput = Console.ReadLine().Trim().ToLower();
        }

        Log.Add($"{userInput}"); // Add the inputed command to the log

        Console.WriteLine($"As you pick up the {userInput} the two other items on the table vanish and a strange voice says:");
        Console.Write($"'Your choice is made~...'\n\nThe voice continues:\n'Now I will choose~...");

        int opponentChoiceNumber = new Random().Next(0, 3); // randomly choose a number between 0 and 2 for the opponent's choice
        string opponentChoiceTxt = ""; // This is just a temporary value and will be set in the switch below
                                       // Based on the opponent's choice(/random number) set 'opponentChoiceTxt' accordingly
        switch (opponentChoiceNumber)
        {
            case 0:
                Console.WriteLine("Rock!'\n");      // Display the opponent's choice
                opponentChoiceTxt = "Rock";
                break;
            case 1:
                Console.WriteLine("Paper!'\n");     // Display the opponent's choice
                opponentChoiceTxt = "Paper";
                break;
            case 2:
                Console.WriteLine("Scissors!'\n");  // Display the opponent's choice
                opponentChoiceTxt = "Scissors";
                break;
        }

        int userRPSchoice = 0; // This is just a temporary value and will be set below, based on the user's input
                               // Based on the user's choice(/input) set 'userRPSchoice' to a number for a comparison between the user's choice and the opponent's choice
        switch (userInput)
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

        // switch to handle the result of the RPS game
        switch (result)
        {
            // It's a tie
            case 0:
                Console.WriteLine("After a pause. The voice continues:\n'You have chosen the same as I, so we are equal~'");
                Console.WriteLine("\nPress 'Enter' to continue");

                // Effectively just a pause so the user can read the message before continuing
                Console.ReadLine();
                Console.Clear();

                // Prompt the user to choose again, (the RPS game effectively restarts)
                Console.WriteLine("The items on the table reappear and you are beckoned to choose again!\n");
                Console.WriteLine("Which do you choose?\n[rock], [paper], or [scissors]:");
                userInput = Console.ReadLine().Trim().ToLower();
                Console.Clear();

                RPS_Tie = true;
                break;

            // The user wins
            case 1:
                Console.WriteLine($"After a pause. The voice continues:\n'You have chosen better than I. {userInput} beats {opponentChoiceTxt} therefore you win!'");

                // IF the user has not found the goal yet, then reveal it to them as a reward for winning the RPS game
                if (userFoundGoal == false)
                {
                    Console.WriteLine("\n'As a reward I'll reveal where the goal of your exploration is located!'");
                    userFoundGoal = true;
                    displayMap(userPosition);
                    Console.WriteLine("\nPress 'Enter' to continue");
                    Console.ReadLine();
                    Console.Clear();
                }
                // otherwise, if the user has already found the goal, then give them more turns to explore the mansion as a reward for winning the RPS game
                else
                {
                    Console.WriteLine("\n'It seems you've already found the goal....\nSo! I'll simply give you more time to explore as you like!'");
                    Console.WriteLine("The mansion stabilizes slightly...");
                    turnsLeft += 5;
                    Console.WriteLine($"(You now have {turnsLeft} turns left)");
                    Console.WriteLine("\nPress 'Enter' to continue");
                    Console.ReadLine();
                    Console.Clear();
                }
                RPS_Tie = false;
                break;

            // The opponent wins
            case 2:
                Console.WriteLine($"After a pause. The voice continues:\n'You have chosen worse than I. {opponentChoiceTxt} beats {userInput} therefore you lose.'");
                Console.WriteLine("'As a punishment I'll teleport you somewhere else in the mansion!'");

                // Teleport the user to a random position in the mansion, but not to the RPS room
                while (userPosition == RPS_Position)
                {
                    userPosition = new Vector2(new Random().Next(0, grid[0].Count), new Random().Next(0, grid.Count));
                }
                // Display the map to show the user where they have been teleported to
                displayMap(userPosition);

                // Effectively just a pause so the user can read the message before continuing
                Console.WriteLine("\nPress 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
                RPS_Tie = false;
                break;
        }
    }
    // Now that the RPS game is over, update the current room to reflect the user's new position in the mansion(/grid)
    currentRoom = grid[(int)userPosition.Y][(int)userPosition.X];
}