using System.ComponentModel;
using System.Linq.Expressions;
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