using AppWeather.Services.Interfaces;
using Microsoft.Win32;

namespace AppWeather.Services.Implementations
{
    class FileDialogService : IFileDialogService
    {
        public string ShowSaveDialog(string title, string filter, string defaultExt)
        {
            var dialog = new SaveFileDialog
            {
                Title = title,
                Filter = filter,
                DefaultExt = defaultExt
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
