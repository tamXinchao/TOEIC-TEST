using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[ApiController]
[Route("api/[controller]")]
public class StickerApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StickerApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<StickerDto>> Get()
    {
        return _context.Stickers
            .Where(s => s.IsActive == true && s.IsDelete == false)
            .Select(d => new StickerDto
            {
                StickerId = d.StickerId,
                StickerName = d.StickerName,
                IsActive = d.IsActive,
                IsDelete = d.IsDelete,
                StickerOfTestCount = _context.StickerOfTests
                    .Where(soc => soc.IsActive == true && soc.IsDelete ==false)
                    .Count(c => c.StickerId == d.StickerId)
            })
            .ToList();
        
    }
}