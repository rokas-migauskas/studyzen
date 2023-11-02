﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudyZen.Application.Repositories;

namespace StudyZen.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private bool _disposed;

    public ICourseRepository Courses { get; }
    public ILectureRepository Lectures { get; }
    public IFlashcardSetRepository FlashcardSets { get; }
    public IFlashcardRepository Flashcards { get; }
    public IQuizRepository Quizzes { get; }
    public IQuizQuestionRepository QuizQuestions { get; }
    public IQuizAnswerRepository QuizAnswers { get; }

    // TODO: change these to save changes interceptors after lab 2
    // https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
    public Action<object> OnInstanceAdded = delegate { };
    public Action<object> OnInstanceUpdated = delegate { };

    public UnitOfWork(
        ApplicationDbContext dbContext,
        ICourseRepository courses,
        ILectureRepository lectures,
        IFlashcardSetRepository flashcardSets,
        IFlashcardRepository flashcards,
        IQuizRepository quizzes,
        IQuizQuestionRepository quizQuestions, 
        IQuizAnswerRepository quizAnswers)
    {
        _dbContext = dbContext;

        Courses = courses;
        Lectures = lectures;
        FlashcardSets = flashcardSets;
        Flashcards = flashcards;
        Quizzes = quizzes;
        QuizQuestions = quizQuestions;
        QuizAnswers = quizAnswers;

        OnInstanceAdded += AuditableEntityInterceptor.SetCreateStamp;
        OnInstanceUpdated += AuditableEntityInterceptor.SetUpdateStamp;
    }

    public async Task<int> SaveChanges()
    {
        var entries = _dbContext.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                OnInstanceAdded(entry.Entity);
            }
            else if (entry.State == EntityState.Modified)
            {
                OnInstanceUpdated(entry.Entity);
            }
        }

        return await _dbContext.SaveChangesAsync();
    }

    public IDbContextTransaction BeginTransaction() => _dbContext.Database.BeginTransaction();

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _dbContext.Dispose();
        _disposed = true;
    }
}