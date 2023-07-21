using System.Drawing;
using System.Text;
using System.Text.Json;

namespace TestAppPARI_elementary_;

internal static class Program
{
    private static void Main(string[] args)
    {
        string path;
        Stage[]? stages;
        do
        {
            Console.WriteLine("Введите путь до .json-файла");
            path = Console.ReadLine()!;
            stages = DeserializeStages(path);
        }
        while (path == null || stages == null);

        
        DocumentApproval(stages);

        DrawTable(stages);
    }

    private static void DrawTable(Stage[] stages)
    {
        // Заголовки столбцов
        string header1 = "Наименование этапа";
        string header2 = "Согласующий";
        string header3 = "Результат";
        string header4 = "Комментарий";

        // Длины столбцов
        const int column1Width = 27;
        const int column2Width = 13;
        const int column3Width = 15;
        const int column4Width = 50;
        const int columnBorderChars = 13;

        // Границы столбцов
        string border = GetDashLine(column1Width + column2Width + column3Width + column4Width + columnBorderChars);

        // Вывод заголовков столбцов
        Console.WriteLine(border);
        Console.WriteLine($"| {header1,-column1Width} | {header2,-column2Width} | {header3,-column3Width} | {header4,-column4Width} |");
        Console.WriteLine(border);

        // Вывод данных
        foreach (var stage in stages)
        {
            Console.Write($"| {stage.Name,-column1Width} ");
            Console.Write($"| {stage.Performer,-column2Width} ");
            WriteApproval(stage.Approval, -column3Width);
            Console.WriteLine($"| {stage.Comment,-column4Width} |");
            Console.WriteLine(border);
        }

        void WriteApproval(Approval? approval, int columnWidth)
        {
            Console.Write("| ");
            switch (approval)
            {
                case Approval.Approval:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{"Согласовано",column3Width} ");
                    break;
                case Approval.NotApproval:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{"Не согласовано",column3Width} ");
                    break;
                case Approval.Skipped:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{"Пропущено",column3Width} ");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    private static Stage[]? DeserializeStages(string path)
    {
        string content = File.ReadAllText(path);
        Stage[]? stages = null;
        try
        {
           stages = JsonSerializer.Deserialize<StagesJSON>(content)!.Stages;
        }
        catch
        {
            Console.Error.WriteLine("Ошибка десериализации");
        }
        return stages;
    }

    private static void DocumentApproval(Stage[] stages)
    {
        Console.WriteLine("\t\tПроцесс согласование документа");
        Console.WriteLine("\nВведите 'y(yes)' если согласовано, либо 'n(no)' если не согласовано");

        foreach (var stage in stages)
        {
            Console.WriteLine($"\n{GetDashLine(85)}");
            StageApproval(stage);
        }
    }

    private static void StageApproval(Stage stage)
    {
        Console.WriteLine($"Наименование этапа: {stage.Name}");
        Console.WriteLine($"Согласующий: {stage.Performer}");
        stage.Approval = GetApproval(stage);
        Console.Write("Комментарий: ");
        stage.Comment = Console.ReadLine();
    }

    private static Approval GetApproval(Stage stage)
    {
        Console.Write("Согласовано: ");
        var answer = Console.ReadLine();
        Approval result = 0;
        while (stage.Approval == null)
        {
            switch (answer)
            {
                case "y":
                case "yes": return Approval.Approval;
                case "n":
                case "no": return Approval.NotApproval;
                case "": return Approval.Skipped;
                default:
                    Console.WriteLine("\nВведите 'y(yes)' если согласовано, либо 'n(no)' если не согласовано");
                    Console.Write("Согласовано: ");
                    answer = Console.ReadLine();
                    continue;
            }
        }
        return result;
    }

    private static string GetDashLine(int countDashes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < countDashes; i++)
        {
            stringBuilder.Append("-");
        }
        return stringBuilder.ToString();
    }
}