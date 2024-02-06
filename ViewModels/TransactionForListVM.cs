using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Draftor.ViewModels;

public class TransactionForListVM : ObservableObject
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public double Value { get; set; }

    public DateTime Date { get; set; }

    public bool IsArchived { get; set; }

    private bool _isToRemove = false;

    public bool ToRemove
    {
        get => _isToRemove;
        set => SetProperty(ref _isToRemove, value);
    }
}