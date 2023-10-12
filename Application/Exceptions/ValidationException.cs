using FluentValidation.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Exceptions;

public class QuizValidationException : Exception
{
    public InvalidFields[] Errors { get; }
    public QuizValidationException(string? message, List<ValidationFailure> errors) : base(message)
    {
        Errors = new InvalidFields[errors.Count];

        for (int i = 0; i < errors.Count; i++)
        {
            Errors[i] = new InvalidFields(errors[i].PropertyName, errors[i].ErrorMessage);
        }
    }
    public class InvalidFields
    {
        public string Field { get; }
        public string Message { get; }
        public InvalidFields(string field,string message)
        {
            Field = field;
            Message = message;
        }
    }
}
