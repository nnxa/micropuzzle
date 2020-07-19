using System;

namespace Micropuzzle
{
    class Micropuzzle
    {
 
        static void Main()
        {

            // Initialise array of all nouns the game knows about
            var Nouns = new string[] {
                null, "KEY", "CASSETTE", "CHEESE", "PAPER", "THREAD", "REMOTE-CONTROL",
                    "BOTTLE", "TRAIN", "CAT", "DOOR", "SWITCH", "TUNNEL", "TREE", "HOLE", "MOUSE",
                "VIDEO", "COMPUTER", "GROCER", "TERMINAL", "111", "BUTTON", "MAXIMISER",
                "TV", "BOX", "NORTH", "SOUTH", "WEST", "EAST", "UP", "DOWN", "STOOL"
            };

            // Create constants to help with indexing Nouns array
            const int Key = 1;
            const int Cassette = 2;
            const int Cheese = 3;
            const int Paper = 4;
            const int Thread = 5;
            const int RemoteControl = 6;
            const int Bottle = 7;
            const int Train = 8;
            const int Cat = 9;            
            const int Switch = 11;            
            const int Tree = 13;            
            const int Video = 16;
            const int Computer = 17;
            const int Grocer = 18;
            const int Terminal = 19;
            const int Code111 = 20;
            const int Button = 21;
            const int Maximiser = 22;
            const int TV = 23;
            const int Box = 24;
            const int North = 25;
            const int South = 26;
            const int West = 27;
            const int East = 28;
            const int Up = 29;
            const int Down = 30;
            const int Stool = 31;

            // Of the 31 objects only the first 8 are items that can be moved around
            const int numberOfItems = 8;

            // Initialise starting locations for each item
            var ItemLocation = new int[] {
                -1, // Item 0 not used
                16, 12, 3, 21, 5, 17, 19, 14
            };

            var ItemFlags = new bool[] { 
                false, // Item 0 not used
                false, 
                true,  // 2.Cassette starts hidden in VCR
                false, 
                true,  // 4.Paper starts hidden in box
                false, 
                false, 
                false, 
                false  
            };

            
            var roomDescriptions = new string[] { 
                null, // Room 0 not used
                "INSIDE THE MOUSEHOLE - IT IS VERY DARK IN HERE", // 1
                "AT A MOUSEHOLE IN A CORNER OF THE ROOM", // 2
                "ON THE EDGE OF A HIGH TABLE", // 3
                "AT THE BACK OF A HALLWAY", // 4
                "IN A STORAGE ROOM", // 5
                "IN THE KITCHEN", // 6
                "FURTHER DOWN A DARK SMELLY TUNNEL", // 7
                "BY A RAILWAY SIDING", // 8
                "AT THE BASE OF A TALL PLASTIC TREE ON THE EDGE OF A HIGH TABLE", // 9
                "OUTSIDE THE OPEN DOOR OF AN ODDLY PROPORTIONED HOUSE", // 10
                "IN A YELLOW FRONT ROOM", // 11
                "BY A TV SET AND A RECORDER", // 12
                "AT THE END OF A DARK TUNNEL", // 13
                "BY A LARGE SWITCH CONNECTED TO THE RAILWAY TRACKS", // 14
                "ALONGSIDE THE WINDING TRACK", // 15
                "AT THE END OF THE LINE-THE TRACK DISAPPEARS THROUGH A HOLE IN THE WALL", // 16
                "BELOW A WHOLE WALL OF OVERSIZED VIDEO SCREENS", // 17
                "STANDING ON THE MAXIMISER PAD", // 18
                "ON A SHELF OF DISTURBING APPARATUS - THERE IS A STOOL NEARBY", // 19
                "ON A SHORT STEP STOOL", // 20
                "ON THE FLOOR OF AN OVERTURNED BOX OF BROKEN ELECTRONIC PARTS", // 21
                "AT A HOLE IN THE WALL FROM WHICH A RAILWAY LINE EMERGES", // 22
                "AT THE BASE OF A SWIVEL CHAIR", // 23
                "STANDING ON A COMPUTER TERMINAL WITH A SECURITY LOCK" // 24
            };

            // Define which exits the player is allowed to take from each room
            var exits = new string[] { null, // Room 0 not used
                "S,E", "S,W", "S,E", "S,W,E", "W,E", "S,W", "N,S", "N,S", "N,E", "N,W,E",
                "E,W", "N,W", "N,S", "N,E", "E,W", "W", "S", "S", "N,D", "U,D", "E,U", "E,W", "N,E,W", "N,W"
            };

            // Use the numbers 1-24 for each of the rooms above but also define some special locations
            const int Deleted = 0;
            const int PlayerInventory = 25;

            // Initialise game flags. These keep track of which puzzles the player has solved
            bool CatCleared = false;
            bool BottleOpened = false;
            bool TerminalUnlocked = false;
            bool GameCompleted = false;
            bool KeyOnTrain = false;
            bool SelfDestructTriggered = false;
                       
            // Initialise the game state
            var currentRoom = 11;
            var countdownTimer = 100;
            var message = "YOU AWAKEN..";
                        
            // Get a room description and print it
     start: Console.Clear();
            Console.WriteLine("MICRO PUZZLE");
            Console.WriteLine("============");
            Console.WriteLine($"YOU ARE {roomDescriptions[currentRoom]}");

            if (currentRoom == 20 && CatCleared == false) Console.WriteLine ("YOU ARE CONFRONTED BY A LARGE CAT");

            // Check if there is anything there
            for (int i = 1; i <= numberOfItems; i++)
            {
                if (ItemLocation[i] == currentRoom && ItemFlags[i] == false) Console.WriteLine($"THERE IS A {Nouns[i]} HERE");
            }

            // Find which directions you can go in and print them
            Console.WriteLine();
            Console.WriteLine($"YOU CAN GO {exits[currentRoom]}");            
            Console.WriteLine("------------------");

            // Default message
            Console.WriteLine(message);
            
            if (SelfDestructTriggered == true) Console.WriteLine($"SELF DESTRUCT COUNTDOWN AT : {countdownTimer}");

            // Get your instructions and split them into two words
            Console.WriteLine("WHAT WILL YOU DO NOW");
            var userCommand = Console.ReadLine().ToUpperInvariant();

            // Split the user input into the first word (verb) and the rest (noun)
            var commandArray = userCommand.Split(" ", 2);
            var verb = commandArray[0];
            var noun = "";            
            if (commandArray.Length > 1) noun = commandArray[1];

            // Check second word
            var objectNumber = Array.IndexOf(Nouns, noun);

            // Set up some default messages in case none of the rules below apply        
            message = "WHAT?";
            if (noun != "" && objectNumber < 1) message = "THAT IS SILLY";
            if (noun == "") message = "I NEED TWO WORDS";
            if (objectNumber > 0 && objectNumber <= numberOfItems) message = $"YOU DO NOT HAVE {noun}";

            // Counter
            countdownTimer -= 1;

            // Selects case depending on the verb you typed
            switch (verb)
            {
                case "I":
                case "INVENTORY":
                    Console.WriteLine("YOU ARE CARRYING:");
                    for (int i = 1; i <= numberOfItems; i++) {
                        if (ItemLocation[i] == PlayerInventory) Console.WriteLine(Nouns[i]);
                    }
                    message = "";
                    Console.WriteLine();
                    Console.WriteLine("PRESS RETURN TO CONTINUE");
                    Console.ReadLine();
                    break;

                case "GO":
                case "N":
                case "S":
                case "W":
                case "E":                
                case "U":
                case "D":                                    
                    // Subroutine which deals with your instructions
                    // about which direction you want to go
                    var direction = verb;

                    // If player typed full "GO NORTH", etc. convert to a single letter
                    if (objectNumber == North) direction = "N";
                    if (objectNumber == South) direction = "S";
                    if (objectNumber == East) direction = "E";
                    if (objectNumber == West) direction = "W";
                    if (objectNumber == Up) direction = "U";
                    if (objectNumber == Down) direction = "D";

                    // Block player movement if various conditions haven't been met
                    if (CatCleared == false && currentRoom == 20 && direction == "E") { message = "THE CAT WILL NOT LET YOU"; break; }
                    if (currentRoom == 2 && ItemLocation[Key] == PlayerInventory && direction == "W") { message = "YOU CANNOT TAKE THE KEY THROUGH"; break; }
                    if (currentRoom == 7 && ItemFlags[Cheese] == false) { message = "AN ANGRY MOUSE BARS YOUR WAY"; break; }
                                        
                    // Check whether player can move in direction and do so
                    var MovedInValidDirection = false;
                    if (exits[currentRoom].Contains("N") && direction == "N") { currentRoom -= 6; MovedInValidDirection = true; }
                    if (exits[currentRoom].Contains("S") && direction == "S") { currentRoom += 6; MovedInValidDirection = true; }
                    if (exits[currentRoom].Contains("W") && direction == "W") { currentRoom -= 1; MovedInValidDirection = true; }
                    if (exits[currentRoom].Contains("E") && direction == "E") { currentRoom += 1; MovedInValidDirection = true; }
                    if (exits[currentRoom].Contains("U") && direction == "U") { currentRoom -= 1; MovedInValidDirection = true; }
                    if (exits[currentRoom].Contains("D") && direction == "D") { currentRoom += 1; MovedInValidDirection = true; }
                    message = "OK";

                    // If  movement failed then let player know
                    if (MovedInValidDirection == false) message = "YOU CANNOT GO THAT WAY";                    
                    break;

                case "JUMP":                
                    message = "ARE YOU PRACTISING FOR THE OLYMPICS?";
                    if (currentRoom == 9 || currentRoom == 3) message = "IT IS TOO FAR TO JUMP";
                    break;

                case "GET":
                case "TAKE":                    
                    // Don't allow the player to pick up the train even though it appears as an item
                    // that can be picked up.
                    if (objectNumber == Train) { message = $"{verb} THE TRAIN EH? VERY FUNNY!"; break; }
                    
                    // Check object is an item that can be picked up
                    if (objectNumber > numberOfItems) { message = $"I CANNOT GET THE {noun}"; break; }
                    
                    // Check item is in the same room as the player
                    if (objectNumber > 0 && ItemLocation[objectNumber] != currentRoom) message = "IT IS NOT HERE";
                    
                    // If an item's flag is set it is invisible or has already been used
                    if (objectNumber > 0 && ItemFlags[objectNumber] == true) message = $"WHAT {noun}?";
                    
                    // Can't pick up an item already in your inventory
                    if (objectNumber > 0 && ItemLocation[objectNumber] == PlayerInventory) message = "YOU ALREADY HAVE IT";

                    // If we're all good, move into player inventory
                    if (objectNumber > 0 && ItemLocation[objectNumber] == currentRoom && ItemFlags[objectNumber] == false)
                    {
                        if (objectNumber == Key) KeyOnTrain = false;
                        ItemLocation[objectNumber] = PlayerInventory;
                        message = $"YOU HAVE THE {noun}";
                    }
                    break;

                case "PUT":                    
                    // For player to solve puzzle of placing the key on the train

                    // Check item is in player's inventory
                    if (ItemLocation[objectNumber] != PlayerInventory) break;
                    message = "NOT REALLY!";

                    // Check item is Key
                    if (objectNumber != Key) break;
                    
                    // Check player wants to put key on train
                    Console.WriteLine("PUT KEY WHERE?");
                    noun = Console.ReadLine().Trim().ToUpperInvariant();
                    objectNumber = Array.IndexOf(Nouns, noun);                    
                    if (objectNumber == Train) {
                        // Success
                        message = "WELL DONE!";
                        KeyOnTrain = true;
                        ItemLocation[Key] = currentRoom;
                    }
                    break;

                case "OPEN":
                    // If the player opens the bottle
                    if (BottleOpened == false && ItemLocation[Bottle] == PlayerInventory)
                    {
                        BottleOpened = true;
                        ItemFlags[Bottle] = true;                        
                        message = "A LOUDLY BUZZING FLY FLIES OUT";

                        // If the player opens the bottle in the presence of the cat
                        if (currentRoom == 20)
                        {
                            CatCleared = true;
                            message += " AND THE CAT CHASES AFTER IT!";
                        }
                    }

                    // If the player opens the box then reveal the paper
                    if (currentRoom == 21 && objectNumber == Box)
                    {
                        ItemFlags[Paper] = false;
                        message = "DUST SETTLES";
                    }

                    if (objectNumber != Video) break;
                    goto case "EXAMINE";

                case "EXAMINE":
                    // Various object descriptions
                    message = "NOTHING OF INTEREST";
                    if (objectNumber == Video && currentRoom == 12) { ItemFlags[Cassette] = false; message = "IT IS A MICRO - VCR"; }
                    if (objectNumber == Box && currentRoom == 21) message = "SOMETHING INSIDE";
                    if (objectNumber == Terminal && currentRoom == 24) goto case "UNLOCK";
                    if (objectNumber == TV && currentRoom == 12) message = "IT IS JUST A BOX WITH PHOTO STUCK ON";
                    if (objectNumber == Bottle) message = "IT CONTAINS A LARGE FLY";
                    if (objectNumber == Cat && currentRoom == 20 && CatCleared == false) message = "IT BITES AND SCRATCHES!";
                    if (objectNumber == Paper) goto case "READ";
                    if (objectNumber == Key && ItemLocation[1] == PlayerInventory) message = "THE NUMBER '111' IS ENGRAVED ON IT";
                    if (objectNumber == RemoteControl && ItemLocation[RemoteControl] == PlayerInventory) message = "THERE IS A BIG RED BUTTON";
                    break;

                case "READ":
                    message = "NOTHING OF INTEREST";

                    // Get terminal code if player reads paper
                    if (objectNumber == Paper && (ItemLocation[objectNumber] == PlayerInventory || ItemLocation[objectNumber] == currentRoom)) message = $"TERMINAL PASSWORD GROCER";

                    break;

                case "TIE":
                    // For tieing the thread to the tree
                    if (objectNumber != Thread) { message = "CANNOT TIE " + noun; break; }
                    if (ItemLocation[Thread] != PlayerInventory) break;                    
                    Console.WriteLine("TIE THE THREAD TO WHAT");
                    noun = Console.ReadLine().Trim().ToUpperInvariant();
                    objectNumber = Array.IndexOf(Nouns, noun); 
                    message = "CANNOT TIE IT TO " + noun;
                    if (objectNumber == Tree && currentRoom == 9)
                    {
                        ItemFlags[Thread] = true;
                        ItemLocation[Thread] = Deleted;
                        message = "IT IS SECURELY TIED.";
                    }
                    break;

                case "CLIMB":
                    // For climbing down the thread
                    if (objectNumber == Thread && ItemFlags[Thread] == false) message = "IT IS NOT TIED TO ANYTHING!";
                    if (objectNumber == Thread && currentRoom == 9 && ItemFlags[Thread] == true) { currentRoom = 8; message = "OK"; break; }
                    if (objectNumber == Thread && currentRoom == 8 && ItemFlags[Thread] == true) { currentRoom = 9; message = "OK"; }
                    if (objectNumber == Tree && currentRoom == 9) message = "IT IS TOO SMOOTH TO CLIMB";

                    // Player can also climb the stool instead of just typing "U" or "D"
                    if (objectNumber == Stool && currentRoom == 19) { objectNumber = Down; goto case "GO"; }
                    if (objectNumber == Stool && currentRoom == 20) { objectNumber = Up; goto case "GO"; }
                    break;

                case "POINT":
                    // For pointing the remote-control at the maximiser
                    if (objectNumber != RemoteControl || ItemLocation[RemoteControl] != PlayerInventory) break;
                    Console.WriteLine("POINT IT AT WHAT?");
                    noun = Console.ReadLine().Trim().ToUpperInvariant();
                    objectNumber = Array.IndexOf(Nouns, noun);
                    if (objectNumber == Maximiser) ItemFlags[RemoteControl] = true;
                    message = "VERY WELL";
                    break;

                case "EAT":
                    // Player can softlock by eating the cheese
                    if (objectNumber == Cheese && ItemLocation[objectNumber] == PlayerInventory) { 
                        ItemLocation[objectNumber] = Deleted; 
                        message = "MUNCH CHOMP"; 
                    }
                    break;

                case "UNLOCK":
                    // Player tries to unlock the terminal
                    if ((objectNumber == Terminal || objectNumber == Computer) && currentRoom == 24) {

                        if (ItemLocation[Key] == PlayerInventory) {
                            TerminalUnlocked = true;
                            message = "TERMINAL ACTIVE";
                            break;
                        }

                        // If the player doesn't have the key then start the self-destruct system
                        message = "*! TAMPER *!"; 
                        countdownTimer -= 12; 
                        SelfDestructTriggered = true;

                    }                        
                    break;

                case "LEAVE":
                case "DROP":

                    // If player drops cheese in presence of mouse then mouse is cleared
                    if (objectNumber == Cheese && currentRoom == 7 && ItemLocation[Cheese] == PlayerInventory) { 
                        ItemFlags[Cheese] = true; 
                        ItemLocation[Cheese] = Deleted;                         
                        message = "THE MOUSE RUNS OFF WITH IT."; 
                    }

                    if (ItemLocation[objectNumber] == PlayerInventory) { 
                        // If an ItemFlag is set to true the item will go invisible if it's dropped
                        if (ItemFlags[objectNumber] == true) { message = "BETTER HANG ON TO IT"; break; }
                        // Drop item
                        ItemLocation[objectNumber] = currentRoom; message = "DONE"; 
                    }

                    break;  
                    
                case "TYPE": // For typing commands into the terminal
                    if (currentRoom != 24) { 
                        message = "NOTHING HERE TO TYPE ON!";
                        break;
                    }

                    if (TerminalUnlocked == false) {
                        message = "THIS TERMINAL IS NON-ACTIVATED"; 
                        break; 
                    }

                    message = "THE TERMINAL ECHOES :" + noun;

                    if (objectNumber == Grocer) { 
                        message = "CODEWORD ACCEPTED"; 
                        TerminalUnlocked = true; 
                    }

                    if (objectNumber == Code111) message = "TERMINAL ID";

                    break;

                case "PRESS":

                    // Pressing railway switch
                    if (currentRoom == 14 && objectNumber == Switch) {
                        if (ItemLocation[Train] == 22)
                        {
                            ItemLocation[Train] = 14;
                            message = "THE TRAIN CHUGS INTO SIGHT AND STOPS HERE";
                            break;
                        }
                        if (ItemLocation[Train] == 14)
                        {
                            ItemLocation[Train] = 22;
                            message = "THE TRAIN CHUGS AWAY AND INTO A TUNNEL";
                        }
                        if (KeyOnTrain == true) ItemLocation[Key] = ItemLocation[Train];
                    }
                    
                    // Pressing remote-control button while pointed at maximiser completes game
                    if (objectNumber == Button && ItemFlags[RemoteControl] == true && ItemLocation[RemoteControl] == PlayerInventory && currentRoom == 18 && TerminalUnlocked == true) {                        
                        message = "THE MAXIMISER BEAM WORKS. YOU ARE RETURNED TO NORMAL SIZE";
                        GameCompleted = true;
                    }

                    break;

                default:
                    if (objectNumber > 0) message = $"YOU CAN'T {userCommand}";
                    if (objectNumber == 0) message = "YOU DO NOT MAKE SENSE";
                    break;
            }

            
            if (GameCompleted == false && countdownTimer > 0) goto start;
            
            // End of game
            if (countdownTimer < 1) message = "YOU HAVE RUN OUT OF TIME. THE MAXIMISER SELF DESTRUCTED";
            Console.WriteLine (message);
            
            if (countdownTimer > 0)

            return;

        }
       
    }
}
