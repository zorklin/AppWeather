namespace AppWeather.Services.Interfaces
{
    interface IFileDialogService
    {
        string ShowSaveDialog(string title, string filter, string defaultExt);
    }
}
