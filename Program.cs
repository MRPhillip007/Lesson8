using System.Text;
using System.Text.RegularExpressions;

class Program
{
    const string phoneBookMain = "test.txt";
    const string result = "result.txt";
    static void Main()
    {
        PrintMenu();
        bool isActive = true;
        while (isActive)
        {
            string? action = Console.ReadLine();
            if (action.ToLower() == "exit")
            {
                isActive = false;
            }

            ParamsHandler(action);
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine("<= Welcome to the PhoneBook program =>");
        Console.WriteLine("show - view all contacts ");
        Console.WriteLine("search - search for specific contact");
        Console.WriteLine("order - order all contacts");
        Console.WriteLine("cls - clear the console ");
        Console.WriteLine("menu - call main menu ");
    }
    static void ParamsHandler(string action)
    {
        switch (action.ToLower())
        {
            case "show":
                FileAction.ShowData("test.txt");
                break;

            case "search":
                Console.WriteLine("Enter contact info >>> ");
                string? searchContact = Console.ReadLine();
                Console.WriteLine(PhoneBook.SearchContact(phoneBookMain, searchContact));
                break;

            case "order":
                PhoneBook.Sort(result);
                Console.WriteLine("Check result.txt file ");
                break;

            case "cls":
                Console.Clear();
                break;

            case "menu":
                PrintMenu();
                break;

            default:
                Console.WriteLine("Wrong command");
                break;
        }
    }
}

class FileAction
{
    static bool IsFileExists(string file) => FileAction.IsFileExists(file);
    public static string[] GetContent(string file)
    {
        using (StreamReader reader = new StreamReader(file))
        {
            string[] content = reader.ReadToEnd().Split(new char[] { ';' });
            Array.Sort(content);

            return content;
        }
    }

    public static void ShowData(string file)
    {
        using (StreamReader reader = new StreamReader(file))
        {
            Console.WriteLine(reader.ReadToEnd());
        }
    }

    public static void WriteData(string file, string data)
    {
        using (StreamWriter writer = new StreamWriter(file, false))
        {
            writer.WriteLine(data);
            Console.WriteLine($"All data was written to {file}");
        }
    }
}

class DataParser
{
    public static string[] GetPhoneNumber(string str)
    {
        List<string> phones = new List<string>();

        // all kind of phone numbers
        Regex regex = new Regex(@"(\+3|8|\b)[\(\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[)\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)[\s-]*(\d)");
        MatchCollection match = regex.Matches(str);

        if (match.Count > 0)
        {
            foreach (Match m in match)
            {
                phones.Add(m.Value);
            }
        }
        return phones.ToArray();
    }

    public static string[] GetUserIfo(string str)
    {
        List<string> userInfo = new List<string>();

        // returns Name and Surname
        Regex regex = new Regex("[^;]*^[^:]*");
        MatchCollection matches = regex.Matches(str);

        if (matches.Count > 0)
        {
            foreach (Match m in matches)
            {
                userInfo.Add(m.Value);
            }
        }
        return userInfo.ToArray();
    }
}

class PhoneBook
{
    public static string SearchContact(string file, string name = "", string surname = "", string phoneNumber = "")
    {
        string[] personalInfo = FileAction.GetContent(file);
        string[] orderdered = personalInfo.Where(x => x.Contains(name) & x.Contains(surname) & x.Contains(phoneNumber)).ToArray();

        if (orderdered.Length > 0)
        {
            return String.Join(" ", orderdered);
        }
        return "Not found";
    }

    public static void Sort(string fileResult)
    {
        StringBuilder sb = new StringBuilder();
        string[] content = FileAction.GetContent("test.txt");

        foreach (var str in content)
        {
            foreach (var contact in DataParser.GetUserIfo(str))
            {
                sb.Append(contact + " ");
            }
            foreach (var phone in DataParser.GetPhoneNumber(str))
            {
                sb.Append(phone + '\n');
            }
        }
        FileAction.WriteData(fileResult, "After sorting" + sb.ToString());
    }
    }
//checked
