using System.Collections.Concurrent;
using System.Reflection;
using Core.DependencyInjection.Exceptions;

namespace Core.DependencyInjection.ServiceResolver
{
    public sealed class ServiceResolversLoader
    {
        private readonly ConcurrentDictionary<MemberTypes, IMemberResolver> _data = new();

        public void RegisterServiceResolver(MemberTypes memberType, IMemberResolver memberResolver)
        {
            if (!_data.TryAdd(memberType, memberResolver))
            {
                throw new DependencyInjectionException($"MemberType {memberType} has already been registered before.");
            }
        }

        public IMemberResolver GetServiceResolver(MemberTypes memberType)
        {
            if (!_data.TryGetValue(memberType, out var resolver))
            {
                throw new DependencyInjectionException($"MemberType {memberType} needs to be registered before trying to get it.");
            }

            return resolver;
        }

        public void Clear()
        {
            _data?.Clear();
        }
    }
}
