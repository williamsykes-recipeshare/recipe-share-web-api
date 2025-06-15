namespace RecipeShareLibrary.Model.CustomExceptions;

public class ForbiddenException(string message) : Exception(message);