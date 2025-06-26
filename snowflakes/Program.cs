var exit = false;

while (!exit)
{
    Console.WriteLine("Hello, please input a number of your choice or write ESC to exit");

    int seed = 0;
    var input = Console.ReadLine();
    while (input != "ESC" && (!int.TryParse(input, out seed) || seed < 1))
    {
        Console.WriteLine("Invalid input. Please enter a number greater than 0 or write ESC to exit.");
        input = Console.ReadLine();
    }

    if (input == "ESC")
    {
        exit = true;
        continue;
    }

    GenerateSnowflake(seed);
}

Console.ReadLine();

static char ReplaceHorizontal(char rowChar)
{
    return rowChar switch
    {
        '\\' => '/',
        '/' => '\\',
        _ => rowChar,
    };
}

static char ReplaceVerticalChar(char rowChar)
{
    return rowChar switch
    {
        '\\' => '/',
        '/' => '\\',
        'o' => '°',
        '°' => 'o',
        _ => rowChar,
    };
}

static string ReplaceVertical(string row)
{
    var result = string.Empty;
    foreach (var rowChar in row)
        result += ReplaceVerticalChar(rowChar);
    return result;
}

static void GenerateSnowflake(int seed)
{
    //char[] ascii_chars = { '\\', '/', '\\', '/', '\\', '/', '\\', '/', '\\', '/', '|', '|', 'o', 'O', '*', '+' };
    char[] ascii_chars = { '\\', '\\', '\\', '\\', '\\', '|', '|', 'o', 'O', '*', '+' };
    char[] ascii_central_chars = { '|', '|', '|', '|', '|', '|', 'o', '*', '+', '°' };
    char[] ascii_center_chars = { 'o', '0', '@', '+', '*', 'X' };
    char[] ascii_center_row_chars = { '-', '-', '-', '-', '-', '-', '*', '*', 'o', 'O', '+', ' ' };

    var random = new Random(seed);

    // We'll use a grid of 13x7 for the snowflakes.
    // ****************************
    // * Example:
    // *       o   
    // *    o  |  o 
    // *   . \ | / .
    // * X'-:-'O'-:-'X
    // *   . / | \ . 
    // *    o  |  o 
    // *       o  
    // ****************************

    // To make them more unique the size can vary +2.
    var increaseSize = random.Next(0, 2) == 1 ? 2 : 0;
    var columnNumber = 13 + increaseSize;
    var rowNumber = 7 + increaseSize;

    // We always have a center point in the grid and odd number of rows and columns.
    var upperRows = new List<string>();
    var lowerRows = new List<string>();
    var halfRowNumber = rowNumber / 2;
    var halfColumnNumber = columnNumber / 2;

    // Upper half of the snowflake.
    for (var i = 0; i < halfRowNumber; i++)
    {
        var row = string.Empty;

        // Left side of the snowflake.
        for (int j = 0; j < halfColumnNumber; j++)
        {
            // Outer rows are sparse, inner rows are denser.
            var hasChar = random.Next(0, 10) > 7 - i;

            // The first chars in outer rows are always empty.
            if (j < halfRowNumber - i)
                hasChar = false;

            if (hasChar)
                row += ascii_chars[random.Next(0, ascii_chars.Length)];
            else
                row += ' ';
        }

        // Center of the snowflake.
        row += ascii_central_chars[random.Next(0, ascii_central_chars.Length)];

        // Right side of the snowflake.
        for (var j = 0; j < halfColumnNumber; j++)
            row += ReplaceHorizontal(row[halfColumnNumber - j - 1]);

        upperRows.Add(row);
    }

    // Center row of the snowflake.
    var centerRow = string.Empty;
    for (var i = 0; i < halfColumnNumber; i++)
    {
        centerRow += ascii_center_row_chars[random.Next(0, ascii_center_row_chars.Length)];
    }
    centerRow += ascii_center_chars[random.Next(0, ascii_center_chars.Length)];
    for (var i = 0; i < halfColumnNumber; i++)
    {
        centerRow += ReplaceHorizontal(centerRow[halfColumnNumber - i - 1]);
    }

    // Lower half of the snowflake.
    for (var i = 0; i < halfRowNumber; i++)
        lowerRows.Add(ReplaceVertical(upperRows[halfRowNumber - i - 1]));

    Console.WriteLine();

    foreach (var row in upperRows)
        Console.WriteLine(row);
    Console.WriteLine(centerRow);
    foreach (var row in lowerRows)
        Console.WriteLine(row);

    Console.WriteLine();
}