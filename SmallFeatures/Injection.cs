using System;
using System.Reflection;
using Exerussus._1Extensions.SmallFeatures;
using UnityEngine;

namespace Exerussus._1EasyEcs.Scripts.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InjectSharedObjectAttribute : Attribute
    {
        public Type MainType { get; }
        public Type SubType { get; }

        public InjectSharedObjectAttribute() { }

        public InjectSharedObjectAttribute(Type mainType)
        {
            MainType = mainType;
        }

        public InjectSharedObjectAttribute(Type mainType, Type subType)
        {
            MainType = mainType;
            SubType = subType;
        }
    }

    public static class SharedObjectInjector
    {
        public static void InjectSharedObjects(this GameShare gameShare, object injectingClass)
        {
            InjectSharedObjects(injectingClass, gameShare);
        }

        public static void InjectSharedObjects(object target, GameShare gameShare)
        {
            var targetType = target.GetType();
    
            while (targetType != null && targetType != typeof(object))
            {
                var fields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var properties = targetType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                foreach (var field in fields)
                {
                    ProcessInjection(field, target, gameShare, 
                        f => f.FieldType, 
                        sharedObject => field.SetValue(target, sharedObject));
                }

                foreach (var property in properties)
                {
                    ProcessInjection(property, target, gameShare, 
                        p => p.PropertyType, 
                        sharedObject => property.SetValue(target, sharedObject));
                }

                targetType = targetType.BaseType;
            }
        }

        private static void ProcessInjection<T>(
            T member,
            object target,
            GameShare gameShare,
            Func<T, Type> getType,
            Action<object> setValue) where T : MemberInfo
        {
#if UNITY_EDITOR
            try
            {
                var attribute = Attribute.GetCustomAttribute(member, typeof(InjectSharedObjectAttribute)) as InjectSharedObjectAttribute;
                if (attribute == null) return;

                var memberType = getType(member);
                var sharedObject = GetSharedObject(gameShare, memberType, attribute);
                setValue(sharedObject);
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка при инъекции {member.Name} в {target.GetType().Name}:\n\n{e.Message}\n{e.StackTrace}");
            }
#else

            var attribute = Attribute.GetCustomAttribute(member, typeof(InjectSharedObjectAttribute)) as InjectSharedObjectAttribute;
            if (attribute == null) return;

            var memberType = getType(member);
            var sharedObject = GetSharedObject(gameShare, memberType, attribute);
            setValue(sharedObject);
#endif
        }

        private static object GetSharedObject(GameShare gameShare, Type memberType, InjectSharedObjectAttribute attribute)
        {
            MethodInfo method;
            if (attribute.MainType == null)
            {
                method = typeof(GameShare).GetMethod("InjectSharedObject", BindingFlags.NonPublic | BindingFlags.Instance);
                var genericMethod = method.MakeGenericMethod(memberType);
                return genericMethod.Invoke(gameShare, new object[] { memberType });
            }

            if (attribute.SubType == null)
            {
                method = typeof(GameShare).GetMethod("InjectSharedObject", BindingFlags.NonPublic | BindingFlags.Instance);
                var genericMethod = method.MakeGenericMethod(memberType);
                return genericMethod.Invoke(gameShare, new object[] { attribute.MainType });
            }

            method = typeof(GameShare).GetMethod("InjectSharedSubTypeObject", BindingFlags.NonPublic | BindingFlags.Instance);
            var genericMethodWithSub = method.MakeGenericMethod(memberType);
            return genericMethodWithSub.Invoke(gameShare, new object[] { attribute.MainType, attribute.SubType });
        }
    }
}
