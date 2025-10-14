using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Core.Observable;
using JetBrains.Annotations;

namespace Core.MVVM;

[PublicAPI]
public static class ViewModelExtension
{
    
    public static BindingBuilder<TViewModel, TProp> BindTo<TViewModel, TProp>(
        this TViewModel viewModel,
        Expression<Func<TViewModel, TProp>> propExpr)
        where TViewModel : INotifyPropertyChanged
    {
        var builder = new BindingBuilder<TViewModel, TProp>(ref viewModel, propExpr);
        return builder;
    }
}

public sealed class BindingBuilder<TViewModel, TProp>
    where TViewModel : INotifyPropertyChanged
{
    private readonly TViewModel _viewModel;
    private readonly Expression<Func<TViewModel, TProp>> _propExpr;
    
    private Action<TProp>? _onChanged;

    public BindingBuilder(ref TViewModel viewModel, Expression<Func<TViewModel, TProp>> propExpr)
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
        if (_onChanged is null)
            throw new Exception("");
        
        if (_propExpr.Body is not MemberExpression { Member: PropertyInfo propertyInfo })
            throw new ArgumentException("Expression must be a simple property access", nameof(_propExpr));
        
        var name = propertyInfo.Name;
        
        _onChanged.Invoke((TProp)propertyInfo.GetValue(_viewModel)!);

        PropertyChangedEventHandler handler = (_, e) =>
        {
            if (e.PropertyName != name)
                return;
            
            _onChanged.Invoke((TProp)propertyInfo.GetValue(_viewModel)!);
        };

        _viewModel.PropertyChanged += handler;

        var binding = Binding.Create(() => _viewModel.PropertyChanged -= handler);
        holder.BindingContext.AddBinding(binding);
    }
}