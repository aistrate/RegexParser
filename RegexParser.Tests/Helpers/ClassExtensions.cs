using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ParserCombinators.Util;

namespace RegexParser.Tests.Helpers
{
    public static class ClassExtensions
    {
        #region Zero-Parameter Constructor

        public static Func<T> GetConstructor<T>()
            where T : class
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(typeof(T), new Type[] { });

            return () => constructorInfo.Invoke(new object[] { }) as T;
        }


        public static T InvokeConstructor<T>()
            where T : class
        {
            return GetConstructor<T>()();
        }
        
        #endregion


        #region One-Parameter Constructor

        public static Func<A, T> GetConstructor<T, A>()
            where T : class
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(typeof(T), new Type[] { typeof(A) });

            return (A a) => constructorInfo.Invoke(new object[] { a }) as T;
        }

        public static T InvokeConstructor<T, A>(A a)
            where T : class
        {
            return GetConstructor<T, A>()(a);
        }

        #endregion


        #region Two-Parameter Constructor

        public static Func<A, B, T> GetConstructor<T, A, B>()
            where T : class
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(typeof(T), new Type[] { typeof(A), typeof(B) });

            return (A a, B b) => constructorInfo.Invoke(new object[] { a, b }) as T;
        }

        public static T InvokeConstructor<T, A, B>(A a, B b)
            where T : class
        {
            return GetConstructor<T, A, B>()(a, b);
        }

        #endregion


        #region Three-Parameter Constructor

        public static Func<A, B, C, T> GetConstructor<T, A, B, C>()
            where T : class
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(typeof(T), new Type[] { typeof(A), typeof(B), typeof(C) });

            return (A a, B b, C c) => constructorInfo.Invoke(new object[] { a, b, c }) as T;
        }

        public static T InvokeConstructor<T, A, B, C>(A a, B b, C c)
            where T : class
        {
            return GetConstructor<T, A, B, C>()(a, b, c);
        }

        #endregion


        #region Four-Parameter Constructor

        public static Func<A, B, C, D, T> GetConstructor<T, A, B, C, D>()
            where T : class
        {
            ConstructorInfo constructorInfo = GetConstructorInfo(typeof(T), new Type[] { typeof(A), typeof(B), typeof(C), typeof(D) });

            return (A a, B b, C c, D d) => constructorInfo.Invoke(new object[] { a, b, c, d }) as T;
        }

        public static T InvokeConstructor<T, A, B, C, D>(A a, B b, C c, D d)
            where T : class
        {
            return GetConstructor<T, A, B, C, D>()(a, b, c, d);
        }

        #endregion


        #region Non-Generic

        public static ConstructorInfo GetConstructorInfo(Type objType, Type[] paramTypes)
        {
            ConstructorInfo constructorInfo = objType.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                null,
                paramTypes,
                null);

            if (constructorInfo != null)
                return constructorInfo;
            else
            {
                string parameters = paramTypes.Length == 0 ?
                                        "with no parameters" :
                                        "with parameter types " + paramTypes.Select(t => "<" + t.FullName + ">")
                                                                            .ConcatStrings(", ");

                throw new ApplicationException(string.Format("Constructor not found for class <{0}> ({1}).",
                                                             objType.FullName,
                                                             parameters));
            }
        }

        #endregion
    }
}
