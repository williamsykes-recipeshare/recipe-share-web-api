using System;

namespace RecipeShareLibrary.Model.CustomExceptions;

public class BadRequestException(string message) : Exception(message);