using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Posicionamiento del jugador en la consola
int playerX = 0;
int playerY = 0;

// Posicionamiento de la comida en la consola
int foodX = 0;
int foodY = 0;

// Tipos de jugadores y comidas
string[] states = {"('-')", "(^-^)", "(X_X)"};
string[] foods = {"@@@@@", "$$$$$", "#####"};

// Tipo de jugador que empieza en la consola
string player = states[0];

// Primera comida en la consola
int food = 0;

InitializeGame();
while (!shouldExit) 
{
    if (TerminalResized()) 
    {
        Console.Clear();
        Console.Write("Console was resized. Program exiting.");
        shouldExit = true;
    } 
    else 
    {
        if (PlayerIsFaster()) 
        {
            Move(1, false);
        } 
        else if (PlayerIsSick()) 
        {
            FreezePlayer();
        } else 
        {
            Move(otherKeysExit: false);
        }
        if (GotFood())
        {
            ChangePlayer();
            ShowFood();
        }
    }
}

// Devuelve verdadero si el tamaño de la consola es modificada
bool TerminalResized() 
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Coloca comida random en lugares Random
void ShowFood() 
{
    // Actualiza la comida
    food = random.Next(0, foods.Length);

    // Actualiza la comida a una posicion random
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Imprime la comida en la locacion
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Devuelve verdadero si la posicion del jugador es la misma que el de la comida
bool GotFood() 
{
    return playerY == foodY && playerX == foodX;
}

// Devuelve verdadera si la apariencia del jugador representa un estado enfermo.
bool PlayerIsSick() 
{
    return player.Equals(states[2]);
}

// Devuelve verdadero si la apariencia del jugador representa un estado rápido
bool PlayerIsFaster() 
{
    return player.Equals(states[1]);
}

// Cambia el jugador para que coincida con la comida consumida.
void ChangePlayer() 
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Detiene temporalmente el movimiento del jugador
void FreezePlayer() 
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// Lee la entrada direccional de la consola y mueve el reproductor.
void Move(int speed = 1, bool otherKeysExit = false) 
{
    int lastX = playerX;
    int lastY = playerY;
    
    switch (Console.ReadKey(true).Key) {
        case ConsoleKey.UpArrow:
            playerY--; 
            break;
		case ConsoleKey.DownArrow: 
            playerY++; 
            break;
		case ConsoleKey.LeftArrow:  
            playerX -= speed; 
            break;
		case ConsoleKey.RightArrow: 
            playerX += speed; 
            break;
		case ConsoleKey.Escape:     
            shouldExit = true; 
            break;
        default:
            // Salir si se presiona alguna otra tecla
            shouldExit = otherKeysExit;
            break;
    }

    // Borrar los caracteres en la posición anterior.
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) 
    {
        Console.Write(" ");
    }

    // Mantenga la posición del jugador dentro de los límites de la ventana de Terminal
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Dibuja al jugador en la nueva ubicación.
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Limpia la consola, muestra la comida y el jugador.
void InitializeGame() 
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}