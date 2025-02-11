using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models;
using OnlyBans.Backend.Models.Posts;
using OnlyBans.Backend.Models.Rules;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RuleController(AppDbContext context) : ControllerBase {
    
    [HttpGet(Name = "getRules")]
    public async Task<ActionResult<IEnumerable<Rule>>> GetRules() {
        var rules = await context.Rules.ToListAsync();
        return Ok(rules);
    }

    [HttpGet("{id:guid}", Name = "getRule")]
    public async Task<ActionResult<Rule>> GetRule(Guid id)
    {
        var rule = await context.Rules.FindAsync(id);
        if (rule == null)
            return NotFound();

        return Ok(rule);
    }
    
    [HttpGet("titleRules")]
    public async Task<ActionResult<List<string>>> GetTitleRules()
    {
        var titleRules = await context.Rules
            .Where(r => r.RuleCategory == RuleEnum.titleRule.ToString())
            .Select(r => r.Text)
            .ToListAsync();

        return Ok(titleRules);
    }
    
    [HttpPost]
    public async Task<ActionResult<Rule>> CreateRule([FromBody] Rule rule) {
        if (!Enum.IsDefined(typeof(RuleEnum), rule.RuleCategory)) 
            return BadRequest(new { message = "Invalid rule category! Must be either \"titleRule\" or \"contentRule\"." });
        
        context.Rules.Add(rule);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRule), new { id = rule.Id }, rule);
    }

}