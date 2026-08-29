using System.Numerics;

Vector2 userPosition = new Vector2(0, 0);
String userInput = "";

Vector2 goalPosition = new Vector2(1, 1);
bool userHasFoundGoal = false;

Vector2 keyPosition = new Vector2(3, 3);
bool userHasKey = false;

// Define the boundaries of the grid
int nr_of_rows_Y_MAX = 5; // nr of rows (+1)    | aka y-axis
int nr_of_columns_X_MAX = 6; // nr of columns   | aka x-axis

int turnsLeft = nr_of_rows_Y_MAX * nr_of_columns_X_MAX; // turns left before the game ends, if the player doesn't find the goal
bool isPlaying = true;

// Start of Game "loop"  -------|
Console.WriteLine("Welcome to the world of Zork!");

while (isPlaying && turnsLeft > 0)
{
    showUserLocation(userPosition);

    if (userPosition == goalPosition && userHasKey)
    {
        Console.WriteLine("Congratulation! you've reached the goal and aquired the key!\nDo you wish to use the key?");
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
            }
        }
    }
    else if (userPosition == goalPosition && !userHasKey)
    {
        Console.WriteLine("You found the goal, but you don't have the key to win the game!\nKeep looking for the key!\n");
        userHasFoundGoal = true;
    }

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
                Console.WriteLine("Stay inside the bounds of the grid PLEASE");
                if (userPosition.X <= 0) { userPosition.X = 0; }
            }
            break;

        case "right":
            if ((userPosition.X + 1) <= (nr_of_columns_X_MAX - 1) && userInput == "right")
            {
                userPosition.X += 1;
            }
            else
            {
                Console.WriteLine("Stay inside the bounds of the grid PLEASE");
                if (userPosition.X >= nr_of_columns_X_MAX) { userPosition.X = nr_of_columns_X_MAX; }
            }
            break;

        case "up":
            if ((userPosition.Y + 1) <= nr_of_rows_Y_MAX && userInput == "up")
            {
                userPosition.Y += 1;
            }
            else
            {
                Console.WriteLine("Stay inside the bounds of the grid PLEASE");
                if (userPosition.Y >= nr_of_rows_Y_MAX) { userPosition.Y = nr_of_rows_Y_MAX; }
            }
            break;

        case "down":
            if ((userPosition.Y - 1) >= 0 && userInput == "down")
            {
                userPosition.Y += -1;
            }
            else
            {
                Console.WriteLine("Stay inside the bounds of the grid PLEASE");
                if (userPosition.Y <= 0) { userPosition.Y = 0; }
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

    if (userPosition == keyPosition)
    {
        Console.WriteLine("\nThere's a key in this room!\nWould u like to pick it up?\n\n[Yes] | [No]");
        userInput = Console.ReadLine().ToLower();

        if (userInput == "yes")
        {
            Console.WriteLine("You pick up the key!");
            keyPosition = new Vector2(-1, -1); // just to make it dissappear
            userHasKey = true;
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
        }
        else if (userInput == "no")
        {
            Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...");
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
            }
            else if (userInput == "no")
            {
                Console.WriteLine("strange... You'll need the key to win the game, but I guess you can keep looking for the goal without it...");
            }
        }
    }
    Console.Clear();
}
void showUserLocation(Vector2 userPosition)
{
    // top border
    Console.Write("|");
    for (global::System.Int32 i = 0; i < nr_of_columns_X_MAX; i++)
    {
        Console.Write("---");
    }
    Console.WriteLine("|");

    Vector2 currentPOS = new Vector2(0, nr_of_rows_Y_MAX);

    // grid displaying...
    while (currentPOS.Y! > -1)
    {
        Console.Write("|");
        while (currentPOS.X! < nr_of_columns_X_MAX)
        {
            if (userPosition == currentPOS)
            {
                Console.Write(" x ");
            }
            else if (goalPosition == currentPOS && userHasFoundGoal)
            {
                Console.Write(" G ");
            }
            else
            {
                Console.Write(" - ");
            }
            currentPOS += new Vector2(+1, 0);
        }
        Console.WriteLine("|");
        currentPOS.X = 0;
        currentPOS += new Vector2(0, -1);
    }
    // bottom border
    Console.Write("|");
    for (global::System.Int32 i = 0; i < nr_of_columns_X_MAX; i++)
    {
        Console.Write("---");
    }
    Console.WriteLine("|");
}