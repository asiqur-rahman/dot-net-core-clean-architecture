using Project.Common.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.Common.Extensions
{

    public static class QueryableExtensions
    {
        public static ProjectionExpression<TSource> NewModel<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
        //public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
        //    this IQueryable<TOuter> outer,
        //    IQueryable<TInner> inner,
        //    Expression<Func<TOuter, TKey>> outerKeySelector,
        //    Expression<Func<TInner, TKey>> innerKeySelector,
        //    Expression<Func<TOuter, TInner, TResult>> result)
        //{

        //    return outer.GroupJoin(
        //            inner,
        //            outerKeySelector,
        //            innerKeySelector,
        //            (a, b) => new { a, b }).AsExpandable()
        //        .SelectMany(
        //            z => z.b.DefaultIfEmpty(),
        //            (z, b) => result.Invoke(z.a, b));
        //}
        public static IOrderedQueryable<T> GenericEvaluateOrderBy<T>(this IQueryable<T> query, string propertyName)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            var propertyReference = Expression.Property(parameter, propertyName);
            var sortExpression = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                new Type[] { type },
                null,
                Expression.Lambda<Func<T, bool>>(propertyReference, new[] { parameter }));

            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(sortExpression);
        }
        public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
        public static Expression<Func<T, bool>> ConstructAndExpressionTree<T>(List<ExpressionFilter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = ExpressionRetriever.GetExpression<T>(param, filters[0]);
            }
            else
            {
                exp = ExpressionRetriever.GetExpression<T>(param, filters[0]);
                for (int i = 1; i < filters.Count; i++)
                {
                    //exp = Expression.And(exp, ExpressionRetriever.GetExpression<T>(param, filters[i]));
                    exp = Expression.Or(exp, ExpressionRetriever.GetExpression<T>(param, filters[i]));
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }
    }

    public class ProjectionExpression<TSource>
    {
        private static readonly Dictionary<string, Expression> ExpressionCache = new Dictionary<string, Expression>();

        private readonly IQueryable<TSource> _source;

        public ProjectionExpression(IQueryable<TSource> source)
        {
            _source = source;
        }

        public IQueryable<TDest> To<TDest>()
        {
            var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();

            return _source.Select(queryExpression);
        }

        private static Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            var key = GetCacheKey<TDest>();

            return ExpressionCache.ContainsKey(key) ? ExpressionCache[key] as Expression<Func<TSource, TDest>> : null;
        }

        private static Expression<Func<TSource, TDest>> BuildExpression<TDest>()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(TSource), "src");

            var bindings = destinationProperties
                                .Select(destinationProperty => BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                                .Where(binding => binding != null);

            var expression = Expression.Lambda<Func<TSource, TDest>>(Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

            var key = GetCacheKey<TDest>();

            ExpressionCache.Add(key, expression);

            return expression;
        }

        private static MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty, IEnumerable<PropertyInfo> sourceProperties)
        {
            IEnumerable<PropertyInfo> propertyInfos = sourceProperties as PropertyInfo[] ?? sourceProperties.ToArray();
            var sourceProperty = propertyInfos.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty != null)
            {
                return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
            }

            var propertyNames = SplitCamelCase(destinationProperty.Name);

            if (propertyNames.Length == 2)
            {
                sourceProperty = propertyInfos.FirstOrDefault(src => src.Name == propertyNames[0]);

                if (sourceProperty != null)
                {
                    var sourceChildProperty = sourceProperty.PropertyType.GetProperties().FirstOrDefault(src => src.Name == propertyNames[1]);

                    if (sourceChildProperty != null)
                    {
                        return Expression.Bind(destinationProperty, Expression.Property(Expression.Property(parameterExpression, sourceProperty), sourceChildProperty));
                    }
                }
            }

            return null;
        }

        private static string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }

        private static string[] SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim().Split(' ');
        }
    }

    // CONSTRUCT DYNAMIC FILTERS 
    public static class ExpressionRetriever
    {
        public static Expression GetExpression<T>(ParameterExpression param, ExpressionFilter filter)
        {
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);

            //var propertyType = ((PropertyInfo)member.Member).PropertyType;
            //var converter = TypeDescriptor.GetConverter(propertyType); // 1
            //if (!converter.CanConvertFrom(typeof(string))) // 2
            //    throw new NotSupportedException();

            //var propertyValue = converter.ConvertFromInvariantString(filter.Value); // 3
            //var constant = Expression.Constant(propertyValue);
            //var valueExpression = Expression.Convert(constant, propertyType); // 4

            //var convertedExpression = Expression.Call(Expression.Convert(member, typeof(object)),typeof(object).GetMethod("ToString"));

            //var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(TypeCode) });
            //var callExpressionReturningObject = Expression.Call(changeTypeMethod, member, Expression.Constant(TypeCode.String));

            switch (filter.Comparison)
            {
                case Comparison.Equal:
                    return Expression.Equal(member, constant);
                case Comparison.GreaterThan:
                    return Expression.GreaterThan(member, constant);
                case Comparison.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);
                case Comparison.LessThan:
                    return Expression.LessThan(member, constant);
                case Comparison.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);
                case Comparison.NotEqual:
                    return Expression.NotEqual(member, constant);
                case Comparison.Contains:
                    return Expression.Call(member, containsMethod, constant);
                case Comparison.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);
                case Comparison.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
                default:
                    return null;
            }
        }
        public static GridData<T> ToGridDataList<T>(this IQueryable<T> superset, Expression<Func<T, bool>> predicate, List<ExpressionFilter> filters, string sort, string order, int pageNumber, int pageSize)
        {
            var dataList = new GridData<T>
            {
                Total = 0,
                Rows = new List<T>()
            };
            try
            {
                var expressionTree = QueryableExtensions.ConstructAndExpressionTree<T>(filters);
                var combineExpression = filters.Count > 0 ? predicate.And(expressionTree) : predicate ?? (arg => true);
                var data = (IQueryable<T>)superset.Where(combineExpression).ApplyOrder(sort, order);

                if (data != null && Queryable.Any<T>(data))
                {
                    dataList.Total = Queryable.Count<T>(data);
                    dataList.Rows = pageNumber == 1 ?
                        Queryable.Skip<T>(data, 0).Take<T>(pageSize).ToList<T>() :
                        Queryable.Skip<T>(data, (pageNumber - 1) * pageSize).Take<T>(pageSize).ToList<T>();
                    dataList.FirstItemOnPage = (pageNumber - 1) * pageSize + 1;
                }
            }
            catch (Exception e)
            {
                // e
            }


            return dataList;
        }
    }

    //var filters = new List<ExpressionFilter>
    //{
    //new ExpressionFilter
    //{
    //    PropertyName="CountryOfOrigin",
    //    Comparison=Comparison.StartsWith,
    //    Value="Fr"
    //},
    //new ExpressionFilter
    //{
    //    PropertyName="MaxMilesPerHour",
    //    Comparison=Comparison.GreaterThanOrEqual,
    //    Value=190
    //}
    //};

    //var expressionTree = ExpressionBuilderHelper.ExpressionBuilder.ConstructAndExpressionTree<Car>(filters);
    //var anonymousFunc = expressionTree.Compile();
    //var result = cars.Where(anonymousFunc);
}
