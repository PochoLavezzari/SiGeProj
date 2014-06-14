using System;
using System.Collections.Generic;
using SIGEPROJ.BaseClasses.Mappers.TypeMapperStrategies;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Clase principal desde la cual se pueden mapear 2 objetos.
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// All Registered Member Injections
        /// </summary>
        private static readonly Dictionary<string, IMemberInjection> RegisteredInjections = new Dictionary<string, IMemberInjection>();

        private static readonly object addInjectionLock = new object();

        /// <summary>
        /// Inicializa la clase <see cref="Mapper"/>.
        /// </summary>
        static Mapper()
        {
            // Carga de Converters base
            CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            CreateMap<string, long>().ConvertUsing(Convert.ToInt64);
            CreateMap<string, float>().ConvertUsing(Convert.ToSingle);
            CreateMap<string, bool>().ConvertUsing(Convert.ToBoolean);
            CreateMap<string, byte>().ConvertUsing(Convert.ToByte);
            CreateMap<string, char>().ConvertUsing(Convert.ToChar);
            CreateMap<string, DateTime>().ConvertUsing(Convert.ToDateTime);
            CreateMap<string, decimal>().ConvertUsing(Convert.ToDecimal);
            CreateMap<string, double>().ConvertUsing(Convert.ToDouble);
            CreateMap<string, short>().ConvertUsing(Convert.ToInt16);
            CreateMap<string, sbyte>().ConvertUsing(Convert.ToSByte);
            CreateMap<string, ushort>().ConvertUsing(Convert.ToUInt16);
            CreateMap<string, uint>().ConvertUsing(Convert.ToUInt32);
            CreateMap<string, ulong>().ConvertUsing(Convert.ToUInt64);
        }

        /// <summary>
        /// Creates a Member Injection for the give Source and Target Type
        /// </summary>
        /// <typeparam name="TSource">Source Type</typeparam>
        /// <typeparam name="TTarget">Target Type</typeparam>
        /// <returns></returns>
        public static MemberInjection<TSource, TTarget> CreateMap<TSource, TTarget>()
        {
            string mappingKey = GetKey<TSource, TTarget>();
            var injection = new MemberInjection<TSource, TTarget>();
            // Siempre bloquea al insertar el injection
            lock (addInjectionLock)
            {
                RegisteredInjections[mappingKey] = injection;
            }
            return injection;
        }

        ///// <summary>
        ///// Creates a Member Injection for the give Source and Target Type
        ///// </summary>
        ///// <returns></returns>
        //public static MemberInjection CreateMap(Type sourceType, Type targetType)
        //{
        //    string mappingKey = GetKey(sourceType, targetType);
        //    if(sourceType.IsGenericTypeDefinition && targetType.IsGenericTypeDefinition)
        //    {
                
        //    }
        //    var injection = new MemberInjection();
        //    RegisteredInjections[mappingKey] = injection;
        //    return injection;
        //}


        /// <summary>
        /// Maps the source object and returns the Target object
        /// </summary>
        /// <typeparam name="TSource">Source Type</typeparam>
        /// <typeparam name="TTarget">Target Type</typeparam>
        /// <param name="source">source object to map</param>
        /// <returns>target object</returns>
        public static TTarget Map<TSource, TTarget>(TSource source)
        {
            string mappingKey = GetKey<TSource, TTarget>();

            TTarget target;
            IMemberInjection injection;
            // Si encuentra el injection, usa
            if (RegisteredInjections.TryGetValue(mappingKey, out injection))
            {
                var memberInjection = injection as MemberInjection<TSource, TTarget>;
                if(memberInjection != null && memberInjection.HasCustomConstructor())
                {
                    target = memberInjection.Construct(source);
                    //return (TTarget)target.InjectFrom(injection, source);
                    return (TTarget)injection.Map(source, target);
                }
            }
            return Map(source, (TTarget)ObjectCreator.Create(typeof(TTarget)));
        }

        /// <summary>
        /// Maps the source object to the given Target object and returns the enriched Target
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>target object</returns>
        public static TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            string mappingKey = GetKey<TSource, TTarget>();

            IMemberInjection injection;
            if (RegisteredInjections.TryGetValue(mappingKey, out injection))
                //return (TTarget)target.InjectFrom(injection, source);
                return (TTarget)injection.Map(source, target);

            // Se bloquea para evitar que se cree un mapping identico
            lock (addInjectionLock)
            {
                // Vuelve a preguntar si existe, por si se hubiese agregado en un momento anterior
                if (RegisteredInjections.TryGetValue(mappingKey, out injection))
                    //return (TTarget)target.InjectFrom(injection, source);
                    return (TTarget)injection.Map(source, target);

                Type sourceType = typeof (TSource);
                Type targetType = typeof (TTarget);

                var strategyConventionInfo = 
                    new StrategyConventionInfo(sourceType, targetType);

                // Recorre las estrategias y si alguna coincide, la utiliza
                foreach (var strategyMapper in StrategyMapers)
                {
                    // Si alguna de las estrategias, se corresponde, entonces se utiliza
                    //if (typeMapperStrategy.Match(sourceType, targetType))
                    if (strategyMapper.Match(strategyConventionInfo))
                    {
                        injection = strategyMapper.GetInjecter(strategyConventionInfo);
                        // Verifica que la configuración sea válida
                        injection.AssertConfigurationIsValid();
                        // Guarda el ValueInjecter para no tener que generarlo la próxima vez.
                        RegisteredInjections[mappingKey] = injection;
                        //return (TTarget) target.InjectFrom(injection, source);
                        return (TTarget) injection.Map(source, target);
                    }
                }

                // En caso de que no exista ninguna estrategia que se ajuste, utiliza un mapper por default
                // que mapea todas las propieades comunes
                injection = new MemberInjection<TSource, TTarget>()
                    .MapUnmappedProperties();
                RegisteredInjections[mappingKey] = injection;
            }
            //return (TTarget)target.InjectFrom(injection, source);
            return (TTarget) injection.Map(source, target);

            //throw new NotSupportedException(string.Format("No Injection found for {0} to {1}", typeof(TSource), typeof(TTarget)));
        }

        /// <summary>
        /// Maps the source object to the given Target object and returns the enriched Target
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="sourceType">The source Type.</param>
        /// <param name="targetType">The target Type.</param>
        /// <returns>target object</returns>
        internal static object Map(object source, object target, Type sourceType, Type targetType)
        {
            string mappingKey = GetKey(sourceType, targetType);

            IMemberInjection injection;
            if (RegisteredInjections.TryGetValue(mappingKey, out injection))
                //return target.InjectFrom(injection, source);
                return injection.Map(source, target);

            lock (addInjectionLock)
            {
                if (RegisteredInjections.TryGetValue(mappingKey, out injection))
                    //return target.InjectFrom(injection, source);
                    return injection.Map(source, target);

                var strategyConventionInfo =
                   new StrategyConventionInfo(sourceType, targetType);

                foreach (var strategyMapper in StrategyMapers)
                {
                    // Si alguna de las estrategias, se corresponde, entonces se utiliza
                    if (strategyMapper.Match(strategyConventionInfo))
                    {
                        injection = strategyMapper.GetInjecter(strategyConventionInfo);
                        // Verifica que la configuración sea válida
                        injection.AssertConfigurationIsValid();
                        RegisteredInjections[mappingKey] = injection;
                        //return target.InjectFrom(injection, source);
                        return injection.Map(source, target);
                    }
                }
            }

            throw new NotSupportedException(string.Format("No Injection found for {0} to {1}", sourceType, targetType));
        }

        /// <summary>
        /// Lista de estrategias a utilizar para poder mapear de un Type a otro Type
        /// </summary>
        private static readonly List<ITypeMapperStrategy> StrategyMapers = new List<ITypeMapperStrategy>
                                                                 {
                                                                     new ValueTypeTypeMapperStrategy(),
                                                                     new EnumToStringTypeMapperStrategy(),
                                                                     new EnumNullableToStringTypeMapperStrategy(),
                                                                     new StringToEnumTypeMapperStrategy(),
                                                                     new StringToEnumNullableTypeMapperStrategy(),
                                                                     new ToStringTypeMapperStrategy(),
                                                                     new TypeConverterTypeMapperStrategy(),
                                                                     new ArrayToArrayTypeMapperStrategy(),
                                                                     new EnumerableToArrayTypeMapperStrategy(),
                                                                     new DictionaryToDictionaryTypeMapperStrategy(),
                                                                     new ArrayToEnumerableTypeMapperStrategy(),
                                                                     new EnumerableTypeMapperStrategy(),
                                                                     new EnumToIntTypeMapperStrategy(),
                                                                     new EnumToIntNullableTypeMapperStrategy(),
                                                                     new IntNullableToEnumNullableTypeMapperStrategy(),
                                                                     new IntNullableToEnumTypeMapperStrategy(),
                                                                     new EnumNullableToIntNullableTypeMapperStrategy(),
                                                                     new EnumNullableToIntTypeMapperStrategy(),
                                                                     new IntToEnumTypeMapperStrategy(),
                                                                     new IntToEnumNullableTypeMapperStrategy(),
                                                                     new NormalToNullableTypeMapperStrategy(),
                                                                     new NullableToNormalTypeMapperStrategy(),
                                                                     new SameTypeTypeMapperStrategy()
                                                                 };

        /// <summary>
        /// Agrega una estrategia de mapeo
        /// </summary>
        /// <param name="typeMapperStrategy"></param>
        public static void AddStrategyMapper(ITypeMapperStrategy typeMapperStrategy)
        {
            StrategyMapers.Add(typeMapperStrategy);
        }

        /// <summary>
        /// Gets the injection for the given source and target type
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <returns></returns>
        public static MemberInjection<TSource, TTarget> GetInjection<TSource, TTarget>()
        {
            return (MemberInjection<TSource, TTarget>) GetInjection(typeof (TSource), typeof (TTarget));
        }

        /// <summary>
        /// Gets the injection for the given source and target type
        /// </summary>
        /// <returns></returns>
        public static IMemberInjection GetInjection(Type sourceType, Type targetType)
        {
            string mappingKey = GetKey(sourceType, targetType);

            IMemberInjection injection;
            if (RegisteredInjections.TryGetValue(mappingKey, out injection))
                return injection;

            // Si no existe el injection, se fija que siga alguna de las estrategias
            lock (addInjectionLock)
            {
                if (RegisteredInjections.TryGetValue(mappingKey, out injection))
                    return injection;


                var strategyConventionInfo =
                    new StrategyConventionInfo(sourceType, targetType);

                foreach (var strategyMapper in StrategyMapers)
                {
                    // Si alguna de las estrategias, se corresponde, entonces se utiliza
                    if (strategyMapper.Match(strategyConventionInfo))
                    {
                        injection = strategyMapper.GetInjecter(strategyConventionInfo);
                        // Verifica que la configuración sea válida
                        injection.AssertConfigurationIsValid();
                        RegisteredInjections[mappingKey] = injection;
                        return injection;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Assert that the configuration is valid.
        /// </summary>
        public static void AssertConfigurationIsValid()
        {
            foreach (var value in RegisteredInjections.Values)
                value.AssertConfigurationIsValid();
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <returns></returns>
        private static string GetKey<TSource, TTarget>()
        {
            return GetKey(typeof (TSource), typeof (TTarget));
        }

        /// <summary>
        /// Obtiene la representación de la clave de un Mapeo entre Source/Target
        /// </summary>
        /// <returns></returns>
        private static string GetKey(Type sourceType, Type targetType)
        {
            return sourceType + ":" + targetType;
        } 
    }
}