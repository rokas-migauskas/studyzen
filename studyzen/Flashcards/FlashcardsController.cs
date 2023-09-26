using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.Flashcards.Requests;

namespace StudyZen.Flashcards;

[ApiController]
[Route("flashcards")]
public sealed class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;

    public FlashcardsController(IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpPost]
    public IActionResult CreateFlashcard([FromBody] CreateFlashcardRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var newFlashcard = _flashcardService.CreateFlashcard(request);
        return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = newFlashcard.Id }, newFlashcard);
    }

    [HttpGet]
    [Route("{flashcardId}")]
    public IActionResult GetFlashcard(int flashcardId)
    {
        var flashcard = _flashcardService.GetFlashcardById(flashcardId);
        return flashcard is null ? NotFound() : Ok(flashcard);
    }

    [HttpGet]
    public IActionResult GetFlashcardsBySetId(int flashcardSetId)
    {
        return Ok(_flashcardService.GetFlashcardsBySetId(flashcardSetId));
    }

    [HttpPatch]
    [Route("{flashcardId}")]
    public IActionResult UpdateFlashcardById(int flashcardId, [FromBody] UpdateFlashcardRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedFlashcard = _flashcardService.UpdateFlashcard(flashcardId, request);
        return updatedFlashcard is null ? NotFound() : Ok(updatedFlashcard);
    }

    [HttpDelete("{flashcardId}")]
    public IActionResult DeleteFlashcard(int flashcardId)
    {
        _flashcardService.DeleteFlashcard(flashcardId);
        return NoContent();
    }
}