using CommunityToolkit.Mvvm.ComponentModel;
using Core.Observable;

namespace Core.MVVM;

public abstract class ViewModelBase : ObservableRecipient, IViewModel
{
    protected BindingContext BindingContext { get; } = new BindingContext();


    public void Destroy()
    {
        throw new NotImplementedException();
    }
}