using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            var candidates = new Queue<Expression>();
            candidates.Enqueue(expression);

            while (candidates.Count > 0)
            {
                var expr = candidates.Dequeue();

                if (expr is MethodCallExpression)
                {
                    foreach (var argument in ((MethodCallExpression)expr).Arguments)
                        candidates.Enqueue(argument);

                    continue;
                }

                if (expr is UnaryExpression)
                {
                    candidates.Enqueue(((UnaryExpression)expr).Operand);
                    continue;
                }

                if (expr is BinaryExpression)
                {
                    var binaryExpression = (BinaryExpression)expr;

                    candidates.Enqueue(binaryExpression.Left);
                    candidates.Enqueue(binaryExpression.Right);

                    continue;
                }                

                if (expr is LambdaExpression)
                {
                    candidates.Enqueue(((LambdaExpression)expr).Body);
                    continue;
                }
            }

            throw new NotImplementedException();
        }
    }
}
