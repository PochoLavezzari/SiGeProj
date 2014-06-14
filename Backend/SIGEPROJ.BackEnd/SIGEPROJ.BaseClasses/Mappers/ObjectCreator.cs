using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SIGEPROJ.BaseClasses.Reflection;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Clase <see cref="ObjectCreator"/>
    /// </summary>
    public static class ObjectCreator
    {
        // TODO: Agregar estrategias de creación de objetos
        /// <summary>
        /// Creates el type especificado.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
          public static object Create(Type type)
          {
              if (type.IsValueType)
              {
                  return CreateObject(type);
              }

              if (type == typeof(string))
                  return string.Empty;

              if(type.IsArray)
              {
                  return CreateArray(type.GetElementType(), 0);
              }

              if (type.IsEnumerable())
              {
                  if (type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type))
                  {
                      var args = type.GetGenericArguments();
                      return CreateDictionary(type, args[0], args[1]);
                  }
                  return CreateList(type.GetGenericArguments()[0]);
              }

              if (type.IsInterface)
                  throw new Exception("don't know any implementation of this type: " + type.Name);
              return Activator.CreateInstance(type);
          }

          /// <summary>
          /// Creates the default value.
          /// </summary>
          /// <param name="type">The type.</param>
          /// <returns></returns>
          public static object CreateDefaultValue(Type type)
          {
              return (type.IsValueType ? CreateObject(type) : null);
          }

          /// <summary>
          /// Creates the non null value.
          /// </summary>
          /// <param name="type">The type.</param>
          /// <returns></returns>
          public static object CreateNonNullValue(Type type)
          {
              if (type.IsValueType)
              {
                  return CreateObject(type);
              }
              if (type == typeof(string))
              {
                  return string.Empty;
              }
              return Activator.CreateInstance(type);
          }

          /// <summary>
          /// Creates the object.
          /// </summary>
          /// <param name="type">The type.</param>
          /// <returns></returns>
          public static object CreateObject(Type type)
          {
              // TODO: return DelegateFactory.CreateCtor(type)();
              return Activator.CreateInstance(type);
          }

          /// <summary>
          /// Creates the list.
          /// </summary>
          /// <param name="elementType">Type of the element.</param>
          /// <returns></returns>
          public static IList CreateList(Type elementType)
          {
              return (IList)CreateObject(typeof(List<>).MakeGenericType(new [] { elementType }));
          }



          /// <summary>
          /// Creates the dictionary.
          /// </summary>
          /// <param name="dictionaryType">Type of the dictionary.</param>
          /// <param name="keyType">Type of the key.</param>
          /// <param name="valueType">Type of the value.</param>
          /// <returns></returns>
          public static object CreateDictionary(Type dictionaryType, Type keyType, Type valueType)
          {
              Type type = dictionaryType.IsInterface ? typeof(Dictionary<,>).MakeGenericType(new [] { keyType, valueType }) : dictionaryType;
              return CreateObject(type);
          }

          /// <summary>
          /// Creates the array.
          /// </summary>
          /// <param name="elementType">Type of the element.</param>
          /// <param name="length">The length.</param>
          /// <returns></returns>
          public static Array CreateArray(Type elementType, int length)
          {
              return Array.CreateInstance(elementType, length);
          }
    }
}