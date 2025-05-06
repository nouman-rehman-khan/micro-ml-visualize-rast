using Microsoft.AspNetCore.Mvc;
using MicroMLASTVisualizer.Models;
using System;

namespace MicroMLASTVisualizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ASTController : ControllerBase
    {
        [HttpPost]
        public IActionResult ParseCode([FromBody] CodeRequest request)
        {
            try
            {
                var parser = new Parser();
                var ast = parser.Parse(request.Code);
                return Ok(ast);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class CodeRequest
    {
        public string Code { get; set; }
    }
}