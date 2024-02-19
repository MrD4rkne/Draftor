using CommunityToolkit.Mvvm.ComponentModel;

namespace Draftor.BindableViewModels;

public class TransactionBindableVM : ObservableObject
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public decimal Value { get; set; }

    public DateTime Date { get; set; }

    public bool IsArchived { get; set; }

    private bool _isToRemove;

    public bool IsToRemove
    {
        get => _isToRemove;
        set => SetProperty(ref _isToRemove, value);
    }
}