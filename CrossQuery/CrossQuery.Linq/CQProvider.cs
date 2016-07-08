﻿using System;
using System.Collections;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using CrossQuery.Interfaces;
using CrossQuery.Linq.Interfaces;

namespace CrossQuery.Linq
{
    public class CQProvider : BaseCQProvider
    {
        private IDataAdapter[] _dataAdapters;
        private Mapper.Mapper _mapper;

        public CQProvider(Mapper.Mapper mapper, params IDataAdapter[] dataAdapters)
        {
            _mapper = mapper;
            _dataAdapters = dataAdapters;
        }

        public override object Execute(Expression expression)
        {
            object returnedObject = null;

            foreach (var dataAdapter in _dataAdapters)
            {
                var query = new QueryBuilder(dataAdapter, expression).Build();

                if (query == null)
                    continue;

                var collection = typeof(IDataAdapter)
                       .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                       .First(m => m.Name == "GetEntities")
                       .MakeGenericMethod(new Type[]
                       {
                            query.EntityType
                       })
                       .Invoke(dataAdapter, null);

                var queryExecuteResult = query.Execute((IQueryable)collection);
                var queryExecuteResultType = queryExecuteResult.GetType();

                if (typeof(IEnumerable).IsAssignableFrom(queryExecuteResultType))
                    queryExecuteResultType = queryExecuteResultType.GetGenericArguments()[0];

                if (!queryExecuteResultType.IsClass || queryExecuteResultType == typeof(string))
                {
                    returnedObject = queryExecuteResult;
                    continue;
                }

                if (typeof(DynamicClass).IsAssignableFrom(queryExecuteResultType))
                {
                    returnedObject = queryExecuteResult;
                    continue;
                }

                var mapMethod = typeof(Mapper.Mapper).GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .First(m => m.Name == "Map" && m.ReturnType == typeof(object));

                var destinationType = expression.Type;

                if (typeof(IEnumerable).IsAssignableFrom(destinationType))
                    destinationType = destinationType.GetGenericArguments()[0];

                returnedObject = mapMethod.Invoke(_mapper, new object[] { query.EntityType, destinationType, queryExecuteResult });
            }

            return returnedObject;
        }
    }
}
