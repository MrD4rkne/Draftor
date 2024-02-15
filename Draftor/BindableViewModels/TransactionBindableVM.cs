using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Draftor.BindableViewModels;
public class TransactionBindableVM : ObservableObject
{
    public required int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required double Value { get; set; }

    public required DateTime Date { get; set; }

    public required bool IsArchived { get; set; }

    private bool _isToRemove;
    public bool IsToRemove
    {
        get => _isToRemove;
        set => SetProperty(ref _isToRemove, value);
    }
}
