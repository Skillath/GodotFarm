using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Core.Observable;
using JetBrains.Annotations;

namespace Core.MVVM;

[PublicAPI]
public static class ViewModelExtension
{
    public static Binding BindToProperty<TViewModel, TProp>(
        this TViewModel viewModel,
        Expression<Func<TViewModel, TProp>> propExpr,
        Action<TProp> onChanged)
        where TViewModel : INotifyPropertyChanged
    {
        if (propExpr.Body is not MemberExpression { Member: PropertyInfo propertyInfo })
            throw new ArgumentException("Expression must be a simple property access", nameof(propExpr));

        var name = propertyInfo.Name;
        
        onChanged.Invoke((TProp)propertyInfo.GetValue(viewModel)!);

        PropertyChangedEventHandler handler = (_, e) =>
        {
            if (e.PropertyName != name)
                return;
            
            onChanged.Invoke((TProp)propertyInfo.GetValue(viewModel)!);
        };

        viewModel.PropertyChanged += handler;
        return Binding.Create(() => viewModel.PropertyChanged -= handler);
    }
}