using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Database;
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
    
    [HttpGet("{id:guid}", Name = "getRule")]
    public async Task<ActionResult<RuleGetDto>> GetRule(Guid id) {
        var rule = await context.Rules.FindAsync(id);
        if (rule == null)
            return NotFound();

        return Ok(new RuleGetDto(rule));
    }
    
    [Authorize]
    [HttpGet("titleRules")]
    public async Task<ActionResult<IEnumerable<string>>> GetTitleRulesText()
    {
        var titleRulesText = await context.Rules
            .Where(r => r.RuleCategory == RuleEnum.titleRule)
            .Select(r => r.Text)
            .ToListAsync();

        return Ok(titleRulesText);
    }
    
    [Authorize]
    [HttpGet("contentRules")]
    public async Task<ActionResult<IEnumerable<string>>> GetContentRulesText()
    {
        var contentRulesText = await context.Rules
            .Where(r => r.RuleCategory == RuleEnum.contentRule)
            .Select(r => r.Text)
            .ToListAsync();

        return Ok(contentRulesText);
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

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRule(Guid id) {
        var rule = await context.Rules.FindAsync(id);
        if (rule == null)
            return NotFound();

        context.Rules.Remove(rule);
        await context.SaveChangesAsync();

        return NoContent();
    }
}