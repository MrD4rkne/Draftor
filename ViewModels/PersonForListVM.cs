
using CommunityToolkit.Mvvm.ComponentModel;

namespace Draftor.ViewModels;

public class PersonForListVM : ObservableObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    private bool _checked = false;

    public bool Checked
    {
        get => _checked;
        set => SetProperty(ref _checked, value);
    }
}