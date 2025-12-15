using System.Data;
using System.Globalization;
using Dapper;

namespace MyCompany.Microservice.Infrastructure.Mappers
{
    /// <inheritdoc />
    public class GuidAsStringHandler : SqlMapper.TypeHandler<Guid>
    {
        /// <inheritdoc />
        public override Guid Parse(object value)
        {
            return new Guid((string)value);
        }

        /// <inheritdoc />
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            ArgumentNullException.ThrowIfNull(parameter);
            parameter.Value = value.ToString().ToUpper(CultureInfo.InvariantCulture);
        }
    }
}
