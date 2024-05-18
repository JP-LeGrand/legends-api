namespace legend.Controllers;

using Microsoft.AspNetCore.Mvc;
using legend.Authorization;
using legend.Models.Address;
using legend.Services;
using Microsoft.AspNetCore.Http;

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

    [HttpPost("add")]
    public IActionResult AddAddress(AddAddressRequest model)
    {
        _addressService.Add(model);
        return Ok(new { message = "Add successful" });
    }

    [HttpGet]
    public IActionResult GetUserAddresses()
    {
        var user = HttpContext.GetUserIdFromContext();

        var addresses = _addressService.GetUserAddress(user.Id);
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


