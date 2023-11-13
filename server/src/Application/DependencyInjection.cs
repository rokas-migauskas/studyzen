﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Services;
using StudyZen.Application.Validation;

namespace StudyZen.Application;

public static class DependencyInjection
{
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
                services.AddScoped<ICourseService, CourseService>();
                services.AddScoped<ILectureService, LectureService>();
                services.AddScoped<IFlashcardService, FlashcardService>();
                services.AddScoped<IFlashcardSetService, FlashcardSetService>();
                services.AddScoped<IQuizService, QuizService>();
                services.AddScoped<ITokenManagementService, TokenManagementService>();
                services.AddScoped<IQuizGameService, QuizGameService>();
                services.AddScoped<IApplicationUserService, ApplicationUserService>();
                services.AddScoped<IDataImporter, CsvDataImporter>();
                services.AddScoped<IFlashcardImporter, FlashcardImporter>();
                services.AddScoped<IUserContextService, UserContextService>();
                services.AddScoped<ValidationHandler>();
                services.AddHttpContextAccessor();

                services.AddValidatorsFromAssemblyContaining<CreateLectureRequestValidator>();

                return services;
        }
}