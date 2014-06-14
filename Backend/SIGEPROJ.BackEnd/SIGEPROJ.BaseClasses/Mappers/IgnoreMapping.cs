namespace SIGEPROJ.BaseClasses.Mappers
{
    /// <summary>
    /// This mapping will ignore the property
    /// </summary>
    public class IgnoreMapping : MappingConfiguration
    {
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string target = TargetProperty.Name;
            return "IgnoreMapping[target."+ target +"]";
        }
    }
}