using System.ComponentModel;

namespace Draftor.Core;

public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string name = "")
    {
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
    }
}