using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Refl
{
    public static class Utils
    {
        public static Func<TClass, TResult> CreateCompiledGetter<TClass, TResult>(string path)
        {
            string[] props = path.Split('.');

            var obj = Expression.Parameter(typeof(TClass), "obj");
            Expression expressionTree = obj;
            Type currType = typeof(TClass);
            for (int i = 0; i < props.Length; i++)
            {
                string prop = props[i];
                var currProp = currType.GetProperty(prop);
                currType = currProp.PropertyType;
                expressionTree = Expression.Property(expressionTree, currProp);
            }
            expressionTree = Expression.Convert(expressionTree, typeof(TResult));
            var lambda = Expression.Lambda(typeof(Func<TClass, TResult>), expressionTree, obj).Compile();
            return (Func<TClass, TResult>)lambda;
        }
    }
}
