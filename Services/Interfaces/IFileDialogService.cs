namespace AppWeather.Services.Interfaces
{
    public interface IFileDialogService
    {
        string? ShowSaveDialog(string title, string filter, string defaultExt);
    }
}
