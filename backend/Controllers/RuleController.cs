using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
public class RuleController(AppDbContext context, UserManager<User> userManager) : ControllerBase {
    
    [HttpGet(Name = "getRules")]
    public async Task<ActionResult<IEnumerable<RuleGetDto>>> GetRules() {
        var rules = await context.Rules.ToListAsync();
        return Ok(rules.Select(rule => new RuleGetDto(rule)));
    }

    [HttpGet("rulesText")]
    public async Task<ActionResult<IEnumerable<RuleTextDto>>> GetRulesText() {
        var rulesText = await context.Rules
            .Select(r => new RuleTextDto(r))
            .ToListAsync();

        return Ok(rulesText);
    }
    
    [HttpGet("{id:guid}", Name = "getRule")]
    public async Task<ActionResult<RuleGetDto>> GetRule(Guid id) {
        var rule = await context.Rules.FindAsync(id);
        if (rule == null)
            return NotFound();

        return Ok(new RuleGetDto(rule));
    }
    
    [HttpGet("titleRules")]
    public async Task<ActionResult<IEnumerable<RuleGetDto>>> GetTitleRules()
    {
        var titleRules = await context.Rules
            .Where(r => r.RuleCategory == RuleEnum.titleRule)
            .Select(r => new RuleGetDto(r))
            .ToListAsync();

        return Ok(titleRules);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Rule>> CreateRule([FromBody] RuleCreateDto ruleDto) {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();
        if (!Enum.IsDefined(typeof(RuleEnum), ruleDto.RuleCategory)) 
            return BadRequest(new { message = "Invalid rule category! Must be either \"titleRule\" or \"contentRule\"." });

        var rule = ruleDto.ToRule(user.Id);
        
        rule.CreatedAt = rule.CreatedAt.ToUniversalTime();
        
        context.Rules.Add(rule);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRule), new { id = rule.Id }, rule);
    }

}