using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CrossQuery.Interfaces;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq
{
    public class CQProvider : BaseCQProvider
    {
        private IDataAdapter[] _dataAdapters;

        public CQProvider(params IDataAdapter[] dataAdapters)
        {
            _dataAdapters = dataAdapters;
        }

        public override object Execute(Expression expression)
        {
            object returnedObject = null;

            foreach(var dataAdapter in _dataAdapters)
            {
                var query = new QueryBuilder(dataAdapter, expression).Build();

                if (query != null)
                {                    
                    var getEntitiesMethod = typeof(IDataAdapter)
                        .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                        .First(m => m.Name == "GetEntities")
                        .MakeGenericMethod(new Type[]
                        {
                            query.EntityType
                        });

                    var dbSet = getEntitiesMethod.Invoke(dataAdapter, null);

                    var linqMethod = typeof(Queryable)
                        .GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .First(m => m.Name == query.MethodName && m.GetParameters().Count() == 2);

                    var linqResult = linqMethod.MakeGenericMethod(query.EntityType)
                        .Invoke(dbSet, new[] { dbSet, query.GetLambdaExpression() });

                    var mapMethods = typeof(Mapper.Mapper).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Where(m => m.Name == "Map");

                    MethodInfo mapMethod = null;
                    Type destinationType = null;

                    if (typeof(IEnumerable).IsAssignableFrom(linqResult.GetType()))
                    {
                        mapMethod = mapMethods
                            .First(m => typeof(IEnumerable)
                            .IsAssignableFrom(m.ReturnType));

                        destinationType = expression.Type.GetGenericArguments()[0];
                    }
                    else
                    {
                        mapMethod = mapMethods
                            .First(m => !typeof(IEnumerable)
                            .IsAssignableFrom(m.ReturnType));

                        destinationType = expression.Type;
                    }

                    returnedObject = mapMethod.MakeGenericMethod(new[] { query.EntityType, destinationType }).Invoke(null, new[] { linqResult });
                }
            }

            return returnedObject;
        }
    }
}
