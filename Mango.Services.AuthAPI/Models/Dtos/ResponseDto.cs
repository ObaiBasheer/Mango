﻿namespace Mango.Services.AuthAPI.Models.Dtos
{
    public record ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public object? Result { get; set; }

    }
}
