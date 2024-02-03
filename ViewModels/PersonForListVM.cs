namespace Draftor.ViewModels;

public class PersonForListVM : Core.ObservableObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    private bool _checked = false;

    public bool Checked
    {
        get { return _checked; }
        set { _checked = value; OnPropertyChanged(nameof(Checked)); }
    }
}