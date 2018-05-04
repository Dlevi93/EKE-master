using System.Collections.Generic;
using static EKE_WebApi.Controllers.ValuesController;

namespace EKE_WebApi.Models.Registration
{
    public class TripResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Length { get; set; }
        public decimal Time { get; set; }
        public string Age { get; set; }
        public string Elevation { get; set; }
        public decimal Price { get; set; }
        public TripCategoryResponse Category { get; set; }
        public List<TripAttributesResponse> Attributes { get; set; }
        public TripDifficultyResponse Difficulty { get; set; }
        public SpotsResponse Spots { get; set; }
        public SpotsResponse RemainingSpots { get; set; }
    }
}
