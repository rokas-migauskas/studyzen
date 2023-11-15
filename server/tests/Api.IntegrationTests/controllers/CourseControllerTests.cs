﻿using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using StudyZen.Application.Dtos;


namespace Api.IntegrationTests.controllers;

[TestFixture]
public class CourseControllerTests
{
    private readonly HttpClient _httpClient;
    private readonly CreateCourseDto _createCourseDto;
    private readonly UpdateCourseDto _updateCourseDto;

    public CourseControllerTests()
    {
        var factory = new WebApplicationFactory<Program>();
        _httpClient = factory.CreateClient();
        _createCourseDto = new CreateCourseDto("Test name", "Test desc");
        _updateCourseDto = new UpdateCourseDto("New name", "New desc");
    }

    [Test]
    public async Task CreateCourse()
    {
        var response = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var course = await response.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.AreEqual(_createCourseDto.Name, course.Name);
        Assert.AreEqual(_createCourseDto.Description, course.Description);
    }

    [Test]
    public async Task GetCourse()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var course = await response.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.AreEqual(newCourse.Name, course.Name);
        Assert.AreEqual(newCourse.Description, course.Description);
    }

    [Test]
    public async Task GetAllCourses()
    {
        var response = await _httpClient.GetAsync("Courses");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var allCourses = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<CourseDto>>();
        Assert.NotNull(allCourses);
    }

    [Test]
    public async Task UpdateCourse()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.PatchAsJsonAsync($"Courses/{newCourse.Id}", _updateCourseDto);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        var course = await getResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.AreEqual(_updateCourseDto.Name, course.Name);
        Assert.AreEqual(_updateCourseDto.Description, course.Description);
    }

    [Test]
    public async Task DeleteCourse()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.DeleteAsync($"Courses/{newCourse.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, getResponse.StatusCode);
    }
}