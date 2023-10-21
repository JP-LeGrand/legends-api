namespace legend.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using legend.Authorization;
using legend.Helpers;
using legend.Models.Address;
using legend.Services;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    private IAddressService _addressService;

    public AddressController(
        IAddressService addressService)
    {
        _addressService = addressService;
    }

    [AllowAnonymous]
    [HttpPost("add")]
    public IActionResult Register(AddAddressRequest model)
    {
        _addressService.Add(model);
        return Ok(new { message = "Add successful" });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var addresses = _addressService.GetAll();
        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var address = _addressService.GetById(id);
        return Ok(address);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, UpdateAddressRequest model)
    {
        _addressService.Update(id, model);
        return Ok(new { message = "Address updated successfully" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _addressService.Delete(id);
        return Ok(new { message = "Address deleted successfully" });
    }
}


