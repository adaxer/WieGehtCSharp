using System.Diagnostics;

var sourcePath = Path.GetFullPath("..\\..\\..\\..\\bin\\debug\\net9.0");
var resx = Path.Combine(sourcePath, "Demo.de.resx");

File.Copy(resx, "Demo.de.resx", true);

var localizeHelper = Path.Combine(sourcePath, "LocalizeHelper.exe");

var info = new ProcessStartInfo
{
    FileName = localizeHelper,
    WorkingDirectory = Environment.CurrentDirectory,
    Arguments = string.Join(' ', args)
};

var process = Process.Start(info);
await process!.WaitForExitAsync();