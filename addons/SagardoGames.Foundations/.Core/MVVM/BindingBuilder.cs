using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Core.Observable;

namespace Core.MVVM;

public struct BindingBuilder<TViewModel, TProp>
    where TViewModel : INotifyPropertyChanged
{
    private readonly TViewModel _viewModel;
    private readonly Expression<Func<TViewModel, TProp>> _propExpr;
    
    private Action<TProp>? _onChanged;

    public BindingBuilder(
        ref TViewModel viewModel, 
        Expression<Func<TViewModel, TProp>> propExpr)
    {
        _viewModel = viewModel;
        _propExpr = propExpr;
    }

    public BindingBuilder<TViewModel, TProp> WithMethodConverter(Action<TProp> onChanged)
    {
        _onChanged = onChanged;
        return this;
    }
    
    public void AddTo(IBindingContextHolder holder)
    {
        var action = _onChanged;
        var viewModel = _viewModel;
        
        if (action is null)
            throw new Exception(""); //TODO: What's the exception exactly?
        
        if (_propExpr.Body is not MemberExpression { Member: PropertyInfo propertyInfo })
            throw new ArgumentException("Expression must be a simple property access", nameof(_propExpr));
        
        var name = propertyInfo.Name;
        
        action.Invoke((TProp)propertyInfo.GetValue(_viewModel)!);

        viewModel.PropertyChanged += OnPropertyChanged;

        var binding = Binding.Create(() => viewModel.PropertyChanged -= OnPropertyChanged);
        holder.BindingContext.AddBinding(binding);
        
        return;

        void OnPropertyChanged(object? _, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != name)
                return;
            
            action.Invoke((TProp)propertyInfo.GetValue(viewModel)!);
        }
    }
}