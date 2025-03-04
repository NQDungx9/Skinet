using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;
    public ProductController(IGenericRepository<Product> productRepo,
    IGenericRepository<ProductBrand> productBrandRepo,
    IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _productBrandRepo = productBrandRepo;
        _productTypeRepo = productTypeRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProduct()
    {
        var spec = new ProductsWithTypesAndBrandsSpecification();

        var products = await _productRepo.ListAsync(spec);
        return Ok(_mapper
        .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);
        var product = await _productRepo.GetEntityWithSpec(spec);
        return _mapper.Map<Product, ProductToReturnDto>(product);
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
}
