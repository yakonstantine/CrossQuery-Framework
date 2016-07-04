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
                    var linqResult = GetLinqQueryResult(dataAdapter, expression, query);

                    var resultType = linqResult.GetType();
                    var isIEnumerable = false;

                    if (typeof(IEnumerable).IsAssignableFrom(resultType))
                    {
                        resultType = resultType.GetGenericArguments()[0];
                        isIEnumerable = true;
                    }

                    if (!resultType.IsClass || resultType == typeof(string))
                    {
                        returnedObject = linqResult;
                        continue;
                    }

                    var mapMethods = typeof(Mapper.Mapper).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Where(m => m.Name == "Map");

                    MethodInfo mapMethod = null;
                    Type destinationType = null;

                    if (isIEnumerable)
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

                    returnedObject = mapMethod
                        .MakeGenericMethod(new[] { query.EntityType, destinationType })
                        .Invoke(null, new[] { linqResult });
                }
            }

            return returnedObject;
        }

        private object GetLinqQueryResult(IDataAdapter adapter, Expression expression, Query query)
        {
            var getEntitiesMethod = typeof(IDataAdapter)
                       .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                       .First(m => m.Name == "GetEntities")
                       .MakeGenericMethod(new Type[]
                       {
                            query.EntityType
                       });

            var dbSet = getEntitiesMethod.Invoke(adapter, null);

            var linqMethod = typeof(Queryable)
                        .GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .First(m => m.Name == query.MethodName && m.GetParameters().Count() == 2);

            switch (query.MethodName)
            {
                case "Select":
                    {
                        return linqMethod
                            .MakeGenericMethod(query.EntityType, expression.Type.GetGenericArguments()[0])
                            .Invoke(dbSet, new[] { dbSet, query.GetLambdaExpression() });
                    }
                default:
                    {
                        return linqMethod
                            .MakeGenericMethod(query.EntityType)
                            .Invoke(dbSet, new[] { dbSet, query.GetLambdaExpression() });
                    }
            }
        }
    }
}
