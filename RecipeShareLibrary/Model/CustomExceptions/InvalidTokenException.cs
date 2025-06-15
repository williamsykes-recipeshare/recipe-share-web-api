namespace RecipeShareLibrary.Model.CustomExceptions;

public class InvalidTokenException(string message) : Exception(message);