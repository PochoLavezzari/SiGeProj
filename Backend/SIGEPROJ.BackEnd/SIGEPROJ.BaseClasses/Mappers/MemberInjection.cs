using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SIGEPROJ.BaseClasses.Mappers.AutoMappingStrategies;
using SIGEPROJ.BaseClasses.Mappers.ValueInjecter;

namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// Clase <see cref="MemberInjection"/>
    /// </summary>
    public abstract class MemberInjection
    {
        /// <summary>
        /// Lista de estrategias que permiten realizar el mapeo de una propiedad a otra
        /// </summary>
        protected readonly static List<IAutoMappingConfigurationStrategy> MappingConfigurationStrategies
            = new List<IAutoMappingConfigurationStrategy>
                  {
                      new SameNameAndTypeConfigurationStrategy(),
                      new SameNameAndTypeWithConverterConfigurationStrategy()
                  };

        /// <summary>
        /// Dictionary to store the mapping configuration for each property
        /// </summary>
        private readonly Dictionary<string, MappingConfiguration> _mappingDictionary = new Dictionary<string, MappingConfiguration>();


        /// <summary>
        /// Obtiene la configuración de los mappings.
        /// </summary>
        /// <remarks>
        /// Este diccionario está representado de la siguiente manera:
        /// <para>Key: Nombre de la propiedad del target.</para>
        /// <para>Value: Configuración que permite cargar la propiedad del Target en base al Source.</para>
        /// </remarks>
        public Dictionary<string, MappingConfiguration> MappingConfigurations
        {
            get { return _mappingDictionary; }
        }

        /// <summary>
        /// Agrega una estrategia de automapping.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        public static void AddAutomappingConfigurationStrategy(IAutoMappingConfigurationStrategy strategy)
        {
            // Agrega al principio para que tenga mayor jerarquía
            MappingConfigurationStrategies.Insert(0, strategy);
        }
    }
    /// <summary>
    /// The MemberInjection class can be used to generate injection classes that uses the AutoMapper configuration
    /// </summary>
    /// <typeparam name="TSource">Source Type</typeparam>
    /// <typeparam name="TTarget">Target Type</typeparam>
    public class MemberInjection<TSource, TTarget> : MemberInjection, IConfigurableMemberInjection, IMemberInjection
    {
        private Func<TSource,TTarget> _convertUsingFunc;
        private Func<TSource, TTarget> _funcConstructor;
        private Action<TSource, TTarget> _afterFunction;
        private Action<TSource, TTarget> _beforeFunction;

        /// <summary>
        /// Determines whether [has custom constructor].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has custom constructor]; otherwise, <c>false</c>.
        /// </returns>
        internal bool HasCustomConstructor()
        {
            return _funcConstructor != null;
        }

        /// <summary>
        /// Constructs el source especificado.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        internal TTarget Construct(TSource source)
        {
            return _funcConstructor(source);
        }

        /// <summary>
        /// Map desde un objeto source a un objeto target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>
        /// El objeto target
        /// </returns>
        public virtual object Map(object source, object target)
        {
            // Si tiene un Custom converter, lo utiliza
            if (_convertUsingFunc != null)
                return _convertUsingFunc((TSource)source);
            // Ejecuta las acciones que deben efectuarse antes de comenzar a mapear
            if(_beforeFunction != null)
                _beforeFunction((TSource)source, (TTarget)target);

            // Realiza el mapeo de todos los componentes
            Inject(source, target);

            // Ejecuta las acciones que deben efectuarse luego de finalizar el mapeo.
            if (_afterFunction != null)
                _afterFunction((TSource)source, (TTarget)target);
            return target;
        }


        /// <summary>
        /// Converts the using.
        /// </summary>
        /// <param name="func">The func.</param>
        public void ConvertUsing(Func<TSource,TTarget> func)
        {
            _convertUsingFunc = func;
        }

        /// <summary>
        /// Permite indicar con cual función se construirá el Target
        /// </summary>
        /// <param name="func">The func.</param>
        public MemberInjection<TSource, TTarget> ConstructUsing(Func<TSource, TTarget> func)
        {
            _funcConstructor = func;
            return this;
        }

        /// <summary>
        /// Afters the map.
        /// </summary>
        /// <param name="after">The after.</param>
        /// <returns></returns>
        public MemberInjection<TSource, TTarget> AfterMap(Action<TSource, TTarget> after)
        {
            _afterFunction = after;
            return this;
        }

        /// <summary>
        /// Afters the map.
        /// </summary>
        /// <param name="before">The before.</param>
        /// <returns></returns>
        public MemberInjection<TSource, TTarget> BeforeMap(Action<TSource, TTarget> before)
        {
            _beforeFunction = before;
            return this;
        }

        /// <summary>
        /// Permite indicar con cual función se construirá el Target
        /// </summary>
        /// <param name="func">The func.</param>
        public MemberInjection<TSource, TTarget> ConstructUsing(Func<TTarget> func)
        {
            _funcConstructor = source => func();
            return this;
        }

        /// <summary>
        /// If true, all unmapped properties are going to be ignored
        /// </summary>
        private bool _ignoreUnmappedProperties;

        /// <summary>
        /// Matches the specified c.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="configuration">The matched configuration.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        internal bool Match(ConventionInfo c, MappingConfiguration configuration, out string error)
        {
            //if (!MappingConfigurations.TryGetValue(c.TargetProp.Name, out configuration))
            //{
            //    error = ignoreUnmappedProperties ? null : "No mapping implemented for " + c.TargetProp.Name;
            //    return false;
            //}

            error = null;

            // Ok, we should ignore the mapping
            if (configuration is IgnoreMapping)
                return false;

            return true;
        }

        /// <summary>
        /// Injects el source especificado.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        protected virtual void Inject(object source, object target)
        {
            // TODO: En lugar de hacerlo por cada property, recorrer cada Mapping
            //var targetProps = target.GetProps();

            var ci = new ConventionInfo
                         {
                             Source =
                                 {
                                     Type = source != null ? source.GetType() : typeof(TSource),
                                     Value = source
                                 },
                             Target =
                                 {
                                     Type = target != null ? target.GetType() : typeof(TTarget),
                                     Value = target
                                 }
                         };

            StringBuilder errors = null;

            var mappings = GetMappingInOrder();
            int count = mappings.Count;
            for (var j = 0; j < count; j++ )
            {
                // Castea el mapping
                var mapping = mappings[j];

                var mappingConfiguration = mapping.Value;

                ci.TargetProp.Name = mapping.Key;
               
                // Setea el Type de la property target
                ci.TargetProp.Type = mappingConfiguration.TargetProperty.Type;
                
                // try to match the traget property
                string error;
                //MappingConfiguration configuration;
                if (Match(ci, mappingConfiguration, out error))

                    try
                    {
                        SetValue(mappingConfiguration, ci);
                    }
                    catch(Exception ex)
                    {
                        //Console.WriteLine("ERROR: " + targetPropertyName);
                        //Console.WriteLine("ERROR: " + ex);
                        throw new NotImplementedException("Error seteado el valor del Target", ex);
                    }
                    //t.SetValue(target, SetValue(configuration, ci));
                else
                {
                    if (error != null)
                    {
                        errors = errors ?? new StringBuilder();
                        errors.AppendLine(error);
                    }
                }
            }

            // throw the aggregated exceptions
            if (errors != null)
                throw new NotImplementedException(errors.ToString());
        }

        /// <summary>
        /// Devuelve todos los mappings en orden
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<string,MappingConfiguration>> GetMappingInOrder()
        {
            var t = from key in MappingConfigurations
                    orderby key.Value.Order ascending
                        select key;
            return t.ToList();
        }

        /// <summary>
        /// Ignores the unmapped properties.
        /// </summary>
        /// <returns>MemberInjection class itself to allow fluent style</returns>
        public MemberInjection<TSource, TTarget> IgnoreUnmappedProperties()
        {
            _ignoreUnmappedProperties = true;
            return this;
        }

        /// <summary>
        /// Indica que el mapping debe hacerse en ambos sentidos.
        /// </summary>
        /// <returns>MemberInjection class itself to allow fluent style</returns>
        public MemberInjection<TSource, TTarget> TwoWayMapping()
        {
            return TwoWayMapping(null);
        }

        /// <summary>
        /// Twoes the way mapping.
        /// </summary>
        /// <param name="customMapperAction">The custom mapper action.</param>
        /// <returns></returns>
         public MemberInjection<TSource, TTarget> TwoWayMapping(Action<MemberInjection<TTarget,TSource>> customMapperAction)
         {
             var mapper = Mapper.CreateMap<TTarget, TSource>();

             // Recorre todos los mappings
            ProcessTwoWayMapping(mapper,  GetMappingInOrder().Select(x => x.Value), true);

             if (customMapperAction != null)
                 customMapperAction(mapper);

             return this;
         }

         /// <summary>
         /// Processes the two way mapping.
         /// </summary>
         /// <param name="mapper">The mapper.</param>
         /// <param name="mappingConfigurations">The mapping configurations.</param>
         /// <param name="includeThisConfiguration">Si es <c>true</c> [include this configuration].</param>
        private void ProcessTwoWayMapping(
            MemberInjection<TTarget, TSource> mapper,
            IEnumerable<MappingConfiguration> mappingConfigurations,
            bool includeThisConfiguration)
        {
            // Recorre todos los mappings
            foreach (var innerMappingConfiguration in mappingConfigurations)
            {
                var thisConfiguration = innerMappingConfiguration as ThisConfiguration;
                if(thisConfiguration != null && includeThisConfiguration)
                {
                    // Sólo se incluye ThisConfiguration para el primer nivel.
                    ProcessTwoWayMapping(mapper, thisConfiguration.InnerMappingConfigurations.Values, false);
                    continue;
                }
                // Excluye las propiedades de tipo OneWay
                var innerOneWayConfig = innerMappingConfiguration as IOneWayConfiguration;
                if (innerOneWayConfig != null && innerOneWayConfig.IsOneWay)
                    continue;

                // Agrega aquellas propiedades de tipo TwoWay que además, permitan ser convertidas
                var innerTwoWayMapping = innerMappingConfiguration as ITwoWayMappingConverter;
                if (innerTwoWayMapping != null && innerTwoWayMapping.CanConvert())
                {
                    var mapping = innerTwoWayMapping.ConvertToInverse();
                    mapper.MappingConfigurations[mapping.TargetProperty.Name] = mapping;
                }
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="configuration">The Configuration</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        internal object SetValue(MappingConfiguration configuration, ConventionInfo c)
        {
            // Evaluate the configuration
           // var configurationType = configuration.GetType();
            //var targetType = c.TargetProp.Type;


            var setValueConfiguration = configuration as IValueInjection;
            if( setValueConfiguration != null )
            {

                return setValueConfiguration.Map(c.Source.Value, c.Target.Value);
            }

            // something went completly wrong
            throw new NotSupportedException();
        }



        /// <summary>
        /// Asserts the configuration.
        /// </summary>
        public void AssertConfigurationIsValid()
        {
            var target = ObjectCreator.Create(typeof(TTarget));
            var source = ObjectCreator.Create(typeof(TSource));
            Inject(source, target);
        }

        /// <summary>
        /// Applies a mapping configuration for a property
        /// </summary>
        /// <typeparam name="TPropertyTarget">The type of the target property.</typeparam>
        /// <param name="targetProp">The target property</param>
        /// <param name="configuration">The mapping configuration.</param>
        /// <returns>MemberInjection class itself to allow fluent style</returns>
        public MemberInjection<TSource, TTarget> ForMember<TPropertyTarget>(
            Expression<Func<TTarget, TPropertyTarget>> targetProp,
            Func<MappingFactory<TSource, TTarget, TPropertyTarget>,
                MappingConfiguration> configuration)
        {
            // Add a new mapping configuration
            var mappingFactory = new MappingFactory<TSource, TTarget, TPropertyTarget>
                (
                    PropInfo.FillProperty(targetProp)
                );
            var targetProperty = mappingFactory.TargetProperty;
            
            // Ejecuta la configuración
            var config = configuration(mappingFactory);

            // Agrega el Orden
            config.Order = MappingConfigurations.Count;

            // Agrega la property al diccionario
            MappingConfigurations[targetProperty.Name] = config;
            return this;
        }

         /// <summary>
        /// Permite agregar el mapeo contra otro objeto del que forma parte.
        /// </summary>
        /// <returns></returns>
        public MemberInjection<TSource, TTarget> ForTarget(
             Func<MappingFactoryForTargetMember<TSource, TTarget>, MappingConfiguration> configuration
            )
        {
            // Add a new mapping configuration
            Expression<Func<TTarget, TTarget>> expression = target => target;

            var mappingFactory = new MappingFactoryForTargetMember<TSource, TTarget>(
                PropInfo.FillProperty(expression)
                );

            var targetProperty = mappingFactory.TargetProperty;

            // Ejecuta la configuración
            var config = configuration(mappingFactory);

            if (!MappingConfigurations.ContainsKey("this"))
            {
                MappingConfigurations[targetProperty.Name] = new ThisConfiguration(typeof(TTarget));
            }

            var thisConfiguration = (ThisConfiguration)MappingConfigurations["this"];

            // Agrega el Orden
            config.Order = thisConfiguration.InnerMappingConfigurations.Count;
            // Agrega la property al diccionario

            // Se agrega:   El Type desde el cual se obtendrá el mapper
            //              La configuración encontrada.
            thisConfiguration.InnerMappingConfigurations.Add(
                config.SourceProperty.Type,
                config);
            return this;
        }

        /// <summary>
        /// Mapea todos las properties de Target desde Source
        /// </summary>
        /// <returns></returns>
        public MemberInjection<TSource, TTarget> MapUnmappedProperties()
        {
            // Recorrer todas las propiedades de Source y generar su mapping
            var targetProps = typeof(TTarget).GetInfos().ToList();

            var ci = new ConventionInfo
            {
                Source =
                {
                    Type = typeof(TSource),
                    Value = null
                },
                Target =
                {
                    Type = typeof(TTarget),
                    Value = null
                }
            };

            // walk through all target properties
            foreach (var t in targetProps)
            {
                ci.TargetProp.Name = t.Name;
                ci.TargetProp.Type = t.PropertyType;

                // Si ya contiene la clave, no la agrega, fue mapeada manualmente.
                if (MappingConfigurations.ContainsKey(t.Name))
                    continue;

                // Recorre las estrategias hasta encontrar alguna que concuerde

                var info = new AutoMappingConventionInfo
                               {TargetPropertyInfo = t, SourceType = typeof (TSource), TargetType = typeof (TTarget)};
                foreach (var mappingConfigurationStrategy in MappingConfigurationStrategies)
                {
                    MappingConfiguration configuration;
                    if (mappingConfigurationStrategy.Match(info, out configuration))
                    {
                        // Add a new mapping configuration
                        MappingConfigurations.Add(t.Name, configuration);
                        break;
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("MemberInjection[\"{0}\",\"{1}\"]", typeof(TTarget).FullName, typeof(TSource).FullName);
        }
    }
}