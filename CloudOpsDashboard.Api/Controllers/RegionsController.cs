using CloudOpsDashboard.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudOpsDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RegionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRegions()
    {
        var regions = await _context.Regions.ToListAsync();
        return Ok(regions);
    }
}