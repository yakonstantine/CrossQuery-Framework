using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Mapper.Interfaces
{
    public interface IMapperConfiguration
    {
        Type GetDestinationType();
        Type GetSourceType();
    }

    public interface IMapperConfiguration<TSource, TDest> : IMapperConfiguration
    {
    }
}
