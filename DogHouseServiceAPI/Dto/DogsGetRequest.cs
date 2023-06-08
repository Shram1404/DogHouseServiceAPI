namespace DogHouseServiceAPI.Dto
{
    // Parameter object for the GetDogs method in the DogsController class
    public class DogsGetRequest
    {
        public string? Attribute { get; set; }
        public string? Order { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
