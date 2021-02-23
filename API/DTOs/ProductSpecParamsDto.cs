using Microsoft.AspNetCore.Mvc;

namespace API.DTOs
{
           //public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParamsDto productParamsDto)
        //public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery(Name="typeId")]ProductSpecParams productParams)
    
    public class ProductSpecParamsDto
    {
        [FromQuery(Name = "brandId")]
        public int? BrandId { get; set; }

        [FromQuery(Name = "typeId")]
        public int? TypeId { get; set; }

        [FromQuery(Name = "sort")]
        public string Sort { get; set; }
    }
}