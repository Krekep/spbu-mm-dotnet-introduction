using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refl
{
    internal static class Utils
    {
        public static Func<string, TResult> GenerateGetFuncForClass<TClass, TResult>(TClass inClass)
        {
            Func<string, TResult> result = path => Getter<TClass, TResult>(path, inClass);
            return result;
        }

        public static Action<string, TResult> GenerateSetFuncForClass<TClass, TResult>(TClass inClass)
        {
            Action<string, TResult> result = (path, value) => Setter(path, inClass, value);
            return result;
        }

        public static Func<TClass, TResult> GenerateGetFuncByPath<TClass, TResult>(string path)
        {
            Func<TClass, TResult> result = inClass => Getter<TClass, TResult>(path, inClass);
            return result;
        }

        public static Action<TClass, TResult> GenerateSetFuncByPath<TClass, TResult>(string path)
        {
            Action<TClass, TResult> result = (inClass, value) => Setter(path, inClass, value);
            return result;
        }

        private static object GetObjectAt(int index, string[] properties, object obj)
        {
            object currObj = obj;
            for (int i = 0; i < properties.Length && i < index; i++)
            {
                string prop = properties[i];
                Type currType = currObj.GetType();
                var currProp = currType.GetProperty(prop);
                currObj = currProp.GetValue(currObj);
            }
            return currObj;
        }

        private static void Setter<TClass, TResult>(string path, TClass inClass, TResult value)
        {
            string[] props = path.Split('.');

            object obj = GetObjectAt(props.Length - 1, props, inClass);
            var targetProp = obj.GetType().GetProperty(props[^1]);
            targetProp.SetValue(obj, value);
        }

        private static TResult Getter<TClass, TResult>(string path, TClass inClass)
        {
            string[] props = path.Split('.');

            object obj = GetObjectAt(props.Length, props, inClass);
            return (TResult)obj;
        }
    }
}
