using MassTransit;

namespace Shared.EntityNameFormatter
{
    public class CustomEntityNameFormatter : IEntityNameFormatter
    {
        private readonly string _env;

        public CustomEntityNameFormatter(string env)
        {
            _env = env;
        }

        public string FormatEntityName<T>()
        {
            return $"{_env}_{typeof(T).Name}".ToUpper();
        }
    }
}
