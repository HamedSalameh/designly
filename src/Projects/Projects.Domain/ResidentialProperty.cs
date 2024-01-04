namespace Projects.Domain
{
    /// <summary>
    /// Supported types are: House, Apartment, Condominium, Duplex
    /// 
    /// House: Single-family dwelling.
    /// Apartment Building: A building with multiple residential units.
    /// Condominium: An individually owned unit within a larger complex. (Shared House)
    /// Duplex: A building with two residential units.
    /// </summary>
    public class ResidentialProperty : Property
    {
        public ResidentialProperty()
        {
            throw new NotImplementedException();
        }
    }
}
