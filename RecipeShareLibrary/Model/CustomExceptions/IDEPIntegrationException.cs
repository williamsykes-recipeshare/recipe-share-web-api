using System;

namespace RecipeShareLibrary.Model.CustomExceptions;

public class IDEPIntegrationException(string message) : Exception(message);