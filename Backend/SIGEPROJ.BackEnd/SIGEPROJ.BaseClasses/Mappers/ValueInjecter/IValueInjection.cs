namespace SIGEPROJ.BaseClasses.Mappers.ValueInjecter
{
    /// <summary>
    /// Permite pasar de una clase source a una clase Target
    /// </summary>
    public interface IValueInjection
    {
        /// <summary>
        /// Map desde un objeto source a un objeto target.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>El objeto target</returns>
        object Map(object source, object target);
    }
}