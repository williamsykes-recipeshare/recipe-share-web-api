using System;

namespace RecipeShareLibrary.Model.CustomExceptions;

public class NotFoundException(string message) : Exception(message);