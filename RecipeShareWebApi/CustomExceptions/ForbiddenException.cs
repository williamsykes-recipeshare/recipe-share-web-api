namespace RecipeShareWebApi.CustomExceptions;

public class ForbiddenException(string message) : Exception(message);