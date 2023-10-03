using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public interface IFlashcardSetService
{
    FlashcardSet CreateFlashcardSet(CreateFlashcardSetDto dto);
    FlashcardSet? GetFlashcardSetById(int flashcardSetId);
    IReadOnlyCollection<FlashcardSet> GetAllFlashcardSets();
    IReadOnlyCollection<FlashcardSet> GetFlashcardSetsByLectureId(int? lectureId);
    bool UpdateFlashcardSet(int flashCardSetId, UpdateFlashcardSetDto dto);
    bool DeleteFlashcardSet(int flashcardSetId);
}