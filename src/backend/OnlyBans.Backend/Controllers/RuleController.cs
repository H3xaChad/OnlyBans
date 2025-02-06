using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Rules;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RuleController(AppDbContext context) : ControllerBase {
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rule>>> GetRules() {
        var rules = await context.Rules.ToListAsync();
        return Ok(rules);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Rule>> GetRule(Guid id)
    {
        var rule = await context.Rules.FindAsync(id);
        if (rule == null)
            return NotFound();

        return Ok(rule);
    }
}