using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CrossQuery.Linq
{
    internal class Query
    {
        internal string Type { get; set; }

        internal Type EntityType { get; set; }

        internal LambdaExpression LambdaExpression { get; set; }
    }
}
