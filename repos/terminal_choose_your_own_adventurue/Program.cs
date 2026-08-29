using System.Numerics;

// Defining the grid for the game, where " K " is the key, " G " is the goal, and " - " are empty spaces
string[,] grid = new string[6, 6]
{
    { " - ", " K ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " },
    { " - ", " G ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " },
    { " - ", " - ", " - ", " - ", " - ", " - " }
};
// Defining the variables for the game
Vector2 userPosition = new Vector2(0, 0);

int turnsLeft = grid.GetUpperBound(0) * grid.GetUpperBound(1); // turns left before the game ends
bool isPlaying = true;

// Defining and setting where the key and the goal are located
Vector2 keyPosition = new Vector2(0, 0);  // Temporary position until we find the key in the grid
Vector2 goalPosition = new Vector2(0, 0); // Temporary position until we find the goal in the grid

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
    }
}
// Defining the function to check if the user has found the key or reached the goal
bool userFoundGoal = false;
bool userHasFoundKey = false;
bool userHasKey = false;

string userInput = "";

// Game loop
Console.WriteLine("Welcome to the land of ZORK!");

while (isPlaying && turnsLeft > 0)
{
    displayMap(userPosition);

    if (userPosition == goalPosition && userHasKey == true)
    {
        Console.WriteLine("Congratulation! you've reached the goal and aquired the key!\nDo you wish to use the key?");
        userInput = Console.ReadLine().ToLower();
        // Check if the user wants to use the key
        if (userInput == "yes")
        {
            Console.WriteLine("\nYou use the key to unlock the heavy door and beyond it lays...\n(Well we'll see you'll have to stay tuned untill the next installment of ZORK\nYOU WIN!!!");
            isPlaying = false;
            break;
        }
        else if (userInput == "no")
        {
            Console.WriteLine("\nstrange... There's not much else you can do besides this but alright...");
            turnsLeft += 1;
        }

        // Error handling for invalid input, keep asking until the user provides a valid response
        while (userInput != "yes" && userInput != "no")
        {
            Console.WriteLine("Invalid input. Please choose 'Yes' or 'No'.");
            userInput = Console.ReadLine().ToLower();

            // Check if the user wants to use the key
            if (userInput == "yes")
            {
                Console.WriteLine("\nYou use the key to unlock the heavy door and beyond it lays...\n(Well we'll see you'll have to stay tuned untill the next installment of ZORK\nYOU WIN!!!");
                isPlaying = false;
                break;
            }
            else if (userInput == "no")
            {
                Console.WriteLine("\nstrange... There's not much else you can do besides this but alright...");
                turnsLeft += 1;
            }
        }
    }
    else if (userPosition == goalPosition && userHasKey == false)
    {
        Console.WriteLine("You found the goal, but you don't have the key to win the game!\nKeep looking for the key!\n");
        userFoundGoal = true;
    }

    if (userPosition == keyPosition)
    {
        Console.WriteLine("\nThere's a key in this room!\nWould u like to pick it up?\n\n[Yes] | [No]");
        userInput = Console.ReadLine().ToLower();

        if (userInput == "yes")
        {
            Console.WriteLine("\nYou pick up the key!");
            keyPosition = new Vector2(-1, -1); // just to make it dissappear
            userHasKey = true;
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
            displayMap(userPosition);
        }
        else if (userInput == "no")
        {
            Console.Clear();
            Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...\n");
            displayMap(userPosition);
        }

        // error handling for invalid input, keep asking until the user provides a valid response
        while (userInput != "yes" && userInput != "no")
        {
            Console.WriteLine("Invalid input. Please choose 'Yes' or 'No'.");
            userInput = Console.ReadLine().ToLower();

            // Check if the user wants to pick up the key
            if (userInput == "yes")
            {
                Console.WriteLine("You pick up the key!");
                keyPosition = new Vector2(-1, -1); // just to make it dissappear
                userHasKey = true;
                Console.WriteLine("Press 'Enter' to continue");
                Console.ReadLine();
                Console.Clear();
                displayMap(userPosition);
            }
            else if (userInput == "no")
            {
                Console.Clear();
                Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...\n");
                displayMap(userPosition);
            }
        }
    }

    if (isPlaying)
    {
        Console.WriteLine($"It's time to move, you've only got {turnsLeft} turns left!\n\nWhere are you headed?");
        Console.WriteLine("Possible paths: 'Left', 'Right', 'up', 'down', 'hint', ('Quit')\nChoose Your path:");
        userInput = Console.ReadLine().ToLower();

        turnsLeft--; // Decrement turns left after each move   

        if (turnsLeft == 0)
        {
            Console.WriteLine("You've run out of turns! Game Over.\nYOU LOSE!");
        }

        switch (userInput)
        {
            case "left":
                if ((userPosition.X - 1) >= 0 && userInput == "left")
                {
                    userPosition.X += -1;
                }
                else
                {
                    Console.WriteLine("\nStay inside the bounds of the grid PLEASE!");
                    if (userPosition.X <= 0) { userPosition.X = 0; }
                    turnsLeft += 1;
                    Console.WriteLine("Press 'Enter' to acknowledge");
                    Console.ReadLine();
                }
                break;

            case "right":
                if ((userPosition.X + 1) <= (grid.GetLength(0) - 1) && userInput == "right")
                {
                    userPosition.X += 1;
                }
                else
                {
                    Console.WriteLine("\nStay inside the bounds of the grid PLEASE!");
                    if (userPosition.X >= grid.GetLength(0) - 1) { userPosition.X = grid.GetLength(0) - 1; }
                    turnsLeft += 1;
                    Console.WriteLine("Press 'Enter' to acknowledge");
                    Console.ReadLine();
                }
                break;

            case "up":
                if ((userPosition.Y - 1) >= 0 && userInput == "up")
                {
                    userPosition.Y -= 1;
                }
                else
                {
                    Console.WriteLine("\nStay inside the bounds of the grid PLEASE!");
                    if (userPosition.Y >= grid.GetLength(1) - 1) { userPosition.Y = grid.GetLength(1) - 1; }
                    turnsLeft += 1;
                    Console.WriteLine("Press 'Enter' to acknowledge");
                    Console.ReadLine();
                }
                break;

            case "down":
                if ((userPosition.Y + 1) <= grid.GetLength(1) - 1 && userInput == "down")
                {
                    userPosition.Y += 1;
                }
                else
                {
                    Console.WriteLine("\nStay inside the bounds of the grid PLEASE!");
                    if (userPosition.Y <= 0) { userPosition.Y = 0; }
                    turnsLeft += 1;
                    Console.WriteLine("Press 'Enter' to acknowledge");
                    Console.ReadLine();
                }
                break;
            case "hint":
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
                    Console.WriteLine("above you...");
                }
                else if (userPosition.Y > goalPosition.Y)
                {
                    Console.WriteLine("below you...");
                }
                break;

            case "quit":
                isPlaying = false;
                break;

            case "": // IF nothing is typed, (usually when the game starts) then just continue
                break;
            default:
                Console.WriteLine("Invalid input. Please choose a valid path (Within bounds).\nPress 'Enter' to acknowledge");
                Console.ReadLine();
                break;
        }
        Console.Clear();
    }
}


// Function to display the map with the user's position, where the Goal is, the key, and the "walls"
void displayMap(Vector2 userPosition)
{
    int gridMaxX = grid.GetUpperBound(0);
    int gridMaxY = grid.GetUpperBound(1);

    Vector2 currentPos = new Vector2(0, 0);

    // top border
    Console.Write("|");
    for (global::System.Int32 i = 0; i < gridMaxX + 1; i++)
    {
        Console.Write("---");
    }
    Console.WriteLine("|");

    Console.Write("|");

    // map content
    foreach (var item in grid)
    {
        if (currentPos == userPosition)
        {
            Console.Write(" X ");
        }
        else
        {
            if (item == " G " && userFoundGoal == false)
            {
                Console.Write(" - ");
            }
            if (item == " K " && userHasFoundKey == false)
            {
                Console.Write(" - ");
            }

            if ((item == " G " && userFoundGoal == false) == false && ((item == " K " && userHasFoundKey == false) == false))
            {
                Console.Write(item);
            }

        }
        if (currentPos.X == gridMaxX)
        {
            Console.WriteLine("|");
        }
        if (currentPos.X + 1 > gridMaxX)
        {
            currentPos.X = 0;
            currentPos.Y++;
            Console.Write("|");
        }
        else
        {
            currentPos.X++;
        }
    }

    // bottom border
    for (global::System.Int32 i = 0; i < gridMaxX + 1; i++)
    {
        Console.Write("---");
    }
    Console.WriteLine("|");
}