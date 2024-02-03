using System.ComponentModel;

namespace Draftor.ViewModels;

public class TransactionListViewModel : INotifyPropertyChanged
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public double Value { get; set; }

    public bool IsArchived { get; set; }

    private bool _isToRemove = false;

    public bool ToRemove
    {
        get
        {
            return _isToRemove;
        }
        set
        {
            _isToRemove = value;
            OnPropertyChanged(nameof(ToRemove));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string name = "")
    {
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
    }
}