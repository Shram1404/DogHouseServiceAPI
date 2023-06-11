using System.ComponentModel.DataAnnotations;

namespace DogHouseServiceAPI.Dto
{
    // Parameter object for the GetDogs method in the DogsController class
    public class DogsGetRequest
    {
        public string? Attribute { get; set; }
        public string? Order { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page can not be less or equal zero")]
        public int PageNumber { get; set; } = 1;
        [Range(1, int.MaxValue, ErrorMessage = "Page size can not be less or equal zero")]
        public int PageSize { get; set; } = 10;
    }
}
