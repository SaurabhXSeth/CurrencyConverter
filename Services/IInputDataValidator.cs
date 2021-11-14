using Models;

namespace Services
{
    public interface IInputDataValidator
    {
        ValidationResult ValidateInputData(ConversionQuery query);
       
    }
}
