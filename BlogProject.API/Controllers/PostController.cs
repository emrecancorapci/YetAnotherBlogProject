﻿using BlogProject.Business.Services.CommentService;
using BlogProject.Business.Services.PostService;
using BlogProject.Business.Services.PostService.Dtos;
using BlogProject.Business.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;

    public PostController(
        IPostService postService,
        IUserService userService,
        ICommentService commentService)
    {
        _postService = postService;
        _userService = userService;
        _commentService = commentService;
    }

    // GET
    [HttpGet("Get")]
    public async Task<IActionResult> Get(int id)
    {
        if (id == 0) return BadRequest();
        if (!await _postService.IsExistAsync(id)) return NotFound();
        
        var response = await _postService.GetAsync(id);

        return Ok(response);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var responseList = await _postService.GetAllAsync();

        return Ok(responseList);
    }
    [HttpGet("GetAllByUser")]
    public async Task<IActionResult> GetAllByUser(int userId)
    {
        if (userId == 0) return BadRequest();
        
        var responseList = await _postService.GetAllByUserIdAsync(userId);

        return Ok(responseList);
    }
    [HttpGet("GetAllByTag")]
    public async Task<IActionResult> GetAllByTag(int tagId)
    {
        if (tagId == 0) return BadRequest();
        
        var responseList = await _postService.GetAllByTagIdAsync(tagId);

        return Ok(responseList);
    }
    [HttpGet("GetAllByCategory")]
    public async Task<IActionResult> GetAllByCategory(int categoryId)
    {
        if (categoryId == 0) return BadRequest();
        
        var responseList = await _postService.GetAllByCategoryIdAsync(categoryId);

        return Ok(responseList);
    }
    [HttpGet("GetAllByEditor")]
    public async Task<IActionResult> GetAllByEditor(int editorId)
    {
        if (editorId == 0) return BadRequest();
        
        var responseList = await _postService.GetAllByEditorIdAsync(editorId);

        return Ok(responseList);
    }
    [HttpGet("IsExist")]
    public async Task<IActionResult> IsExist(int postId)
    {
        if (postId == 0) return BadRequest();
        
        var response = await _postService.IsExistAsync(postId);

        return Ok(response);
    }

    // POST
    [HttpPost("Add")]
    public async Task<IActionResult> Add(AddPostRequest request)
    {
        var affectedRows = await _postService.AddAsync(request);

        return Ok(affectedRows);
    }
    [HttpPost("AddTag")]
    public async Task<IActionResult> AddTag(string name, string desc)
    {
        var affectedRows = await _postService.AddTagAsync(name,desc);

        return Ok(affectedRows);
    }
    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory(string name, string desc)
    {
        var affectedRows = await _postService.AddCategoryAsync(name,desc);

        return Ok(affectedRows);
    }
    

    // PATCH
    [HttpPatch("UpdateContent")]
    public async Task<IActionResult> UpdateContent(UpdatePostContentRequest request)
    {
        var affectedRows = await _postService.UpdateContentAsync(request);
        // TODO : Wants categoryId. Find a way to change only modified variables.
        return Ok(affectedRows);
    }
    [HttpPatch("React")]
    public async Task<IActionResult> React(ReactionPostRequest request)
    {
        var affectedRows = await _postService.ReactAsync(request);

        return Ok(affectedRows);
    }

    // DELETE
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int postId)
    {
        var affectedRows = await _postService.DeleteAsync(postId);

        return Ok(affectedRows);
    }
}