using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

public class ImageController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IFileService _fileService;

    public ImageController(NeoQuasarDBContext dbContext, IFileService fileService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
    }

    public IActionResult Get(string? filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            return NotFound();
        }
        FileData file = _fileService.GetFileDataByName(filename);
        if (file == null)
        {
            return NotFound();
        }
        
        var content = _fileService.GetFileContent(file.FileID);
        if (content is null || content.Length == 0)
        {
            return NotFound();
        }

        var contentType = string.IsNullOrWhiteSpace(file.MIMEType) ? "application/octet-stream" : file.MIMEType;

        return File(content,contentType);
    }
}