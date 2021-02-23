using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
                                    IGenericRepository<ProductBrand> productBrandRepo,
                                    IGenericRepository<ProductType> productTypeRepo,
                                    IMapper mapper)
        {
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);

            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }

    // OLD
        // [HttpGet("{id}")]
        // //public async Task<ActionResult<Product>> GetProduct(int id)
        // public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        // {
        //     //var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        //     //var product = await _productsRepo.GetByIdAsync(id);

        //     var spec = new ProductsWithTypesAndBrandsSpecification(id);
        //     var product = await _productsRepo.GetEntityWithSpec(spec);

        //     //return Ok(product);

        //     // return new ProductToReturnDto
        //     // {
        //     //     Id = product.Id,
        //     //     Name = product.Name,
        //     //     Description = product.Description,
        //     //     PictureUrl = product.PictureUrl,
        //     //     Price = product.Price,
        //     //     ProductBrand = product.ProductBrand.Name,
        //     //     ProductType = product.ProductType.Name
        //     // };

        //     return Ok(_mapper.Map<Product, ProductToReturnDto>(product));


        // }


        // [HttpGet]
        // //public async Task<ActionResult<List<Product>>> GetProducts()
        // public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        // {
        //     //var products = await _productsRepo.ListAllAsync();

        //     var spec = new ProductsWithTypesAndBrandsSpecification();
            
        //     var products = await _productsRepo.ListAsync(spec);

        //     // return Ok(products);

        //     // return products.Select(product => new ProductToReturnDto
        //     // {
        //     //     Id = product.Id,
        //     //     Name = product.Name,
        //     //     Description = product.Description,
        //     //     PictureUrl = product.PictureUrl,
        //     //     Price = product.Price,
        //     //     ProductBrand = product.ProductBrand.Name,
        //     //     ProductType = product.ProductType.Name
        //     // }).ToList();

        //     return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        // }


    }
}