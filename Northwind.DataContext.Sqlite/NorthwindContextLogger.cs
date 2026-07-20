using Microsoft.EntityFrameworkCore;

namespace Northwind.DataContext.Sqlite;

public class NorthwindContextLogger
{
    public static void WriteLine(string message)
    {
        string logsFolderPath = Combine(GetFolderPath(SpecialFolder.DesktopDirectory), "book-logs");
        if(!Directory.Exists(logsFolderPath)) Directory.CreateDirectory(logsFolderPath);
        string logsDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        string logsFile = Path.Combine(logsFolderPath, $"northwindlog-{logsDateTime}.txt");

        using StreamWriter textWriter = File.AppendText(logsFile);
        textWriter.WriteLine(message);
    }
}
