using System.Net.Http.Json;

public class Program
{


    /*დაწერეთ ფუნქცია, რომელსაც გადაეცემა ტექსტი  და აბრუნებს პალინდრომია თუ 
    არა. (პალინდრომი არის ტექსტი რომელიც ერთნაირად იკითხება ორივე მხრიდან).  
      */
    public static bool IsPalindrome(string text)
    {
        for (int i = 0; i < text.Length / 2; i++)
        {
            if (text[i] != text[text.Length - 1 - i])
                return false;
        }
        return true;
    }

    /* გვაქვს 1,5,10,20 და 50 თეთრიანი მონეტები. დაწერეთ ფუნქცია, რომელსაც 
    გადაეცემა თანხა (თეთრებში) და აბრუნებს მონეტების მინიმალურ რაოდენობას, 
    რომლითაც შეგვიძლია ეს თანხა დავახურდაოთ.*/
    public static int MinSplit(int amount)
    {
        int[] coins = { 50, 20, 10, 5, 1 };
        int count = 0;
        foreach (int coin in coins)
        {
            count += amount / coin;
            amount %= coin;
        }
        return count;
    }

    /*  მოცემულია მასივი, რომელიც შედგება მთელი რიცხვებისგან. დაწერეთ ფუნქცია 
    რომელსაც გადაეცემა ეს მასივი და აბრუნებს მინიმალურ მთელ რიცხვს, რომელიც 0-ზე 
    მეტია და ამ მასივში არ შედის.*/
    public static int NotContains(int[] array)
    {
        var numbers = new HashSet<int>(array);
        int missing = 1;
        while (numbers.Contains(missing)) missing++;
        return missing;
    }

    /* მოცემულია String რომელიც შედგება „(„ და „)“ ელემენტებისგან. დაწერეთ ფუნქცია 
    რომელიც აბრუნებს ფრჩხილები არის თუ არა მათემატიკურად სწორად დასმული.*/
    public static bool IsProperly(string sequence)
    {
        int balance = 0;
        foreach (char c in sequence)
        {
            balance += c == '(' ? 1 : -1;
            if (balance < 0) return false;
        }
        return balance == 0;
    }

    /*   გვაქვს n სართულიანი კიბე, ერთ მოქმედებაში შეგვიძლია ავიდეთ 1 ან 2 საფეხურით. 
    დაწერეთ ფუნქცია რომელიც დაითვლის n სართულზე ასვლის ვარიანტების 
    რაოდენობას.*/
    public static int CountVariants(int stairCount)
    {
        if (stairCount <= 1) return 1;
        int a = 1, b = 1;
        for (int i = 2; i <= stairCount; i++)
            (a, b) = (b, a + b);
        return b;
    }

    /*მოცემულია Countries REST API ის მისამართი: https://restcountries.com/v3.1/all, 
    რომელიც აბრუნებს ინფორმაციას ქვეყნების შესახებ. დაწერეთ ფუნქცია, რომელიც 
    ყოველი ქვეყნისთვის შექმნის ტექსტურ დოკუმენტს (.txt) სახელად {ქვეყნის_სახელი.txt}. 
    თითოეულ დოკუმენტში უნდა იყოს შევსებული ქვეყნის “region”, “subregion”, “latlng”, 
    “area” და “population” ველები.*/
    public static async Task GenerateCountryDataFiles()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync("https://restcountries.com/v3.1/all");
        var countries = await response.Content.ReadFromJsonAsync<List<Country>>();

        foreach (var country in countries)
        {
            string fileName = $"{country.Name.Common.Replace(" ", "_")}.txt";
            string content = $"Region: {country.Region}\nSubregion: {country.Subregion}\n"
                           + $"Coordinates: {string.Join(", ", country.Latlng)}\n"
                           + $"Area: {country.Area}\nPopulation: {country.Population}";
            await File.WriteAllTextAsync(fileName, content);
        }
    }

    private class Country
    {
        public Name Name { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public double[] Latlng { get; set; }
        public decimal Area { get; set; }
        public long Population { get; set; }
    }

    private class Name
    {
        public string Common { get; set; }
    }

    /*
      სინქრონიზაციის შესრულება კონსოლურ აპლიკაციაში SemaphoreSlim-ის მეშვეობით 
    აღწერა: ამ დავალებაში, თქვენ მოგიწევთ C# და .NET-ის გამოყენებით კონსოლური 
    აპლიკაციის შექმნა, რაც გამოჩნდება დავალების სინქრონიზაციისა და კონკურენციის 
    კონტროლის გამოცდილების მაღალ დონეზე. აპლიკაციამ უნდა შეასრულოს ორი 
    ძირითადი ფუნქცია უსასრულო ციკლში: "1" და "0"–ის უწყვეტი გამოტანა: აპლიკაციამ 
    უნდა გამოიტანოს რიცხვები "1" და "0" კონსოლში მწვანე ფერში, რიცხვებს შორის 
    განშორების გარეშე. პერიოდული გზავნილის ჩვენება: ყოველ ხუთ წამში, აპლიკაციამ 
    უნდა შეაჩეროს "1" და "0"–ის უწყვეტი გამოტანა, რათა კონსოლში ყვითელ ფერში 
    გამოიტანოს გზავნილი "Neo, you are the chosen one", შემდეგ დააპაუზოს გამოტანა 
    დამატებითი ხუთი წამი, სანამ გაგრძელდება "1" და "0"–ის უწყვეტი გამოტანა.*/
    private static SemaphoreSlim _lock = new SemaphoreSlim(1);
    private static bool _running = true;

    private static async Task MatrixEffect()
    {
        Console.CancelKeyPress += (s, e) => _running = false;
        Console.ForegroundColor = ConsoleColor.Green;

        var tasks = new[]
        {
            Task.Run(PrintBinary),
            Task.Run(ShowMessages)
        };

        await Task.WhenAll(tasks);
    }

    private static async Task PrintBinary()
    {
        var rng = new Random();
        while (_running)
        {
            await _lock.WaitAsync();
            Console.Write(rng.Next(2));
            _lock.Release();
            await Task.Delay(50);
        }
    }

    private static async Task ShowMessages()
    {
        while (_running)
        {
            await Task.Delay(5000);
            await _lock.WaitAsync();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nNeo, you are the chosen one");
            await Task.Delay(5000);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            _lock.Release();
        }
    }



    public static void Main(string[] args)
    {

        Console.WriteLine("1. Palindrome Tests:");
        Console.WriteLine($"Racecar: {IsPalindrome("racecar")}");
        Console.WriteLine($"Hello: {IsPalindrome("hello")}");

        Console.WriteLine("\n2. Coin Split:");
        Console.WriteLine($"87 tetri: {MinSplit(87)} coins");

        Console.WriteLine("\n3. Missing Number:");
        int[] nums = { 1, 3, 6, 4, 1, 2 };
        Console.WriteLine(NotContains(nums));

        Console.WriteLine("\n4. Parentheses Check:");
        Console.WriteLine($"(()()): {IsProperly("(()())")}");
        Console.WriteLine($"())(: {IsProperly("())(")}");

        Console.WriteLine("\n5. Stair Variants:");
        Console.WriteLine($"3 steps: {CountVariants(3)} ways");

        Console.WriteLine("\n8. Country Data Export (check filesystem)...");
        GenerateCountryDataFiles().GetAwaiter().GetResult();

        Console.WriteLine("\n9. Matrix Effect (press CTRL+C to exit)...");
        MatrixEffect().GetAwaiter().GetResult();
    }
}