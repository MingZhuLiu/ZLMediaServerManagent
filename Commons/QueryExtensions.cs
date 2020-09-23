using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Commons
{
    public static class QueryExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException("SortBy Error!Source Canot be null!");
            }

            string sortDirection = String.Empty;
            string propertyName = String.Empty;

            sortExpression = sortExpression.Trim();
            int spaceIndex = sortExpression.Trim().IndexOf(" ");
            if (spaceIndex < 0)
            {
                propertyName = sortExpression;
                sortDirection = "ASC";
            }
            else
            {
                propertyName = sortExpression.Substring(0, spaceIndex);
                sortDirection = sortExpression.Substring(spaceIndex + 1).Trim();
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (sortDirection == "ASC") ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }


        /// <summary>
        /// 将列表转换为树形结构
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">数据</param>
        /// <param name="rootwhere">根条件</param>
        /// <param name="childswhere">节点条件</param>
        /// <param name="addchilds">添加子节点</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static List<T> ToTree<T>(this List<T> list, Func<T, T, bool> rootwhere, Func<T, T, bool> childswhere, Action<T, IEnumerable<T>> addchilds, T entity = default(T))
        {
            var treelist = new List<T>();
            //空树
            if (list == null || list.Count == 0)
            {
                return treelist;
            }
            if (!list.Any<T>(e => rootwhere(entity, e)))
            {
                return treelist;
            }

            //树根
            if (list.Any<T>(e => rootwhere(entity, e)))
            {
                treelist.AddRange(list.Where(e => rootwhere(entity, e)));
            }

            //树叶
            foreach (var item in treelist)
            {
                if (list.Any(e => childswhere(item, e)))
                {
                    var nodedata = list.Where(e => childswhere(item, e)).ToList();
                    foreach (var child in nodedata)
                    {
                        //添加子集
                        var data = list.ToTree(childswhere, childswhere, addchilds, child);
                        addchilds(child, data);
                    }
                    addchilds(item, nodedata);
                }
            }

            return treelist;
        }



        /// <summary>
        /// 根据指定属性名称对序列进行排序
        /// </summary>
        /// <typeparam name="TSource">source中的元素的类型</typeparam>
        /// <param name="source">一个要排序的值序列</param>
        /// <param name="property">属性名称</param>
        /// <param name="descending">是否降序</param>
        /// <returns></returns>
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string property, bool descending) where TSource : class
        {
            ParameterExpression param = Expression.Parameter(typeof(TSource), "c");
            PropertyInfo pi = typeof(TSource).GetProperty(property);
            MemberExpression selector = Expression.MakeMemberAccess(param, pi);
            LambdaExpression le = Expression.Lambda(selector, param);
            string methodName = (descending) ? "OrderByDescending" : "OrderBy";
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TSource), pi.PropertyType }, source.Expression, le);
            return source.Provider.CreateQuery<TSource>(resultExp);
        }








    }

}